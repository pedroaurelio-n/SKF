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

    [Header("Wall Sliding")]
    [SerializeField] float maxWallSlideSpeed = -2f; // Velocidade m�xima de descida ao deslizar na parede

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

    // Flag para indicar se o personagem est� deslizando na parede
    bool _isWallSliding = false;

    void Start()
    {
        Motor.CharacterController = this;
        _gravity = defaultGravity;
        _jumpForce = Mathf.Sqrt(-2f * jumpHeight * _gravity.y);
    }

    public void SetInputs(ref PlayerCharacterInputs inputs)
    {
        _rawInputVector = new Vector2(inputs.MoveRightAxis, 0f);

        // Clamp no input para movimento
        Vector3 moveInputVector = Vector2.ClampMagnitude(new Vector3(_rawInputVector.x, _rawInputVector.y), 1f);
        _moveInputVector = moveInputVector;

        if (inputs.JumpPressed)
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        _gravity = defaultGravity;
        _isWallSliding = false; // Reseta o wall sliding a cada frame
        _canCoyoteJump = _timeSinceLastAbleToJump <= jumpPostGroundingGraceTime;
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // Aqui voc� pode implementar a rota��o baseada na dire��o de movimento, se necess�rio.
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 targetMovementVelocity;

        // Movimento no ch�o
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;

            targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

            currentVelocity = Vector3.Lerp(
                currentVelocity,
                targetMovementVelocity,
                1 - Mathf.Exp(-stableMovementSharpness * deltaTime)
            );
        }
        // Movimento no ar
        else
        {
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                targetMovementVelocity = _moveInputVector * maxAirMoveSpeed;

                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpendicularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                currentVelocity += airAccelerationSpeed * deltaTime * velocityDiff;
            }
            else
            {
                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.CharacterUp, inputRight).normalized * _moveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * maxAirMoveSpeed;
                targetMovementVelocity.y = currentVelocity.y;

                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetMovementVelocity,
                    1 - Mathf.Exp(-airDecelerationSpeed * deltaTime)
                );
            }

            // Aplica gravidade e drag
            currentVelocity += _gravity * deltaTime;
            currentVelocity *= 1f / (1f + drag * deltaTime);

            // Se estiver deslizando na parede, limita a velocidade de descida
            if (_isWallSliding && currentVelocity.y < maxWallSlideSpeed)
            {
                currentVelocity.y = maxWallSlideSpeed;
            }
        }

        // Tratamento do pulo
        _jumpedThisFrame = false;
        _timeSinceJumpRequested += deltaTime;
        if (_jumpRequested && CanJump)
        {
            bool evaluateGroundStatus = allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround;

            if (_currentJumpCount == 0 && (evaluateGroundStatus || _canCoyoteJump))
            {
                if (_canCoyoteJump && !evaluateGroundStatus)
                    _coyoteJumped = true;

                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus is { FoundAnyGround: true, IsStableOnGround: false })
                    jumpDirection = Motor.GroundingStatus.GroundNormal;

                Motor.ForceUnground();

                currentVelocity += jumpDirection * _jumpForce - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _currentJumpCount++;
                _jumpedThisFrame = true;
            }

            bool hasExtraJumps = _currentJumpCount > 0 && _currentJumpCount < maxJumpCount;
            if (!_coyoteJumped && hasExtraJumps && !evaluateGroundStatus)
            {
                Motor.ForceUnground();

                currentVelocity += Motor.CharacterUp * _jumpForce - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _currentJumpCount++;
                _jumpedThisFrame = true;
            }
        }

        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }

        _velocity = Motor.Velocity;
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        if (_coyoteJumped && _currentJumpCount > 0)
            _coyoteJumped = false;

        if (!_canCoyoteJump && _currentJumpCount == 0)
            _currentJumpCount++;

        if (_jumpRequested && _timeSinceJumpRequested > jumpPreGroundingGraceTime)
            _jumpRequested = false;

        if (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
        {
            if (!_jumpedThisFrame)
                _currentJumpCount = 0;

            _timeSinceLastAbleToJump = 0f;
        }
        else
            _timeSinceLastAbleToJump += deltaTime;
    }


    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    // M�todo chamado quando ocorre uma colis�o durante o movimento
    public void OnMovementHit(
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport
    )
    {
        // Se n�o est� no ch�o e o normal tem componente horizontal suficiente, ativa o wall slide.
        // Ajustamos o limiar para 0.3 para detectar paredes mesmo com �ngulos menos extremos.
        if (!Motor.GroundingStatus.IsStableOnGround && Mathf.Abs(hitNormal.x) > 0.3f)
        {
            _isWallSliding = true;
            Debug.Log("Wall Slide ativado: hitNormal = " + hitNormal);
        }
    }

    public void OnGroundHit(
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport
    )
    {
        // Pode ser utilizado para outras l�gicas ao tocar o ch�o.
    }

    public void ProcessHitStabilityReport(
        Collider hitCollider,
        Vector3 hitNormal,
        Vector3 hitPoint,
        Vector3 atCharacterPosition,
        Quaternion atCharacterRotation,
        ref HitStabilityReport hitStabilityReport
    )
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            HandleLanded();
        else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            HandleLeftStableGround();
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }

    public void AddVelocity(Vector3 velocity)
    {
        _internalVelocityAdd += velocity;
    }

    void HandleLanded()
    {
        // L�gica ao aterrissar, se necess�rio.
    }

    void HandleLeftStableGround()
    {
        // L�gica ao deixar o solo, se necess�rio.
    }
}
