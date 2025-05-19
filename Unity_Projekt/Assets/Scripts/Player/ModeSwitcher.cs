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

    [Header("LigthParameters")]
    [SerializeField] private float LigthSpeed;
    bool isLight = true;

    [Header("DarkParameters")]
    [SerializeField] private float ShadowSpeed;
    bool isDark = false;

    private void Start()
    {
        Player = GetComponent<CharacterController2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isDark)
        {
            isDark =  false;
            isLight = true;
            Debug.ClearDeveloperConsole();
            Debug.Log("Dark = " + isDark);
            Debug.Log("Light = " + isLight);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isLight)
        {
            isLight = false;
            isDark = true;
            Debug.ClearDeveloperConsole();
            Debug.Log("Dark = " + isDark);
            Debug.Log("Light = " + isLight);
        }
    }
}
