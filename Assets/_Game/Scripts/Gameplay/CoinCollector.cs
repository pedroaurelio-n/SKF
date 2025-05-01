using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        coinText.text = $"Coins: {coinCount}";
    }
}
