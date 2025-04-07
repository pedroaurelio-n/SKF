using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] TextMeshProUGUI waveNumber;

    public void UpdateWaveCount (int current, int total)
    {
        container.SetActive(true);
        waveNumber.text = $"{current}/{total}";
    }
    
    public void DisableWaveCounter ()
    {
        container.SetActive(false);
    }
}
