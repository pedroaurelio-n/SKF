using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageToPlayer : MonoBehaviour
{
    [Tooltip("Quanto de vida tirar do player")]
    [SerializeField] private int damage = 1;

    [Tooltip("Se true usa OnTriggerEnter, caso contrário usa OnCollisionEnter")]
    [SerializeField] private bool useTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        TryDamage(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;
        TryDamage(collision.collider);
    }

    private void TryDamage(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Debug para ver quem estamos tocando
        Debug.Log($"DamageToPlayer: Colidiu com {other.name}, causando {damage} de dano.");

        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            // Direção de knockback: do inimigo para o player
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            playerHealth.ModifyHealth(-damage, hitDirection);
        }
        else
        {
            Debug.LogWarning("DamageToPlayer: objeto com tag Player não possui componente Health!");
        }
    }
}
