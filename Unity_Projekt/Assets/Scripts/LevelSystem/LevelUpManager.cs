//op Dagobert
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class LevelUpManager : MonoBehaviour
{
    public float LevelPoints; //op aktuele LevelPunkte
    public float PointsNeeded = 2; //op Punkte die für den Levelaufstieg benötigt werden
    public float Level = 0; //op aktuels Level
    public Slider LevelAmount; //op Slider
    public TMPro.TMP_Text Points;
    public TMP_Text SkillPonts;
    float SkillPoints;
    public GameObject SkillTree;


    void Update()
    {
        Cursor.visible = true;
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

        //op mit escape wird der Skilltree wieder geschlossen
        if (Input.GetKey(KeyCode.Escape))
        {
            SkillTree.SetActive(false);
        }
    }

    //op Skilltree wird geöffnet wenn die Anzeige angeklickt wird
    public void GoToSkillTree()
    {
        Debug.Log("OpenSkillTree");
        SkillTree.SetActive(true);
    }


}
