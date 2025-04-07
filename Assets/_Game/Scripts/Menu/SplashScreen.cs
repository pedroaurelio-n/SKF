using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
