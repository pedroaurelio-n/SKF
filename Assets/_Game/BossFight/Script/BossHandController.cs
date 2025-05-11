using System.Collections;
using UnityEngine;

public class BossHandController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Animator animator;
    [SerializeField] private BossHealth bossHealth;

    private int currentPhase = 1;

    void Start()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += OnBossHealthChanged;
        }

        PlayCurrentPhaseAnimation();
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= OnBossHealthChanged;
        }
    }

    private void OnBossHealthChanged(float currentHealth, float maxHealth)
    {
        float healthPercent = (currentHealth / maxHealth) * 100f;

        if (healthPercent <= 10f && currentPhase != 3)
        {
            currentPhase = 3;
            PlayCurrentPhaseAnimation();
        }
        else if (healthPercent <= 50f && currentPhase == 1)
        {
            currentPhase = 2;
            PlayCurrentPhaseAnimation();
        }
    }

    private void PlayCurrentPhaseAnimation()
    {
        switch (currentPhase)
        {
            case 1:
                animator.Play("Phase1Attack"); // Nome da animação que você criará
                break;
            case 2:
                animator.Play("Phase2Attack");
                break;
            case 3:
                animator.Play("Phase3Attack");
                break;
        }
    }
}
