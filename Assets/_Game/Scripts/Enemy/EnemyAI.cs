using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float jumpThreshold = 1.5f; // How high before it jumps
    public float jumpCooldown = 1f;
    
    NavMeshAgent _agent;
    PlayerCharacterController _characterController;

    float _lastJumpTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<PlayerCharacterController>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (Player.Instance == null)
            return;
        
        _agent.SetDestination(Player.Instance.transform.position);
        
        PlayerCharacterInputs aiInputs = new();

        Vector3 localDir = transform.InverseTransformDirection(_agent.desiredVelocity);
        aiInputs.MoveRightAxis = localDir.x;

        float yDiff = Player.Instance.transform.position.y - transform.position.y;
        if (_agent.isOnOffMeshLink && yDiff > jumpThreshold && Time.time - _lastJumpTime > jumpCooldown)
        {
            aiInputs.JumpPressed = true;
            _lastJumpTime = Time.time;
        }

        _characterController.SetInputs(ref aiInputs);
    }
    
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }
}
