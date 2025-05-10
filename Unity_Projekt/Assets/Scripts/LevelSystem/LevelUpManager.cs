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


    void Update()
    {
        Debug.Log(LevelPoints);
        
        LevelAmount.value = LevelPoints;
        LevelAmount.maxValue = PointsNeeded;
        Points.text = "LevelUp:         " + LevelPoints + "/" + PointsNeeded;

    }

}
