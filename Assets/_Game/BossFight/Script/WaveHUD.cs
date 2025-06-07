using UnityEngine;
using TMPro;

public class WaveHUD : MonoBehaviour
{
    [Header("Refer�ncias UI")]
    [SerializeField] private GameObject wavePanel;  // Painel que cont�m tudo de waves
    [SerializeField] private TMP_Text waveText;     // Texto que mostra �1/3�, �2/3�, etc.

    void OnEnable()
    {
        EventManager.OnWaveStarted += OnWaveStarted;
        EventManager.OnAllWavesDefeated += OnAllWavesDefeated;
    }

    void OnDisable()
    {
        EventManager.OnWaveStarted -= OnWaveStarted;
        EventManager.OnAllWavesDefeated -= OnAllWavesDefeated;
    }

    private void OnWaveStarted(int currentWave, int totalWaves)
    {
        // Mostra o painel (caso estivesse oculto) e atualiza texto
        wavePanel.SetActive(true);
        waveText.text = $"{currentWave}/{totalWaves}";
    }

    private void OnAllWavesDefeated()
    {
        // Esconde completamente o painel de waves
        wavePanel.SetActive(false);
    }
}
