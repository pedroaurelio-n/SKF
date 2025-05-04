//SceneTransitionManager.cs
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Refer�ncias")]
    //[SerializeField] private WaveManager waveManager;
    [SerializeField] private BossController bossController;   // seu script de boss
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider[] sceneColliders;       // coliders do cen�rio que devem �cair�
    [SerializeField] private float cameraShakeDuration = 1f;
    [SerializeField] private float cameraShakeMagnitude = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float dropDelay = 0.5f;          // tempo at� o cen�rio come�ar a �cair�
    [SerializeField] private float fightStartYOffset = -5f;   // dist�ncia do player ao boss para ativar a luta

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

        // 1) Camera shake r�pido
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

        // 3) Desativa colliders do cen�rio (eles continuam com RigidBody para cair)
        foreach (var col in sceneColliders)
        {
            if (col.attachedRigidbody == null)
            {
                var rb = col.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
            }
            col.enabled = false;
        }

        // 4) Aguarda at� o player cair perto do boss
        Vector3 bossPos = bossController.transform.position;
        while (player.position.y - bossPos.y > fightStartYOffset)
            yield return null;

        // 5) Inicia a luta: ativa o boss e sua HUD
        bossController.gameObject.SetActive(true);
        bossController.BeginFight();      // m�todo que voc� deve expor pra come�ar o ciclo
    }
}
