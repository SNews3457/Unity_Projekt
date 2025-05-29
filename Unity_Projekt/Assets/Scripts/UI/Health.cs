using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public List<GameObject> heartImages;  // Herz-UI-Objekte im Inspector zuweisen
    public CharacterController2D controller;

    private float lastLives;

    void Start()
    {
        lastLives = controller.lives;
        UpdateHearts();
    }

    void Update()
    {
        if (controller.lives != lastLives)
        {
            UpdateHearts();
            lastLives = controller.lives;
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].SetActive(i < controller.lives);
        }
    }
}
