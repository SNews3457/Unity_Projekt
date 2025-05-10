//op Dagobert
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelUpManager : MonoBehaviour
{
    public float LevelPoints; //op aktuele LevelPunkte
    public float PointsNeeded = 2; //op Punkte die für den Levelaufstieg benötigt werden
    public float Level = 0; //op aktuels Level
    public Slider LevelAmount; //op Slider
    public TMPro.TMP_Text Points;
    public TMP_Text SkillPonts;
    float SkillPoints;


    void Update()
    { 
        //op Aktualisierung der visuellen Anzeige
        LevelAmount.value = LevelPoints;
        LevelAmount.maxValue = PointsNeeded;
        Points.text = "LevelUp:         " + LevelPoints + "/" + PointsNeeded;
        SkillPonts.text = SkillPoints.ToString();
        //op Levelaufstieg
        if ( LevelPoints >= PointsNeeded)
        {
            SkillPoints++;
            Level++;
            PointsNeeded = PointsNeeded * 2;
            LevelPoints = 0;
        }
    }

}
