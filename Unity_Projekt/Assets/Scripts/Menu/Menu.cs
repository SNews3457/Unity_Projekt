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

    private int currentIndex = 0;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
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
            PlaySound(selectSound);
            if (sceneNames != null && currentIndex < sceneNames.Length)
            {
                SceneManager.LoadScene(sceneNames[currentIndex]);
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
