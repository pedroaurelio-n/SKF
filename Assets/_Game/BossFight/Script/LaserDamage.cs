using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqui voc� pode chamar um m�todo de dano no player
            Debug.Log("Player atingido pelo laser!");
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
