// SceneTransitionManager.cs
using System.Collections;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private BossController bossController;
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider[] sceneColliders;
    [SerializeField] private GameObject waveHUD;        // O painel UI que mostra �Wave X/Y�
    [SerializeField] private float cameraShakeDuration = 1f;
    [SerializeField] private float cameraShakeMagnitude = 0.1f;
    [SerializeField] private float dropDelay = 0.5f;
    [SerializeField] private float fightStartYOffset = -5f;
    [SerializeField] private GameObject[] arrowObjects;

    private bool transitioning = false;
    private Vector3 camOriginalPos;

    void OnEnable()
    {
        EventManager.OnAllWavesDefeated += OnAllWavesDefeated;
    }

    void OnDisable()
    {
        EventManager.OnAllWavesDefeated -= OnAllWavesDefeated;
    }

    private void OnAllWavesDefeated()
    {
        if (!transitioning)
            StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        transitioning = true;

        foreach (GameObject obj in arrowObjects)
        {
            obj.SetActive(true);
        }

        // 1) Shake
        camOriginalPos = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < cameraShakeDuration)
        {
            float x = (Random.value * 2 - 1) * cameraShakeMagnitude;
            float y = (Random.value * 2 - 1) * cameraShakeMagnitude;
            mainCamera.transform.position = camOriginalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = camOriginalPos;

        // 2) Espera
        yield return new WaitForSeconds(dropDelay);

        // 3) Desmonta cen�rio
        foreach (var col in sceneColliders)
        {
            col.enabled = false;
            if (col.attachedRigidbody == null)
            {
                var rb = col.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
            }
        }

        // 4) Oculta HUD de waves
        if (waveHUD != null)
            waveHUD.SetActive(false);

        // 5) Espera player cair perto do boss
        Vector3 bossPos = bossController.transform.position;
        while (player.position.y - bossPos.y > fightStartYOffset)
            yield return null;

        // 6) Ativa boss e inicia fight
        bossController.gameObject.SetActive(true);
        bossController.BeginFight();

        // 7) Dispara evento para HUD do boss se inscrever
        EventManager.TriggerBossFightStarted();
    }
}
