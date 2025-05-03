using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip collectSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var coinCollector = FindObjectOfType<CoinCollector>();
            if (coinCollector != null)
            {
                coinCollector.AddCoins(value);
                if (collectSound != null)
                    AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            else
            {
                Debug.LogWarning("Nenhum CoinCollector encontrado na cena!");
            }

            Destroy(gameObject);
        }
    }
}
