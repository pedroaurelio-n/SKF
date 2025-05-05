using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [field: SerializeField] public PlayerCharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    
    [SerializeField] Health health;
    [SerializeField] GameObject mainSprite;
    
    SpriteRenderer[] sprites;

    [SerializeField] float knockBackForce;
    bool _isIntangible;

    private void Awake()
    {
        Instance = this;
        sprites = mainSprite.GetComponentsInChildren<SpriteRenderer>();
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
    }

    void Die ()
    {
        EventManager.TriggerPlayerDeath();
        CharacterController.enabled = false;
        Animator.SetTrigger("Died");
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
        Color originalColor = Color.white;
        float flashSpeed = 0.1f;
        WaitForSeconds waitForFlash = new(flashSpeed);
        
        while (timer < duration)
        {
            foreach (SpriteRenderer sprite in sprites)
                sprite.color = Color.yellow;
            yield return waitForFlash;

            foreach (SpriteRenderer sprite in sprites)
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

        foreach (SpriteRenderer sprite in sprites)
            sprite.color = originalColor;
    }

    void IgnoreEnemyLayers (bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
        CharacterController.Motor.BuildCollidableLayers();
        // Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"), ignore);
    }
}