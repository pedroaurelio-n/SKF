using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Animator animator;
    [SerializeField] GameEnd gameEnd;

    public event Action<float, float> OnHealthChanged;

    private bool isDead = false;

    void Start()
    {
        // Inicializa a vida
        SetMaxHealth((int)maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        SetHealth((int)currentHealth);
    }

    // Atualiza o valor m�ximo da barra e reseta vida
    public void SetMaxHealth(int max)
    {
        maxHealth = max;
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthSlider.interactable = false;
        }
    }

    // Atualiza valor atual da barra e dispara eventos/morte
    public void SetHealth(int health)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(health, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log("Boss morreu!");
        Invoke(nameof(EndGame), 3f);

        // Toca anima��o de morte
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Death");

        // Desativa demais componentes (IA, controle de movimento, etc.)
        // foreach (var comp in GetComponents<MonoBehaviour>())
        // {
        //     if (comp != this) comp.enabled = false;
        // }

        // Opcional: desativa colisores
        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
    }

    void EndGame()
    {
        gameEnd.transform.parent.gameObject.SetActive(true);
        gameEnd.gameObject.SetActive(true);
        gameEnd.TriggerGameWin();
    }
}
