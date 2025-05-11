using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;

    [SerializeField] private Slider healthSlider;

    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.interactable = false;
        }
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
