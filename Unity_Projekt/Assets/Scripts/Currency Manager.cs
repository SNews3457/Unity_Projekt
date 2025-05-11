using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CurrencyManager : MonoBehaviour
{
    public CharacterController2D CharacterController;
    public Text amount;
    public float Orbs = 0;

    void Update()
    {
        amount.text = Orbs.ToString();
    }
}
