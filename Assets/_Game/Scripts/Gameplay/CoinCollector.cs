using UnityEngine;
using TMPro;

// Teste com isso:
public class CoinCollector : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private int coinCount = 0;

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        coinText.text = $"{coinCount}";
    }
}
