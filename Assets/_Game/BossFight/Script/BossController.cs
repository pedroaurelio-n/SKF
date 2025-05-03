// BossController.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour
{
    [Header("Vida do Boss")]
    public int maxHealth = 1000;
    private int currentHealth;

    [Header("Refer�ncia para HUD")]
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
        // Aqui voc� pode adicionar anima��o de morte ou efeitos
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
                // Aqui voc� chamaria o ataque da m�o i (esquerda ou direita alternando por exemplo)
                Debug.Log($"Ataque da m�o {i + 1}");
                // Simula o tempo de cada ataque
                yield return new WaitForSeconds(1.5f);
            }

            // Ap�s os 5 ataques, dispara laser
            Debug.Log("Disparo de laser!");
            // Chame aqui a fun��o de laser se necess�rio
            yield return new WaitForSeconds(2f);
        }
    }
}
