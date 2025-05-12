using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider volumeSlider;        // Referenz zum UI-Slider
    public AudioSource musicSource;    // Die Audioquelle mit Musik
    private const string VolumeKey = "MusicVolume";

    void Start()
    {
        // Lautstärke aus PlayerPrefs laden, oder Standardwert 1.0f verwenden
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1.0f);
        volumeSlider.value = savedVolume;
        musicSource.volume = savedVolume;

        // Event hinzufügen
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float newVolume)
    {
        musicSource.volume = newVolume;
        PlayerPrefs.SetFloat(VolumeKey, newVolume);
        PlayerPrefs.Save(); // Speichert sofort in Datei
    }

    private void OnDestroy()
    {
        // Event entfernen, um Speicherlecks zu vermeiden
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }
}

