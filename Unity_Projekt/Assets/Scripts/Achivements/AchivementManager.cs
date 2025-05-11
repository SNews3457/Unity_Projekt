using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Transform achievementContainer;

    [System.Serializable]
    public class Achievement
    {
        public string name;
        public float currentAmount;
        public float requiredAmount;
    }

    public List<Achievement> achievements;

    private List<GameObject> uiEntries = new List<GameObject>();

    void Start()
    {
        // Einmalige UI-Erstellung
        foreach (Achievement achievement in achievements)
        {
            GameObject entry = Instantiate(achievementPrefab, achievementContainer);
            uiEntries.Add(entry);

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            Slider slider = entry.GetComponentInChildren<Slider>();

            text.text = achievement.name;
            slider.maxValue = achievement.requiredAmount;
        }
    }

    void Update()
    {
        // Nur Werte updaten
        for (int i = 0; i < achievements.Count; i++)
        {
            Achievement achievement = achievements[i];
            GameObject entry = uiEntries[i];

            Slider slider = entry.GetComponentInChildren<Slider>();
            slider.value = achievement.currentAmount;
        }
    }
}
