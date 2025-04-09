using System;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] TextMeshProUGUI waveNumber;

    void OnEnable ()
    {
        EventManager.OnWaveStarted += UpdateWaveCount;
        EventManager.OnPlayerDeath += DisableWaveCounter;
        EventManager.OnAllWavesDefeated += DisableWaveCounter;
    }

    void OnDisable ()
    {
        EventManager.OnWaveStarted -= UpdateWaveCount;
        EventManager.OnPlayerDeath -= DisableWaveCounter;
        EventManager.OnAllWavesDefeated -= DisableWaveCounter;
    }

    void UpdateWaveCount (int current, int total)
    {
        container.SetActive(true);
        waveNumber.text = $"{current}/{total}";
    }
    
    void DisableWaveCounter ()
    {
        container.SetActive(false);
    }
}
