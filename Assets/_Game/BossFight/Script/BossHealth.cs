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

    // NOVO: Prefabs para drop aleatório
    [Header("Item Drop Settings")]
    [SerializeField] private GameObject[] dropPrefabs; // lista de itens que podem ser dropados
    [SerializeField] private Transform dropPoint; // ponto onde os itens aparecerão (pode ser a posição do boss)

    private float healthSinceLastDrop = 0f; // controle do dano acumulado

    void Start()
    {
        SetMaxHealth((int)maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float oldHealth = currentHealth;

        currentHealth -= damage;
        SetHealth((int)currentHealth);

        // NOVO: Verifica se perdeu 30 de vida desde o último drop
        healthSinceLastDrop += oldHealth - currentHealth;

        while (healthSinceLastDrop >= 30f)
        {
            DropRandomItem();
            healthSinceLastDrop -= 30f;
        }
    }

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

        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Death");

        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            if (comp != this) comp.enabled = false;
        }

        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
    }

    // NOVO: Função de drop
    private void DropRandomItem()
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;

        int index = UnityEngine.Random.Range(0, dropPrefabs.Length);
        GameObject itemToDrop = dropPrefabs[index];

        Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;

        Instantiate(itemToDrop, spawnPosition, Quaternion.identity);
        Debug.Log("Item dropado: " + itemToDrop.name);
    }

    void EndGame()
    {
        gameEnd.transform.parent.gameObject.SetActive(true);
        gameEnd.gameObject.SetActive(true);
        gameEnd.TriggerGameWin();
    }
}
