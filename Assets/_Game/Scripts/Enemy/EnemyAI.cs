using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Distância de Comportamento")]
    public float stopDistance = 2f;     // Enemy stops moving toward player at this range
    public float retreatDistance = 1f;  // Enemy moves away from player if closer than this

    [Header("Configura��es de Pulo")]
    public float jumpThreshold = 1.5f;
    public float jumpCooldown = 1f;

    [Header("Refer�ncias")]
    public Transform weaponTransform;   // Transform da arma (sprite)
    public Transform weaponHolder;      // Posi��o base da arma (ex: m�o do inimigo)
    public Transform firePoint;         // Ponto onde a bullet vai sair
    public GameObject bulletPrefab;     // Prefab da bala que o inimigo atira

    [Header("Configura��es de Tiro")]
    public float fireRate = 1f;         // Quantas balas por segundo
    public float attackRange = 10f;     // Dist�ncia m�xima para come�ar a atirar

    public float distanceMin = 0.3f;
    public float distanceMax = 1f;
    public float baseSmoothMove = 10f;
    public float limiteInferiorFisico = -0.2f;

    private NavMeshAgent agent;
    private Transform player;
    private PlayerCharacterController characterController;

    private float lastJumpTime;
    private float lastFireTime;
    private Vector3 weaponOriginalScale;
    private SpriteRenderer srEnemy;
    Vector2 localDir;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PlayerCharacterController>();
        player = Player.Instance?.Center;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        srEnemy = GetComponent<SpriteRenderer>();

        if (weaponTransform != null)
            weaponOriginalScale = weaponTransform.localScale;
    }

    private void Update()
    {
        if (player == null || agent == null || characterController == null)
            return;

        PlayerCharacterInputs aiInputs = new();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // === Movement Behavior Based on Distance === //
        if (agent.isOnNavMesh)
        {
            if (distanceToPlayer > stopDistance)
            {
                // Follow the player
                agent.SetDestination(player.position);
                localDir = transform.InverseTransformDirection(agent.desiredVelocity);
                aiInputs.MoveRightAxis = localDir.x;
            }
            else if (distanceToPlayer < retreatDistance)
            {
                // Move away from player
                Vector3 dirAway = (transform.position - player.position).normalized;
                Vector3 moveTarget = transform.position + dirAway * 2f;

                agent.SetDestination(moveTarget);
                localDir = transform.InverseTransformDirection(agent.desiredVelocity);
                aiInputs.MoveRightAxis = localDir.x;
            }
            else
            {
                // In idle buffer zone, stop moving
                agent.ResetPath();
                localDir = Vector2.zero;
                aiInputs.MoveRightAxis = 0f;
            }
        }

        // === Jump Logic === //
        float yDiff = player.position.y - transform.position.y;
        if (agent.isOnOffMeshLink && yDiff > jumpThreshold && Time.time - lastJumpTime > jumpCooldown)
        {
            aiInputs.JumpPressed = true;
            lastJumpTime = Time.time;
        }

        characterController.SetInputs(ref aiInputs);

        UpdateWeaponMovementAndRotation();
        TryFire();
    }


    private void UpdateWeaponMovementAndRotation()
    {
        if (weaponTransform == null || player == null || weaponHolder == null)
            return;

        Vector2 dir = ((Vector2)player.position - (Vector2)weaponHolder.position).normalized;

        float dist = Mathf.Clamp(Vector2.Distance(weaponHolder.position, player.position), distanceMin, distanceMax);

        Vector2 targetPos = (Vector2)weaponHolder.position + dir * dist;

        if (targetPos.y < weaponHolder.position.y + limiteInferiorFisico)
            targetPos.y = weaponHolder.position.y + limiteInferiorFisico;

        float smooth = baseSmoothMove;
        weaponTransform.position = Vector2.Lerp(weaponTransform.position, targetPos, Time.deltaTime * smooth);

        bool facingLeft = player.position.x < transform.position.x;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        weaponTransform.rotation = Quaternion.RotateTowards(weaponTransform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smooth * 100f);

        if (angle > 90f || angle < -90f)
            weaponTransform.localScale = new Vector3(weaponOriginalScale.x, -Mathf.Abs(weaponOriginalScale.y), weaponOriginalScale.z);
        else
            weaponTransform.localScale = new Vector3(weaponOriginalScale.x, Mathf.Abs(weaponOriginalScale.y), weaponOriginalScale.z);

        if (srEnemy != null)
            srEnemy.flipX = facingLeft;
    }

    private void TryFire()
    {
        if (firePoint == null || bulletPrefab == null || player == null)
            return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer > attackRange)
            return; // player longe demais, n�o atira

        if (Time.time - lastFireTime < 1f / fireRate)
            return; // ainda n�o passou o tempo do pr�ximo tiro

        lastFireTime = Time.time;

        // Instancia a bala no firePoint
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Se sua bullet tiver um script de controle, pode configurar dire��o, for�a, etc aqui.
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Setup(firePoint.right); // ou forward dependendo do seu eixo da bullet
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyDecisionZone zone))
        {
            if (player == null || characterController == null)
                return;

            if (localDir.x > 0 && zone.FromRight)
                return;
            if (localDir.x < 0 && !zone.FromRight)
                return;

            // Temporarily stop the agent
            if (agent != null && agent.enabled)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.enabled = false;
            }

            PlayerCharacterInputs aiInputs = new();

            // Decide to jump or fall based on vertical position of player
            if (zone.Jump)
                aiInputs.JumpPressed = true;
            
            aiInputs.MoveRightAxis = Mathf.Sign(player.position.x - transform.position.x);

            // Send the inputs for 1 frame (or create a coroutine for smoother control)
            characterController.SetInputs(ref aiInputs);

            // Optionally re-enable the NavMeshAgent after a delay
            StartCoroutine(ReenableAgentAfterDelay(1f, aiInputs));
        }
    }
    
    IEnumerator ReenableAgentAfterDelay(float delay, PlayerCharacterInputs aiInputs)
    {
        aiInputs.JumpPressed = false;
        float timer = 0f;
        while (timer < delay)
        {
            characterController.SetInputs(ref aiInputs);
            timer += Time.deltaTime;
            yield return null;
        }

        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        yield return null;
    }
}
