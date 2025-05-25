using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    [SerializeField] private GameObject healEffect; // Partícula que aparece no player

    private void OnTriggerEnter(Collider other)
    {
        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.ModifyHealth(healAmount, Vector3.zero);

            if (healEffect != null)
            {
                // Instancia a partícula diretamente na posição do player
                Instantiate(healEffect, playerHealth.transform.position, Quaternion.identity, playerHealth.transform);
            }

            Destroy(gameObject);
        }
    }
}
