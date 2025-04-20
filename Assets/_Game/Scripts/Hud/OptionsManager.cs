using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Toggle fullscreenToggle;

    void Start()
    {
        // Carrega configurações salvas
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        ApplySettings(); // Aplica os valores ao iniciar

        // Adiciona os listeners
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        FindObjectOfType<GunSystem>()?.UpdateSensitivity(value);
    }



    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void ApplySettings()
    {
        SetVolume(volumeSlider.value);
        SetSensitivity(sensitivitySlider.value);
        SetFullscreen(fullscreenToggle.isOn);
    }
}
