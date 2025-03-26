using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] Health health;

    public void Reset ()
    {
        health.Reset();
    }
    
    public void Die ()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(true);
    }
}
