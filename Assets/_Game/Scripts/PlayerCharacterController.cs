using KinematicCharacterController;
using UnityEngine;

public struct PlayerCharacterInputs
{
    public float MoveRightAxis;
    public bool JumpPressed;
}

public class PlayerCharacterController : MonoBehaviour, ICharacterController
{
    [field: SerializeField] public KinematicCharacterMotor Motor { get; private set; }
    [field: SerializeField] public bool CanJump { get; private set; }
    
    [Header("Stable Movement")]
    [SerializeField] float maxStableMoveSpeed = 8f;
    [SerializeField] float stableMovementSharpness = 15f;
    [SerializeField] float orientationSharpness = 10f;

    [Header("Air Movement")]
    [SerializeField] float maxAirMoveSpeed = 8f;
    [SerializeField] float airAccelerationSpeed = 5f;
    [SerializeField] float airDecelerationSpeed = 5f;
    [SerializeField] float drag = 0.1f;

    [Header("Jumping")]
    [SerializeField] bool allowJumpingWhenSliding;
    [SerializeField] int maxJumpCount = 2;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpPreGroundingGraceTime = 0.2f;
    [SerializeField] float jumpPostGroundingGraceTime = 0.2f;

    [Header("Misc")]
    [SerializeField] Vector3 defaultGravity = new(0, -30f, 0);

    public Vector2 RawInputVector => _rawInputVector;
    public Vector2 MoveInputVector => _moveInputVector;

    Vector2 _rawInputVector;
    Vector3 _moveInputVector;
    Vector3 _lookInputVector;

    Vector3 _gravity;
    Vector3 _velocity;
    Vector3 _internalVelocityAdd = Vector3.zero;

    int _currentJumpCount;
    float _jumpForce;
    float _timeSinceJumpRequested = Mathf.Infinity;
    float _timeSinceLastAbleToJump;
    bool _jumpRequested;
    bool _jumpedThisFrame;
    bool _canCoyoteJump;
    bool _coyoteJumped;

    void Start ()
    {
        Motor.CharacterController = this;

        _gravity = defaultGravity;
        _jumpForce = Mathf.Sqrt(-2f * jumpHeight * _gravity.y);
    }

    public void SetInputs (ref PlayerCharacterInputs inputs)
    {
        _rawInputVector = new Vector2(inputs.MoveRightAxis, 0f);

        // Clamp raw input for movement
        Vector3 moveInputVector = Vector2.ClampMagnitude(new Vector3(_rawInputVector.x, _rawInputVector.y), 1f);

        _moveInputVector = moveInputVector;

        if (inputs.JumpPressed)
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }
    }
    
    public void BeforeCharacterUpdate (float deltaTime)
    {
        _gravity = defaultGravity;

        // Check if coyote time jump is possible
        _canCoyoteJump = _timeSinceLastAbleToJump <= jumpPostGroundingGraceTime;
    }

    public void UpdateRotation (ref Quaternion currentRotation, float deltaTime)
    {
    }

    public void UpdateVelocity (ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 targetMovementVelocity;
        
        // Ground movement logic
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            // Adjust velocity to match the ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) *
                              currentVelocity.magnitude;

            // Calculate target velocity based on input
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized *
                                      _moveInputVector.magnitude;
            
            targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

            // Smoothly interpolate the current velocity toward the target velocity
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                targetMovementVelocity,
                1 - Mathf.Exp(-stableMovementSharpness * deltaTime)
            );
        }
        // Air movement logic
        else
        {
            // Assign velocity based on air input
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                targetMovementVelocity = _moveInputVector * maxAirMoveSpeed;

                // Prevent climbing on steep slopes with air movement
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpendicularObstructionNormal = Vector3
                        .Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp)
                        .normalized;
                    
                    targetMovementVelocity =
                        Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                currentVelocity += airAccelerationSpeed * deltaTime * velocityDiff;
            }
            // Assign velocity based on air deceleration
            else
            {
                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.CharacterUp, inputRight).normalized *
                                          _moveInputVector.magnitude;

                targetMovementVelocity = reorientedInput * maxAirMoveSpeed;
                targetMovementVelocity.y = currentVelocity.y;

                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetMovementVelocity,
                    1 - Mathf.Exp(-airDecelerationSpeed * deltaTime)
                );
            }

            // Apply gravity and drag
            currentVelocity += _gravity * deltaTime;
            currentVelocity *= 1f / (1f + drag * deltaTime);
        }

        // Handle jumping
        _jumpedThisFrame = false;
        _timeSinceJumpRequested += deltaTime;
        if (_jumpRequested && CanJump)
        {
            bool evaluateGroundStatus = allowJumpingWhenSliding
                ? Motor.GroundingStatus.FoundAnyGround
                : Motor.GroundingStatus.IsStableOnGround;

            // Handle first jump & wall jump
            if (_currentJumpCount == 0 && (evaluateGroundStatus || _canCoyoteJump))
            {
                if (_canCoyoteJump && !evaluateGroundStatus)
                    _coyoteJumped = true;

                // Calculate jump direction before un-grounding
                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus is { FoundAnyGround: true, IsStableOnGround: false })
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                
                // Makes the character skip ground probing/snapping on its next update.
                // If this line weren't here, the character would remain snapped to the ground when trying to jump
                Motor.ForceUnground();

                // Add to the return velocity and reset jump state
                currentVelocity += jumpDirection * _jumpForce - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _currentJumpCount++;
                _jumpedThisFrame = true;
            }

            bool hasExtraJumps = _currentJumpCount > 0 && _currentJumpCount < maxJumpCount;
            if (!_coyoteJumped && hasExtraJumps && !evaluateGroundStatus)
            {
                Motor.ForceUnground();

                // Add to the return velocity and reset jump state
                currentVelocity += Motor.CharacterUp * _jumpForce - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _currentJumpCount++;
                _jumpedThisFrame = true;
            }
        }

        // Apply additive velocity if necessary
        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }

        _velocity = Motor.Velocity;
    }
    
    public void AfterCharacterUpdate (float deltaTime)
    {
        // Handle jump-related values
        if (_coyoteJumped && _currentJumpCount > 0)
            _coyoteJumped = false;

        if (!_canCoyoteJump && _currentJumpCount == 0)
            _currentJumpCount++;

        // Handle jumping pre-ground grace period
        if (_jumpRequested && _timeSinceJumpRequested > jumpPreGroundingGraceTime)
            _jumpRequested = false;

        // Handle jumping while sliding
        if (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
        {
            // If we're on a ground surface, reset jumping values
            if (!_jumpedThisFrame)
                _currentJumpCount = 0;

            _timeSinceLastAbleToJump = 0f;
        }
        else
            // Keep track of time since we were last able to jump (for grace period)
            _timeSinceLastAbleToJump += deltaTime;
    }
    
    
    public bool IsColliderValidForCollisions (Collider coll)
    {
        return true;
    }
    
    public void OnGroundHit (
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport
    )
    {
    }

    public void OnMovementHit (
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport
    )
    {
    }
    
    public void ProcessHitStabilityReport (
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        Vector3 atCharacterPosition,
        Quaternion atCharacterRotation,
        ref HitStabilityReport hitStabilityReport
    )
    {
    }

    public void PostGroundingUpdate (float deltaTime)
    {
        if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            HandleLanded();
        else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            HandleLeftStableGround();
    }

    public void OnDiscreteCollisionDetected (Collider hitCollider)
    {
    }

    public void AddVelocity (Vector3 velocity)
    {
        _internalVelocityAdd += velocity;
    }

    void HandleLanded ()
    {
    }

    void HandleLeftStableGround ()
    {
    }
}