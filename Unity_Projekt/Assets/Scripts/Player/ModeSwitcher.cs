//op Dagobert
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class ModeSwitcher : MonoBehaviour
{
    [Header("References")]
    private CharacterController2D Player;
    private SpriteRenderer sp;
    private PlayerMovement Movement;

    [Header("LigthParameters")]
    [SerializeField] private float LigthSpeed = 50;
    bool isLight = true;

    [Header("DarkParameters")]
    [SerializeField] private float ShadowSpeed = 40;
    bool isDark = false;
    public enum PlayerMode { Light, Dark }
    private PlayerMode currentMode;

    private void Start()
    {
        Player = GetComponent<CharacterController2D>();
        sp = GetComponent<SpriteRenderer>();
        Movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            currentMode = currentMode == PlayerMode.Light ? PlayerMode.Dark : PlayerMode.Light;
            Debug.ClearDeveloperConsole();
            Debug.Log("Switched to " + currentMode + " mode");
            UpdateMode();
        }
    }

    public void UpdateMode()
    {
        switch (currentMode)
        {
            case PlayerMode.Light:
                Movement.runSpeed = LigthSpeed;
                sp.color = Color.white;
                break;
            case PlayerMode.Dark:
                Movement.runSpeed = ShadowSpeed;
                sp.color = Color.black;
                break;
        }
    }

}
