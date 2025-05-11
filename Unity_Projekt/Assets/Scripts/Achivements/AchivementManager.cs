using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [System.Serializable]
    public class Achievement
    {
        public string title;
        [Range(0, 1)] public float progress;
    }

    [Header("UI References")]
    public GameObject achievementPrefab;
    public Transform achievementContainer;

    [Header("Achievements")]
    public List<Achievement> achievements = new List<Achievement>();

    private List<GameObject> instantiatedAchievements = new List<GameObject>();
    private List<TMP_Text> titleTexts = new List<TMP_Text>();
    private List<Slider> progressBars = new List<Slider>();

    void Start()
    {
        InitializeUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void InitializeUI()
    {
        // Clear existing
        foreach (GameObject go in instantiatedAchievements)
        {
            Destroy(go);
        }

        instantiatedAchievements.Clear();
        titleTexts.Clear();
        progressBars.Clear();

        // Instantiate once
        foreach (var achievement in achievements)
        {
            GameObject go = Instantiate(achievementPrefab, achievementContainer);
            TMP_Text titleText = go.GetComponentInChildren<TMP_Text>();
            Slider progressBar = go.GetComponentInChildren<Slider>();

            if (titleText != null) titleText.text = achievement.title;
            if (progressBar != null) progressBar.value = achievement.progress;

            instantiatedAchievements.Add(go);
            titleTexts.Add(titleText);
            progressBars.Add(progressBar);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < achievements.Count && i < progressBars.Count; i++)
        {
            progressBars[i].value = achievements[i].progress;
            // Optional: update title too (e.g., with percentage)
            titleTexts[i].text = $"{achievements[i].title} ({Mathf.RoundToInt(achievements[i].progress * 100)}%)";
        }
    }
}
