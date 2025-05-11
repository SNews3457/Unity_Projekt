using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Menü-Elemente")]
    public Button[] menuButtons;
    public GameObject[] panels; // Die Panels, die angezeigt werden sollen (gleiche Reihenfolge wie Buttons)

    [Header("Auswahl-Icons")]
    public Image leftIcon;
    public Image rightIcon;
    public float iconOffset = 100f;

    [Header("Audio")]
    public AudioClip navigateSound;
    public AudioClip selectSound;
    public AudioSource audioSource;

    [Header("Optionales Menü-Panel")]
    public GameObject menuPanel;

    private int currentIndex = 0;
    private GameObject activePanel = null;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        if (activePanel == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
                UpdateSelection();
                PlaySound(navigateSound);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex = (currentIndex + 1) % menuButtons.Length;
                UpdateSelection();
                PlaySound(navigateSound);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleSelection();
            }
        }
        else
        {
            // Wenn ein Panel aktiv ist, kann es mit Leertaste wieder geschlossen werden
            if (Input.GetKeyDown(KeyCode.Space))
            {
                activePanel.SetActive(false);
                activePanel = null;
                PlaySound(selectSound);
            }
        }
    }

    void HandleSelection()
    {
        PlaySound(selectSound);

        // Sonderfall "Fortsetzen"
        if (menuButtons[currentIndex].name == "Fortsetzen")
        {
            if (menuPanel != null)
                menuPanel.SetActive(false);
        }
        else
        {
            // Aktuelles Panel anzeigen
            if (panels != null && currentIndex < panels.Length && panels[currentIndex] != null)
            {
                panels[currentIndex].SetActive(true);
                activePanel = panels[currentIndex];
            }
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
