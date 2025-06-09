using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameObject gameOverContainer;
    [SerializeField] GameObject gameWinContainer;

    void OnEnable ()
    {
        EventManager.OnPlayerDeath += TriggerGameOver;
    }

    void OnDisable ()
    {
        EventManager.OnPlayerDeath -= TriggerGameOver;
    }

    public void TriggerGameOver ()
    {
        gameOverContainer.SetActive(true);
    }

    public void TriggerGameWin ()
    {
        gameWinContainer.SetActive(true);
        Player.Instance.IsIntangible = true;
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
