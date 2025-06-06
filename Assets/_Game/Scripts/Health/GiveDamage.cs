using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GiveDamage : MonoBehaviour
{
    [Tooltip("Em quais layers o dano deve ser aplicado")]
    [SerializeField] private LayerMask damageableLayers;
    [Tooltip("Quantidade de vida a diminuir")]
    [SerializeField] private int damage = 1;
    [Tooltip("Usar trigger ao invés de colisão física")]
    [SerializeField] private bool useTrigger = true;
    [SerializeField] bool destroyOnDamage;

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
        // Só aplica em objetos que estejam na Layer correta
        if ((damageableLayers.value & (1 << other.gameObject.layer)) == 0)
            return;

        // Só aplica se tiver um Health
        if (!other.TryGetComponent<Health>(out var health))
            return;

        // Direção de knockback: do ponto de colisão para o centro do player/enemy
        Vector3 hitDirection = (other.transform.position - transform.position).normalized;
        Debug.Log($"[GiveDamage] Aplicando {damage} de dano em {other.name}");

        health.ModifyHealth(-damage, hitDirection);
        
        if (destroyOnDamage)
            Destroy(gameObject);
    }
}
