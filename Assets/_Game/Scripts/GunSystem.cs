using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private Transform player; // Refer�ncia ao jogador
    [SerializeField] private float distanceMin = 0.3f; // Dist�ncia m�nima da arma ao jogador
    [SerializeField] private float distanceMax = 1f; // Dist�ncia m�xima da arma ao jogador
    [SerializeField] private float smoothMove = 10f; // Controla a suavidade da transi��o
    [SerializeField] private SpriteRenderer srGun; // Sprite da arma
    [SerializeField] private Transform firePoint; // Ponto de disparo

    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;

    [SerializeField] private float limiteInferiorFisico = -0.2f; // Posi��o m�nima da arma relativa ao player

    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // Calcula dire��o da arma em rela��o ao player
        dirArma = (mousePosi - (Vector2)player.position).normalized;

        // Calcula a posi��o alvo da arma respeitando os limites de dist�ncia
        float distanciaAtual = Mathf.Clamp(Vector2.Distance(player.position, mousePosi), distanceMin, distanceMax);
        Vector2 targetPosition = (Vector2)player.position + dirArma * distanciaAtual;

        // Impede que a arma ultrapasse fisicamente a parte inferior do jogador
        if (targetPosition.y < player.position.y + limiteInferiorFisico)
        {
            targetPosition.y = player.position.y + limiteInferiorFisico;
        }

        // Move a arma suavemente para a posi��o desejada
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * smoothMove);

        // Calcula o �ngulo da arma
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;

        // Rotaciona a arma de forma mais precisa e suave
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smoothMove * 100f);

        // Ajusta espelhamento corretamente
        bool mirandoParaEsquerda = mousePosi.x < player.position.x;
        Vector3 scale = transform.localScale;
        scale.y = mirandoParaEsquerda ? -1 : 1;
        transform.localScale = scale;
    }
}
