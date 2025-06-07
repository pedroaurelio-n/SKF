// BossHUD.cs
using UnityEngine;
using UnityEngine.UI;

public class BossHUD : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel; // o painel do boss

    void Awake() => rootPanel.SetActive(false);

    public void Show() => rootPanel.SetActive(true);
    public void Hide() => rootPanel.SetActive(false);

    // Se preferir eventos:
    void OnEnable()
    {
        EventManager.OnBossFightStarted += Show;
        EventManager.OnBossDefeated += Hide;
    }
    void OnDisable()
    {
        EventManager.OnBossFightStarted -= Show;
        EventManager.OnBossDefeated -= Hide;
    }
}

