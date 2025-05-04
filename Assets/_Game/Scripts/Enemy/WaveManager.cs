//SceneTransitionManager.cs
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Referências")]
    //[SerializeField] private WaveManager waveManager;
    [SerializeField] private BossController bossController;   // seu script de boss
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider[] sceneColliders;       // coliders do cenário que devem “cair”
    [SerializeField] private float cameraShakeDuration = 1f;
    [SerializeField] private float cameraShakeMagnitude = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float dropDelay = 0.5f;          // tempo até o cenário começar a “cair”
    [SerializeField] private float fightStartYOffset = -5f;   // distância do player ao boss para ativar a luta

    private bool transitioning = false;
    private Vector3 camOriginalPos;

    void OnEnable()
    {
        //EventManager.AllWavesDefeated += OnAllWavesDefeated;
    }

    void OnDisable()
    {
        //EventManager.AllWavesDefeated -= OnAllWavesDefeated;
    }

    private void OnAllWavesDefeated()
    {
        if (!transitioning)
            StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        transitioning = true;

        // 1) Camera shake rápido
        camOriginalPos = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            float y = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            mainCamera.transform.position = camOriginalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = camOriginalPos;

        // 2) Pequena espera
        yield return new WaitForSeconds(dropDelay);

        // 3) Desativa colliders do cenário (eles continuam com RigidBody para cair)
        foreach (var col in sceneColliders)
        {
            if (col.attachedRigidbody == null)
            {
                var rb = col.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
            }
            col.enabled = false;
        }

        // 4) Aguarda até o player cair perto do boss
        Vector3 bossPos = bossController.transform.position;
        while (player.position.y - bossPos.y > fightStartYOffset)
            yield return null;

        // 5) Inicia a luta: ativa o boss e sua HUD
        bossController.gameObject.SetActive(true);
        bossController.BeginFight();      // método que você deve expor pra começar o ciclo
    }
}
