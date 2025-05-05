// Health.cs
using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int, int> OnStart;
    public event Action<int, int, Vector3> OnDamage;
    public event Action OnDeath;
    public event Action<float> OnInvincibilityStart;
    public event Action OnInvincibilityEnd;

    [SerializeField] int maxHealth;
    [SerializeField] bool hasInvincibility;
    [SerializeField] float invincibilityTime;

    int _currentHealth;
    bool _hasDied;

    bool _isInvincible;
    WaitForSeconds _waitForInvincibility;
    Coroutine _invincibilityRoutine;

    void Start()
    {
        Reset();
        Debug.Log($"[Health] Start: vida inicial = {_currentHealth}/{maxHealth}");
        OnStart?.Invoke(_currentHealth, maxHealth);
        _waitForInvincibility = new WaitForSeconds(invincibilityTime);
    }

    public void Reset()
    {
        _hasDied = false;
        _currentHealth = maxHealth;
    }

    public void ModifyHealth(int difference, Vector3 direction)
    {
        if (_hasDied) return;
        if (_isInvincible && difference < 0) return;

        _currentHealth += difference;
        Debug.Log($"[Health] ModifyHealth: diff={difference}, nova vida = {_currentHealth}/{maxHealth}");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _hasDied = true;
            Debug.Log("[Health] OnDeath");
            OnDeath?.Invoke();
            return;
        }

        if (difference < 0)
        {
            OnDamage?.Invoke(_currentHealth, maxHealth, direction);
            if (hasInvincibility)
                _invincibilityRoutine = StartCoroutine(InvincibilityRoutine());
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        OnInvincibilityStart?.Invoke(invincibilityTime);
        _isInvincible = true;
        yield return _waitForInvincibility;
        _isInvincible = false;
        OnInvincibilityEnd?.Invoke();
        _invincibilityRoutine = null;
    }
}
