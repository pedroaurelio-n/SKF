// BossController.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour
{
    [Header("Vida do Boss")]
    public int maxHealth = 1000;
    private int currentHealth;

    [Header("Referência para HUD")]
    public BossHealth bossHealthBar;

    [Header("Eventos")]
    public UnityEvent onBossDeath;

    private bool isAlive = true;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);
        StartCoroutine(BossRoutine());
    }

    public void TakeDamage(int amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        bossHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        StopAllCoroutines();
        onBossDeath?.Invoke();
        // Aqui você pode adicionar animação de morte ou efeitos
        Debug.Log("Boss morreu!");
    }

    private IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(2f); // Espera inicial

        while (isAlive)
        {
            isAttacking = true;

            for (int i = 0; i < 5; i++)
            {
                // Aqui você chamaria o ataque da mão i (esquerda ou direita alternando por exemplo)
                Debug.Log($"Ataque da mão {i + 1}");
                // Simula o tempo de cada ataque
                yield return new WaitForSeconds(1.5f);
            }

            // Após os 5 ataques, dispara laser
            Debug.Log("Disparo de laser!");
            // Chame aqui a função de laser se necessário
            yield return new WaitForSeconds(2f);
        }
    }
}
