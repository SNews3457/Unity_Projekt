//op Dagobert
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph;
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
    public PlayerMovement player;
    public Attack attack;
    public CharacterController2D Character;

    void Update()
    {
        Cursor.visible = true;
        //op Aktualisierung der visuellen Anzeige
        LevelAmount.value = LevelPoints;
        LevelAmount.maxValue = PointsNeeded;
        Points.text = LevelPoints + "/" + PointsNeeded;
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

    public void DashSkillUnlock(Button button)
    {
        Image image = GameObject.Find("Dash").GetComponent<Image>();
        if (SkillPoints > 0)
        {     
            Color tempColor = image.color;
            Image image2 = GameObject.Find("DashUpgrade").GetComponent<Image>();
            image2.color = tempColor;
            tempColor *= 0.7f; //op macht das Bild ca. 30% dunkler
            tempColor.a = 0.7f; 
            image.color = tempColor;
            Destroy(button);
            SkillPoints--;
            player.SkillDash = true;

        }
        else
        {
            // "Nein"-Wackeln
            StartCoroutine(ShakeUI(image.rectTransform));
        }
    }

    public void ShootSkillUnlock(Button button)
    {
        Image image = GameObject.Find("Shoot").GetComponent<Image>();
        if (SkillPoints > 0)
        {
            Color tempColor = image.color;
            tempColor *= 0.7f; //op macht das Bild ca. 30% dunkler
            tempColor.a = 0.7f;
            image.color = tempColor;
            Destroy (button);
            SkillPoints--;
            attack.SkillShoot = true;
        }
        else
        {
            // "Nein"-Wackeln
            StartCoroutine(ShakeUI(image.rectTransform));
        }
    }

    public void DashSkillUpgrade(Button button)
    {
        Image image = GameObject.Find("DashUpgrade").GetComponent<Image>();
        if (SkillPoints > 0 && player.SkillDash)
        {
            Color tempColor = image.color;
            tempColor *= 0.7f; //op macht das Bild ca. 30% dunkler
            tempColor.a = 0.7f;
            image.color = tempColor;
            Destroy(button);
            SkillPoints--;
            Character.m_DashForce = 40;
        }
        else
        {
            // "Nein"-Wackeln
            StartCoroutine(ShakeUI(image.rectTransform));
        }
    }

    IEnumerator ShakeUI(RectTransform target, float duration = 0.2f, float magnitude = 10f)
    {
        Vector3 originalPos = target.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            target.anchoredPosition = originalPos + new Vector3(x, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = originalPos;
    }

}
