using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] UnityEvent onDeath;
    [SerializeField] UnityEvent onDamage;
    [SerializeField] UnityEvent<float> onInvincibilityStart;
    [SerializeField] UnityEvent onInvincibilityEnd;
    
    [SerializeField] float maxHealth;
    [SerializeField] bool hasInvincibility;
    [SerializeField] float invincibilityTime;

    float _currentHealth;
    bool _hasDied;
    
    bool _isInvincible;
    WaitForSeconds _waitForInvincibility;
    Coroutine _invincibilityRoutine;

    void Start ()
    {
        Reset();
        _waitForInvincibility = new WaitForSeconds(invincibilityTime);
    }

    public void Reset ()
    {
        _hasDied = false;
        _currentHealth = maxHealth;
    }

    public void ModifyHealth (int difference)
    {
        if (_isInvincible && difference < 0)
            return;
        
        _currentHealth += difference;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _hasDied = true;
            onDeath?.Invoke();
            return;
        }

        if (difference < 0)
        {
            onDamage?.Invoke();
            
            if (hasInvincibility)
                _invincibilityRoutine = StartCoroutine(InvincibilityRoutine());
        }
    }

    IEnumerator InvincibilityRoutine ()
    {
        onInvincibilityStart?.Invoke(invincibilityTime);
        _isInvincible = true;
        yield return _waitForInvincibility;
        
        _isInvincible = false;
        onInvincibilityEnd?.Invoke();
        _invincibilityRoutine = null;
    }
}
