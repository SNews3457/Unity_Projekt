using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    [System.Serializable]
    public class Achievement
    {
        public string title;
        [Range(0, 1)] public float progress;
        public AchievementType type;
        public float targetValue = 100f; 
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
    public float levelPoints = 0f; 
    private float lastKnownSkillPoints = 0; // Zwischenspeicher
    public GameObject PopUp;

    [Header("UI References")]
    public GameObject achievementPrefab;
    public Transform achievementContainer;
    public LevelUpManager levelUpManager;

    [Header("Achievements")]
    public List<Achievement> achievements = new List<Achievement>();

    private List<GameObject> instantiatedAchievements = new List<GameObject>();
    private List<TMP_Text> titleTexts = new List<TMP_Text>();
    private List<Slider> progressBars = new List<Slider>();


    [Header("Popup Settings")]
    public GameObject popupPrefab; 
    public Transform popupParent;  
    public float popupDuration = 2f;
    void Start()
    {
        InitializeUI();
    }

    void Update()
    {
        playTimeInSeconds += Time.deltaTime;

        TrackLevelPoints();
        CheckAchievementProgress();
        UpdateUI();
        playTimeInSeconds += Time.deltaTime;

        CheckAchievementProgress();
        UpdateUI();
    }

    void TrackLevelPoints()
    {
        if (levelUpManager != null)
        {
            float currentSkillPoints = levelUpManager.LevelPoints;
            if (currentSkillPoints > lastKnownSkillPoints)
            {
                float delta = currentSkillPoints - lastKnownSkillPoints;
                levelPoints += delta;
                lastKnownSkillPoints = currentSkillPoints;
            }
        }
    }

    void CheckAchievementProgress()
    {
        foreach (var achievement in achievements)
        {
            float currentValue = 0f;

            switch (achievement.type)
            {
                case AchievementType.CollectCoins:
                    currentValue = levelPoints;
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
        Debug.Log($"✅ Achievement freigeschaltet: {a.title}");
        StartCoroutine(ShowPopup($"Achievement freigeschaltet: {a.title}"));
    }
    IEnumerator ShowPopup(string message)
    {
        GameObject popup = Instantiate(popupPrefab, popupParent);

        RectTransform rect = popup.GetComponent<RectTransform>();
        TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
        text.text = message;

        // Größe ermitteln
        float height = rect.rect.height;

        // Startposition: außerhalb des oberen Rands
        Vector2 startPos = new Vector2(0, height + 100);

        // Zielposition: knapp unter oberem Rand (z. B. -10 bei top-anchored Element)
        Vector2 endPos = new Vector2(0, -10); // anchoredPosition.y = -10 bedeutet: fast oben

        rect.anchoredPosition = startPos;

        float animTime = 0.4f;
        float t = 0;

        // Einblenden
        while (t < animTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / animTime);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, p);
            yield return null;
        }

        rect.anchoredPosition = endPos;

        yield return new WaitForSeconds(popupDuration);

        // Ausblenden
        t = 0;
        while (t < animTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / animTime);
            rect.anchoredPosition = Vector2.Lerp(endPos, startPos, p);
            yield return null;
        }

        Destroy(popup);
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
