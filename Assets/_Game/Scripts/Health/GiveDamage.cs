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
        
        health.ModifyHealth(-damage);
    }

    void OnCollisionEnter (Collision other)
    {
        if (!IsInDamageableLayer(other.gameObject))
            return;
        
        if (!other.gameObject.TryGetComponent<Health>(out Health health))
            return;
        
        health.ModifyHealth(-damage);
    }
    
    bool IsInDamageableLayer(GameObject obj)
    {
        return (damageableLayers.value & (1 << obj.layer)) != 0;
    }
}