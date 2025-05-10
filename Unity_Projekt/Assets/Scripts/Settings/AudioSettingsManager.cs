using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("UI")]
    public Slider volumeSlider;

    [Header("Einstellungen")]
    public float defaultVolume = 1.0f;

    private const string PlayerPrefsKey = "MasterVolume";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(PlayerPrefsKey, defaultVolume);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        // Speichern
        PlayerPrefs.SetFloat(PlayerPrefsKey, volume);
        PlayerPrefs.Save();
    }
}
