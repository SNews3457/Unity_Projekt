// op dagobert
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public Button Continue;
    public GameObject OptionsObject;
    public Button Achivements;
    public Button settings;
    public Button MainMenu;
    bool isActiv = false;
    public GameObject AchivementPanel;
    public LevelUpManager LevelUpManager;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActiv && LevelUpManager.canOpenOptionMenu)
            {
                Time.timeScale = 0;
                isActiv = true;
                OptionsObject.SetActive(true);
            }
            else if(isActiv )
            {
                Time.timeScale = 1;
                isActiv = false;
                OptionsObject.SetActive(false);
            }

        }
    }

    public void Continues()
    {
        Time.timeScale = 1;
        isActiv = false;
        OptionsObject.SetActive(false);
    }

    public void AchivmentOpen ()
    {
        OptionsObject.SetActive(false);
        AchivementPanel.SetActive(true);
    }

    public void SettingsOpen()
    {

    }

    public void MainMenueOpen()
    {
        SceneManager.LoadScene("Menu");
    }
}
