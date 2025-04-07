using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameObject gameOverContainer;
    [SerializeField] GameObject gameWinContainer;

    public void TriggerGameOver ()
    {
        gameOverContainer.SetActive(true);
    }

    public void TriggerGameWin ()
    {
        gameWinContainer.SetActive(true);
    }

    public void PlayAgain ()
    {
        SceneManager.LoadScene("TestPedro");
    }

    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
