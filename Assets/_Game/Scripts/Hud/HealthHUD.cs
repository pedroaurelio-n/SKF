using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthCircle;

    private int maxHealth;

    void OnEnable()
    {
        playerHealth.OnStart += HandleHealthStart;
        playerHealth.OnDamage += HandleHealthChange;
        playerHealth.OnIncrease += HandleHealthChange;
        playerHealth.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        playerHealth.OnStart -= HandleHealthStart;
        playerHealth.OnDamage -= HandleHealthChange;
        playerHealth.OnIncrease -= HandleHealthChange;
        playerHealth.OnDeath -= HandleDeath;
    }

    private void HandleHealthStart(int current, int max)
    {
        maxHealth = max;
        UpdateHealthUI(current);
    }

    private void HandleHealthChange(int current, int max, Vector3 dir)
    {
        UpdateHealthUI(current);
        // Opcional: animar ou piscar ao sofrer dano
        // StartCoroutine(FlashDamage());
    }

    private void HandleDeath()
    {
        UpdateHealthUI(0);
        // Opcional: mostrar tela de morte
    }

    private void UpdateHealthUI(int currentHealth)
    {
        float fill = (float)currentHealth / maxHealth;
        healthCircle.fillAmount = fill;
    }
}