using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Menü-Elemente")]
    public Button[] menuButtons;
    public string[] sceneNames;

    [Header("Auswahl-Icons")]
    public Image leftIcon;
    public Image rightIcon;
    public float iconOffset = 100f;

    [Header("Audio")]
    public AudioClip navigateSound;
    public AudioClip selectSound;
    public AudioSource audioSource;

    [Header("Optionales Menü-Panel")]
    public GameObject menuPanel; // Das Panel, das deaktiviert werden soll
    public GameObject AchivementPanel;

    public LevelUpManager LevelUpManager;
    bool isActiv = false;
    private int currentIndex = 0;
    public GameObject Option;
    bool AchivementActiv = false; //dagobert überprüfe ob Achivements Aktiv sind
    public InventoryManager inventoryManager;
    void Start()
    {
        UpdateSelection();
    }

    public void StartGame()
    {
        PlaySound(selectSound);

        if (sceneNames != null && currentIndex < sceneNames.Length)
        {
            string scene = sceneNames[currentIndex];
            string sceneName = SceneManager.GetActiveScene().name;
            if (scene == "QUIT")
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else if (scene == "Fortsetzen")
            {
                Time.timeScale = 1.0f;  
                if (menuPanel != null)
                    menuPanel.SetActive(false); // Menü schließen
            }
            else if (scene == "Achievements" && isActiv && sceneName != "Menu")
            {
                menuPanel.SetActive(false); // Menü schließen
                AchivementPanel.SetActive(true); //dagobert öffnen
                AchivementActiv = true;
            }
            else
            {
                SceneManager.LoadScene(scene);
                Debug.Log("Gut");
            }
            Debug.Log(scene);
        }
    }

    void Update()
    {
        //dagobert Menü öffnen bzw schließen
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            OpenClose();
        }
        string sceneName = SceneManager.GetActiveScene().name;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Select();
        }
    }


    public void Select()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (isActiv)
        {
            StartGame();
        }
        else if (sceneName == "Menu")
        {
            StartGame();
        }
    }
    public void Up()
    {
        if (!AchivementActiv && isActiv)
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateSelection();
            PlaySound(navigateSound);
        }

    }
    public void Down()
    {
        if (!AchivementActiv && isActiv)
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            UpdateSelection();
            PlaySound(navigateSound);
        }
    }
    public void OpenClose()
    {
        if (!isActiv && LevelUpManager.canOpenOptionMenu && !inventoryManager.InventoryisActiv)
        {
            Time.timeScale = 0;
            isActiv = true;
            Option.SetActive(true);
        }
        else if (isActiv && !AchivementActiv)
        {
            Time.timeScale = 1;
            isActiv = false;
            Option.SetActive(false);
        }
        back();
    }

    public void back()
    {
        if (AchivementActiv) //Dagobert Achivement schließen
        {
            Option.SetActive(true);
            AchivementActiv = false;
            isActiv = true;
            AchivementPanel.SetActive(false);
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].interactable = (i == currentIndex);
        }

        RectTransform selectedRect = menuButtons[currentIndex].GetComponent<RectTransform>();
        Vector3 position = selectedRect.position;

        leftIcon.transform.position = position + Vector3.left * iconOffset;
        rightIcon.transform.position = position + Vector3.right * iconOffset;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
