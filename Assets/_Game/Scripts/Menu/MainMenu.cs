using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;

    public void Play ()
    {
        ScreenTransitionManager.Instance.ChangeScene("TestPedro");
    }

    public void GoToMenuPanel ()
    {
        mainMenuPanel.SetActive(true);
        upgradePanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    
    public void GoToUpgradePanel ()
    {
        mainMenuPanel.SetActive(false);
        upgradePanel.SetActive(true);
    }
    
    public void GoToOptionsPanel ()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void GoToCreditsPanel ()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void Exit ()
    {
        Application.Quit();
    }
}
