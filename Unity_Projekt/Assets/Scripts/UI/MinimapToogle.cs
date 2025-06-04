using UnityEngine;

public class MinimapToggle : MonoBehaviour
{
    public GameObject bigMinimapPanel;
    public Animator animator;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMinimap();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseMinimap();
        }
    }

    public void OpenMinimap()
    {
        bigMinimapPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseMinimap()
    {
        bigMinimapPanel.SetActive(false);
        Time.timeScale = 1f; // Spiel fortsetzen
        isOpen = false;
    }
}
