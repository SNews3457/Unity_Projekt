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
    public LevelUpManager LevelUpManager;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActiv && LevelUpManager.canOpenOptionMenu)
            {
                isActiv = true;
                OptionsObject.SetActive(true);
            }
            else if(isActiv )
            {
                isActiv = false;
                OptionsObject.SetActive(false);
            }

        }
    }

    public void Continues()
    {
        isActiv = false;
        OptionsObject.SetActive(false);
    }

    public void AchivmentOpen ()
    {

    }

    public void SettingsOpen()
    {

    }

    public void MainMenueOpen()
    {
        SceneManager.LoadScene("Menu");
    }
}
