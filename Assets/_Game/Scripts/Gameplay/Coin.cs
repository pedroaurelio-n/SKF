using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinCollector.Instance?.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
