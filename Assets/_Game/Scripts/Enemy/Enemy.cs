using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;
    
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public PlayerCharacterController CharacterController { get; private set; }

    [SerializeField] Health health;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform gun;

    EnemyAI _enemyAI;
    bool _isFlashing;
    Vector3 _initialScale;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
        Invoke(nameof(ActivateAI), 0.7f);
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

    void Die()
    {
        GetComponent<DropSpawner>()?.TrySpawnDrop(); // <- chama o spawner
        OnDeath?.Invoke();
        EnemyKillTracker.Instance?.RegisterKill();
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
