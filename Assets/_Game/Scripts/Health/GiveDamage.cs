using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    [SerializeField] LayerMask damageableLayers;
    [SerializeField] int damage;

    void OnTriggerEnter (Collider other)
    {
        if (!IsInDamageableLayer(other.gameObject))
            return;
        
        if (!other.TryGetComponent<Health>(out Health health))
            return;
        
        ModifyHealth(health);
    }

    void OnCollisionEnter (Collision other)
    {
        if (!IsInDamageableLayer(other.gameObject))
            return;
        
        if (!other.gameObject.TryGetComponent<Health>(out Health health))
            return;
        
        ModifyHealth(health);
    }

    void ModifyHealth (Health health)
    {
        Vector3 direction = health.gameObject.transform.position - transform.position;
        health.ModifyHealth(-damage, direction.normalized);
    }
    
    bool IsInDamageableLayer(GameObject obj)
    {
        return (damageableLayers.value & (1 << obj.layer)) != 0;
    }
}