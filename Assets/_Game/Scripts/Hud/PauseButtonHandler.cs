using UnityEngine;

public class PauseButtonHandler : MonoBehaviour
{
    public void PauseButtonClicked()
    {
        FindObjectOfType<PauseManager>().PauseGame();
    }
}
