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
        public bool completed;
    }

    public enum AchievementType
    {
        Collect,
        KillEnemies,
        PlayTime,
        Custom // Placeholder for anything else
    }

    [Header("Game State Simulations")]
    public LevelUpManager levelUpManager;
    public int enemiesKilled;
    public float playTimeInSeconds;

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
        // Simulate time passing
        playTimeInSeconds += Time.deltaTime;

        CheckAchievementProgress();
        UpdateUI();
    }

    void CheckAchievementProgress()
    {
        foreach (var achievement in achievements)
        {
            switch (achievement.type)
            {
                case AchievementType.Collect:
                    achievement.progress = Mathf.Clamp01(levelUpManager.LevelPoints / 100f);
                    break;
                case AchievementType.KillEnemies:
                    achievement.progress = Mathf.Clamp01(enemiesKilled / 50f);
                    break;
                case AchievementType.PlayTime:
                    achievement.progress = Mathf.Clamp01(playTimeInSeconds / 300f); // 5 min
                    break;
                case AchievementType.Custom:
                    // Optional: allow manual progress update elsewhere
                    break;
            }

            if (achievement.progress >= 1f && !achievement.completed)
            {
                achievement.completed = true;
                OnAchievementCompleted(achievement);
            }
        }
    }

    void OnAchievementCompleted(Achievement a)
    {
        Debug.Log($"Achievement unlocked: {a.title}");
        //dagobert trigger sound, visual effect, save state
    }

    void InitializeUI()
    {
        foreach (GameObject go in instantiatedAchievements)
        {
            Destroy(go);
        }

        instantiatedAchievements.Clear();
        titleTexts.Clear();
        progressBars.Clear();

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
            titleTexts[i].text = $"{achievements[i].title} ({Mathf.RoundToInt(achievements[i].progress * 100)}%)";
        }
    }
}
