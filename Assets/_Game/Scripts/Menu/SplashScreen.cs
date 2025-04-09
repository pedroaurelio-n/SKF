using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public void GoToMainMenu ()
    {
        ScreenTransitionManager.Instance.ChangeScene("MainMenu");
    }
}
