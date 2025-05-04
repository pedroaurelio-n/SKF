using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTrigger) return;

        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                Vector3 hitDirection = (other.transform.position - transform.position).normalized;
                playerHealth.ModifyHealth(-damage, hitDirection);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTrigger) return;

        if (collision.collider.CompareTag("Player"))
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                Vector3 hitDirection = (collision.transform.position - transform.position).normalized;
                playerHealth.ModifyHealth(-damage, hitDirection);
            }
        }
    }
}
