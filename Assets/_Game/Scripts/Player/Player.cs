using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] PlayerCharacterController characterController;
    [SerializeField] Health health;
    [SerializeField] SpriteRenderer sprite;

    [SerializeField] float knockBackForce;
    bool _isIntangible;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable ()
    {
        health.OnDamage += PlayerDamaged;
        health.OnDeath += Die;
        health.OnInvincibilityStart += StartInvincibilityFlash;
    }

    void OnDisable ()
    {
        health.OnDamage -= PlayerDamaged;
        health.OnDeath -= Die;
        health.OnInvincibilityStart -= StartInvincibilityFlash;
    }

    void PlayerDamaged (int current, int max, Vector3 direction)
    {
        EventManager.TriggerPlayerHealthChanged(current, max);
        characterController.AddVelocity(direction * knockBackForce);
    }

    void Die ()
    {
        EventManager.TriggerPlayerDeath();
        gameObject.SetActive(false);
    }

    void StartInvincibilityFlash (float duration)
    {
        if (_isIntangible)
            return;
        
        _isIntangible = true;
        IgnoreEnemyLayers(_isIntangible);
        StartCoroutine(FlashRoutine(duration));
    }

    IEnumerator FlashRoutine (float duration)
    {
        float timer = 0f;
        Color originalColor = sprite.color;
        float flashSpeed = 0.1f;
        WaitForSeconds waitForFlash = new(flashSpeed);
        
        while (timer < duration)
        {
            sprite.color = Color.white;
            yield return waitForFlash;

            sprite.color = originalColor;
            yield return waitForFlash;
            
            timer += flashSpeed * 2;

            if (_isIntangible && timer >= duration * 0.8f)
            {
                _isIntangible = false;
                IgnoreEnemyLayers(_isIntangible);
            }
            yield return null;
        }

        sprite.color = originalColor;
    }

    void IgnoreEnemyLayers (bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
        characterController.Motor.BuildCollidableLayers();
        // Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"), ignore);
    }
}