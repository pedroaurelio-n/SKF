using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Vida do Boss")]
    public float maxHealth = 1000f;
    private float currentHealth;

    [Header("Referência da Barra de Vida")]
    [SerializeField] private Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Boss morreu!");
        // Aqui você pode colocar animações, desativar o boss, tocar som, etc.
        Destroy(gameObject);
    }

    internal void SetMaxHealth(int maxHealth)
    {
        throw new NotImplementedException();
    }

    internal void SetHealth(int currentHealth)
    {
        throw new NotImplementedException();
    }
}
