using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] Health health;
    [SerializeField] SpriteRenderer sprite;

    EnemyAI _enemyAI;
    bool _isFlashing;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
        Invoke(nameof(ActivateAI), 0.7f);
    }

    void ActivateAI()
    {
        _enemyAI.enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
    }

    void OnEnable ()
    {
        health.OnDamage += StartDamageFlash;
        health.OnDeath += Die;
    }

    void OnDisable ()
    {
        health.OnDamage -= StartDamageFlash;
        health.OnDeath -= Die;
    }

    public void ResetEnemy ()
    {
        health.Reset();
    }
    
    void Die ()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
    
    void StartDamageFlash (int current, int max, Vector3 direction)
    {
        if (_isFlashing)
            return;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine ()
    {
        _isFlashing = true;
        Color originalColor = sprite.color;
        float flashSpeed = 0.1f;
        WaitForSeconds waitForFlash = new(flashSpeed);

        sprite.color = Color.white;
        yield return waitForFlash;

        sprite.color = originalColor;
        _isFlashing = false;
    }
}
