using UnityEngine;
using TMPro;

public class EnemyKillTracker : MonoBehaviour
{
    public static EnemyKillTracker Instance { get; private set; }

    [SerializeField] private TMP_Text killText;
    private int killCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterKill()
    {
        killCount++;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        killText.text = $"{killCount}";
    }
}
