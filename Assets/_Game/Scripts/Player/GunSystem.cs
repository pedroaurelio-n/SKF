using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceMin = 0.3f;
    [SerializeField] private float distanceMax = 1f;
    [SerializeField] private float baseSmoothMove = 10f;
    [SerializeField] private SpriteRenderer srGun;
    [SerializeField] private Transform firePoint;

    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;

    [SerializeField] private float limiteInferiorFisico = -0.2f;

    private bool gameStarted = false;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D gameCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    private float sensitivity = 1f; // Sensibilidade do movimento da mira

    void Start()
    {
        // Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f); // Valor entre 0.1 e 2 por exemplo
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            // Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
            return;
        }

        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
            // Cursor.SetCursor(gameCursor, cursorHotspot, CursorMode.Auto);
        }

        if (gameStarted && Input.GetMouseButtonDown(0))
        {
            // Cursor.SetCursor(gameCursor, cursorHotspot, CursorMode.Auto);
        }

        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            gameStarted = false;
            // Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        dirArma = (mousePosi - (Vector2)player.position).normalized;

        float distanciaAtual = Mathf.Clamp(Vector2.Distance(player.position, mousePosi), distanceMin, distanceMax);
        Vector2 targetPosition = (Vector2)player.position + dirArma * distanciaAtual;

        if (targetPosition.y < player.position.y + limiteInferiorFisico)
        {
            targetPosition.y = player.position.y + limiteInferiorFisico;
        }

        // A sensibilidade agora afeta a velocidade com que a arma segue o mouse
        float smooth = baseSmoothMove * sensitivity;
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smooth * 100f);

        bool mirandoParaEsquerda = mousePosi.x < player.position.x;
        Vector3 scale = transform.localScale;
        scale.y = mirandoParaEsquerda ? -1 : 1;
        transform.localScale = scale;
        // Vira o player conforme a direção da arma
        if (dirArma.x < 0)
        {
            player.localScale = new Vector3(-Mathf.Abs(player.localScale.x), player.localScale.y, player.localScale.z);
        }
        else if (dirArma.x > 0)
        {
            player.localScale = new Vector3(Mathf.Abs(player.localScale.x), player.localScale.y, player.localScale.z);
        }

    }

    public void UpdateSensitivity(float value)
    {
        sensitivity = value;
    }
}
