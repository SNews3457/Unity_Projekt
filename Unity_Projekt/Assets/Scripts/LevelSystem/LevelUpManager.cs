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
    public float PointsNeeded = 2; //op Punkte die f�r den Levelaufstieg ben�tigt werden
    public float Level = 0; //op aktuels Level
    public Slider LevelAmount; //op Slider
    public TMPro.TMP_Text Points;
    public TMP_Text SkillPonts;
    public float SkillPoints;
    public GameObject SkillTree;
    public PlayerMovement player;
    public Image SkillPointBg;
    public Attack attack;
    public CharacterController2D Character;
    Coroutine skillPointEffectRoutine;
    bool skillPointAvailable = false;
    AchievementManager AchievementManager;
    public bool canOpenOptionMenu = true;
    void Update()
    {
        Cursor.visible = true;
        //op Aktualisierung der visuellen Anzeige
        LevelAmount.value = LevelPoints;
        LevelAmount.maxValue = PointsNeeded;
        Points.text = LevelPoints + "/" + PointsNeeded;
        SkillPonts.text = SkillPoints.ToString();

        //op Effekt dewaktivieren wenn keine skillpoint verf�gbar sind
        if (SkillPoints <= 0)
            skillPointAvailable = false;

        //op Levelaufstieg
        if (LevelPoints >= PointsNeeded)
        {
            SkillPoints++;
            Level++;
            PointsNeeded *= 2;
            LevelPoints = 0;

            // Starte Effekt
            skillPointAvailable = true;
            if (skillPointEffectRoutine != null)
                StopCoroutine(skillPointEffectRoutine);

            skillPointEffectRoutine = StartCoroutine(SkillPointEffectLoop());
        }


        //op mit escape wird der Skilltree wieder geschlossen
        if (Input.GetKey(KeyCode.Escape))
        {
            SkillTree.SetActive(false);
            canOpenOptionMenu = true;
        }

        if (Input.GetKey(KeyCode.R))
        {
            GoToSkillTree();
        }

    }

    //op Skilltree wird ge�ffnet wenn die Anzeige angeklickt wird
    public void GoToSkillTree()
    {
        canOpenOptionMenu =false;
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
            tempColor.a = 0.7f; 
            image.color = tempColor;
            StartCoroutine(PlayUnlockEffect(image));
            Destroy(button);
            SkillPoints--;
            player.SkillDash = true;

        }
        else
        {
            //op "Nein"-Wackeln
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
            StartCoroutine(PlayUnlockEffect(image));
            Destroy (button);
            SkillPoints--;
            attack.SkillShoot = true;
        }
        else
        {
            //op "Nein"-Wackeln
            StartCoroutine(ShakeUI(image.rectTransform));
        }
    }

    public void DoubleJumpSkillUnlock(Button button)
    {
        Image image = GameObject.Find("DoubleJump").GetComponent<Image>();
        if (SkillPoints > 0)
        {
            Color tempColor = image.color;
            tempColor *= 0.7f; //op macht das Bild ca. 30% dunkler
            tempColor.a = 0.7f;
            image.color = tempColor;
            StartCoroutine(PlayUnlockEffect(image));
            Destroy(button);
            SkillPoints--;
            Character.SkillDoubkeJump = true;
        }
        else
        {
            //op "Nein"-Wackeln
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
            StartCoroutine(PlayUnlockEffect(image));
            Destroy(button);
            SkillPoints--;
            Character.m_DashForce = 40;
        }
        else
        {
            //op "Nein"-Wackeln
            StartCoroutine(ShakeUI(image.rectTransform));
        }
    }

    IEnumerator SkillPointEffectLoop()
    {
        Vector3 originalScale = SkillPointBg.rectTransform.localScale;
        Color originalColor = SkillPointBg.color;

        float time = 0;

        while (skillPointAvailable)
        {
            float t = Mathf.PingPong(time * 2f, 1f); //op PingPong f�r Loop-Effekt

            float scale = Mathf.Lerp(1f, 1.2f, t);
            SkillPointBg.rectTransform.localScale = new Vector3(scale, scale, 1f);

            SkillPointBg.color = Color.Lerp(originalColor, new Color(1f, 0.85f, 0.4f), t);

            time += Time.deltaTime;
            yield return null;
        }

        //op Reset
        SkillPointBg.rectTransform.localScale = originalScale;
        SkillPointBg.color = originalColor;
    }

    IEnumerator PlayUnlockEffect(Image image)
    {
        Vector3 originalScale = image.rectTransform.localScale;
        Color originalColor = image.color;

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            float t = time / duration;
            float scale = Mathf.Lerp(1f, 1.3f, Mathf.Sin(t * Mathf.PI));
            image.rectTransform.localScale = new Vector3(scale, scale, 1f);
            image.color = Color.Lerp(originalColor, Color.white, Mathf.Sin(t * Mathf.PI));
            time += Time.deltaTime;
            yield return null;
        }

        image.rectTransform.localScale = originalScale;
        image.color = originalColor;

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
