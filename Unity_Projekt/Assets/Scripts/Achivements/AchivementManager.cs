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
        public AchievementType type;
        public float targetValue = 100f; // <-- Benutzerdefiniert im Inspector
        public bool completed;
    }

    public enum AchievementType
    {
        CollectCoins,
        KillEnemies,
        PlayTime,
        Custom // Optional, kann ignoriert oder manuell gesteuert werden
    }

    [Header("Game State")]
    public int coinsCollected;
    public int enemiesKilled;
    public float playTimeInSeconds;

    [Header("UI References")]
    public GameObject achievementPrefab;
    public Transform achievementContainer;
    public LevelUpManager levelUpManager;

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
        playTimeInSeconds += Time.deltaTime;

        CheckAchievementProgress();
        UpdateUI();
    }

    void CheckAchievementProgress()
    {
        foreach (var achievement in achievements)
        {
            float currentValue = 0f;

            switch (achievement.type)
            {
                case AchievementType.CollectCoins:
                    currentValue = levelUpManager.SkillPoints;
                    break;
                case AchievementType.KillEnemies:
                    currentValue = enemiesKilled;
                    break;
                case AchievementType.PlayTime:
                    currentValue = playTimeInSeconds;
                    break;
                case AchievementType.Custom:
                    continue; // Custom logik extern verwalten
            }

            achievement.progress = Mathf.Clamp01(currentValue / Mathf.Max(achievement.targetValue, 0.01f));

            if (achievement.progress >= 1f && !achievement.completed)
            {
                achievement.completed = true;
                OnAchievementCompleted(achievement);
            }
        }
    }

    void OnAchievementCompleted(Achievement a)
    {
        Debug.Log($" Achievement freigeschaltet: {a.title}");
        // Hier z. B. Sound, Animation oder Popup einbauen
    }

    void InitializeUI()
    {
        foreach (GameObject go in instantiatedAchievements)
            Destroy(go);

        instantiatedAchievements.Clear();
        titleTexts.Clear();
        progressBars.Clear();

        foreach (var achievement in achievements)
        {
            GameObject go = Instantiate(achievementPrefab, achievementContainer);
            TMP_Text titleText = go.GetComponentInChildren<TMP_Text>();
            Slider progressBar = go.GetComponentInChildren<Slider>();

            titleTexts.Add(titleText);
            progressBars.Add(progressBar);
            instantiatedAchievements.Add(go);

            if (titleText != null)
                titleText.text = $"{achievement.title} (0%)";
            if (progressBar != null)
                progressBar.value = achievement.progress;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            var achievement = achievements[i];
            progressBars[i].value = achievement.progress;
            titleTexts[i].text = $"{achievement.title} ({Mathf.RoundToInt(achievement.progress * 100)}%)";
        }
    }
}
