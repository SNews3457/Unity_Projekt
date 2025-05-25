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

    void OpenMinimap()
    {
        bigMinimapPanel.SetActive(true);
        animator.SetTrigger("Show");
        isOpen = true;
    }

    void CloseMinimap()
    {
        bigMinimapPanel.SetActive(false);
        Time.timeScale = 1f; // Spiel fortsetzen
        isOpen = false;
    }
}
