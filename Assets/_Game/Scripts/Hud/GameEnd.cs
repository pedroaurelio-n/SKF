using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameObject gameOverContainer;
    [SerializeField] GameObject gameWinContainer;

    void OnEnable ()
    {
        EventManager.OnPlayerDeath += TriggerGameOver;
        EventManager.OnAllWavesDefeated += TriggerGameWin;
    }

    void OnDisable ()
    {
        EventManager.OnPlayerDeath -= TriggerGameOver;
        EventManager.OnAllWavesDefeated -= TriggerGameWin;
    }

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
        ScreenTransitionManager.Instance.ChangeScene("GameplayScene");
    }

    public void GoToMainMenu ()
    {
        ScreenTransitionManager.Instance.ChangeScene("MainMenu");
    }
}
