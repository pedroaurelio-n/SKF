using System;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    [SerializeField] LayerMask damageableLayers;
    [SerializeField] int damage;

    void OnTriggerEnter (Collider other)
    {
        if (!other.TryGetComponent<Health>(out Health health))
            return;
        health.ModifyHealth(-damage);
    }
}