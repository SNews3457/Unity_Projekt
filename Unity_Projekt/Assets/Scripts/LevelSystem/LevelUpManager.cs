//op Dagobert
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelUpManager : MonoBehaviour
{
    public float LevelPoints = 0; //aktuele LevelPunkte
    public float PointsNeeded = 2; //Punkte die für den Levelaufstieg benötigt werden
    public float Level = 0; //op aktuels Level
    public Slider LevelAmount;
    public TMPro.TMP_Text Points;
    void Update()
    {
        LevelAmount.value = LevelPoints;
        Points.text = "LevelUp:         " + LevelPoints + "/" + PointsNeeded;
    }
}
