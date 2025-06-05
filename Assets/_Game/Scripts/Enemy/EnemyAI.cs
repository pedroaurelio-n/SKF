using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações de Pulo")]
    public float jumpThreshold = 1.5f;
    public float jumpCooldown = 1f;

    [Header("Referências")]
    public Transform weaponTransform;   // Transform da arma (sprite)
    public Transform weaponHolder;      // Posição base da arma (ex: mão do inimigo)
    public Transform firePoint;         // Ponto onde a bullet vai sair
    public GameObject bulletPrefab;     // Prefab da bala que o inimigo atira

    [Header("Configurações de Tiro")]
    public float fireRate = 1f;         // Quantas balas por segundo
    public float attackRange = 10f;     // Distância máxima para começar a atirar

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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PlayerCharacterController>();
        player = Player.Instance?.transform;

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

        if (agent.isOnNavMesh)
            agent.SetDestination(player.position);

        PlayerCharacterInputs aiInputs = new();

        Vector3 localDir = transform.InverseTransformDirection(agent.desiredVelocity);
        aiInputs.MoveRightAxis = localDir.x;

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
        Vector2 dirAdjusted = dir;
        if (facingLeft)
        {
            dirAdjusted.x = -dirAdjusted.x;
        }

        float angle = Mathf.Atan2(dirAdjusted.y, dirAdjusted.x) * Mathf.Rad2Deg;

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
            return; // player longe demais, não atira

        if (Time.time - lastFireTime < 1f / fireRate)
            return; // ainda não passou o tempo do próximo tiro

        lastFireTime = Time.time;

        // Instancia a bala no firePoint
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Se sua bullet tiver um script de controle, pode configurar direção, força, etc aqui.
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(firePoint.right); // ou forward dependendo do seu eixo da bullet
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }
}
