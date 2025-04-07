using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] SpriteRenderer sprite;
    
    public void Die ()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public void StartInvincibilityFlash (float duration)
    {
        StartCoroutine(FlashRoutine(duration));
    }

    IEnumerator FlashRoutine (float duration)
    {
        float timer = 0f;
        Color originalColor = sprite.color;
        float flashSpeed = 0.1f;
        WaitForSeconds waitForFlash = new(flashSpeed);
        
        while (timer < duration)
        {
            sprite.color = Color.white;
            yield return waitForFlash;

            sprite.color = originalColor;
            yield return waitForFlash;
            
            timer += flashSpeed * 2;
            yield return null;
        }

        sprite.color = originalColor;
    }
}