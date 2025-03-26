using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] UnityEvent onDeath;
    
    [SerializeField] float maxHealth;

    float _currentHealth;
    bool _hasDied;

    void Start ()
    {
        Reset();
    }

    public void Reset ()
    {
        _hasDied = false;
        _currentHealth = maxHealth;
    }

    public void ModifyHealth (int difference)
    {
        _currentHealth += difference;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _hasDied = true;
            onDeath?.Invoke();
        }
    }
}
