using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [field: SerializeField] public PlayerCharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [field: SerializeField] public Transform Center { get; private set; }
    
    [SerializeField] Health health;
    [SerializeField] GameObject mainSprite;
    [SerializeField] Transform gun;
    
    SpriteRenderer[] sprites;

    [SerializeField] float knockBackForce;
    bool _isIntangible;
    Vector3 _initialScale;

    private void Awake()
    {
        Instance = this;
        sprites = mainSprite.GetComponentsInChildren<SpriteRenderer>();
        _initialScale = Animator.transform.localScale;
    }

    private void Update()
    {
        if (CharacterController.Motor.Velocity != Vector3.zero)
        {
            float aimDirection = transform.position.x - gun.position.x;
            float movementDirection = CharacterController.Motor.Velocity.x;
            
            Animator.SetBool("IsBackwards",
                Mathf.Approximately(Mathf.Sign(aimDirection), Mathf.Sign(movementDirection)));
        }
        
        if (transform.position.x < gun.position.x)
        {
            Animator.transform.localScale = new Vector3(_initialScale.x, _initialScale.y, _initialScale.z);
        }
        else if (transform.position.x > gun.position.x)
        {
            Animator.transform.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
        }
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
                sprite.color = Color.black;
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