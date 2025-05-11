using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualitySettingsController : MonoBehaviour
{
    public Slider qualitySlider;
    public TMP_Text qualityLabel; // Optional – zeigt den Namen an

    void Start()
    {
        // Initialisieren mit gespeichertem Wert oder aktuellem Level
        int savedLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedLevel);
        qualitySlider.maxValue = QualitySettings.names.Length - 1;
        qualitySlider.value = savedLevel;

        UpdateLabel(savedLevel);
        qualitySlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        int level = Mathf.RoundToInt(value);
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("QualityLevel", level);
        UpdateLabel(level);
    }

    void UpdateLabel(int level)
    {
        if (qualityLabel != null)
        {
            qualityLabel.text = "" + QualitySettings.names[level];
        }
    }
}
