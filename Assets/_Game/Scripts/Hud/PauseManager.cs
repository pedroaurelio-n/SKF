using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenuUI; // Menu de pause
    public GameObject optionsUI;   // Menu de op��es (dentro do pause)

    private bool isPaused = false;

    void Update()
    {
        // ESC alterna pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsUI.activeSelf)
            {
                // Se est� nas op��es, volta pro menu de pause
                optionsUI.SetActive(false);
                pauseMenuUI.SetActive(true);
                return;
            }

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Pausa o jogo
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Retoma o jogo
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Vai pro menu principal
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Ajuste o nome da sua cena de menu aqui
    }

    // Abre o menu de op��es
    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    // Fun��o p�blica para o bot�o de pause
    public void PauseButtonClicked()
    {
        PauseGame();
    }
}
