using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] Health health;
    [SerializeField] SpriteRenderer sprite;

    bool _isFlashing;

    public void Reset ()
    {
        health.Reset();
    }
    
    public void Die ()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
    
    public void StartDamageFlash ()
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
