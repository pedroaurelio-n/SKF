using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float distanceMin = 0.3f; // Distância mínima da arma ao jogador
    [SerializeField] private float distanceMax = 1f; // Distância máxima da arma ao jogador
    [SerializeField] private float smoothMove = 10f; // Controla a suavidade da transição
    [SerializeField] private SpriteRenderer srGun; // Sprite da arma
    [SerializeField] private Transform firePoint; // Ponto de disparo

    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;

    [SerializeField] private float limiteInferiorFisico = -0.2f; // Posição mínima da arma relativa ao player

    // Controle de estado do jogo
    private bool gameStarted = false;

    // Texturas para o cursor
    [SerializeField] private Texture2D defaultCursor; // Cursor padrão (antes do jogo iniciar ou ao pausar)
    [SerializeField] private Texture2D gameCursor; // Cursor que aparece quando o jogo está ativo
    // Ponto ativo do cursor (hotspot)
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    void Start()
    {
        // Define o cursor padrão no início
        // Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        // Converte a posição do mouse para coordenadas do mundo (útil para jogos 2D de plataforma)
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Inicia o jogo ao clicar com o mouse e troca para o cursor do jogo
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
            // Cursor.SetCursor(gameCursor, cursorHotspot, CursorMode.Auto);
        }

        // Se o jogo estiver ativo e o jogador pressionar ESC, volta para o cursor padrão
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            gameStarted = false;
            // Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
    }

    private void FixedUpdate()
    {
        // Calcula a direção da arma em relação ao jogador
        dirArma = (mousePosi - (Vector2)player.position).normalized;

        // Calcula a posição alvo da arma respeitando os limites de distância
        float distanciaAtual = Mathf.Clamp(Vector2.Distance(player.position, mousePosi), distanceMin, distanceMax);
        Vector2 targetPosition = (Vector2)player.position + dirArma * distanciaAtual;

        // Impede que a arma ultrapasse fisicamente a parte inferior do jogador
        if (targetPosition.y < player.position.y + limiteInferiorFisico)
        {
            targetPosition.y = player.position.y + limiteInferiorFisico;
        }

        // Move a arma suavemente para a posição desejada
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * smoothMove);

        // Calcula o ângulo da arma
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;

        // Rotaciona a arma de forma suave e precisa
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smoothMove * 100f);

        // Ajusta o espelhamento da arma conforme a posição do mouse
        bool mirandoParaEsquerda = mousePosi.x < player.position.x;
        Vector3 scale = transform.localScale;
        scale.y = mirandoParaEsquerda ? -1 : 1;
        transform.localScale = scale;
    }
}
