using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public CharacterController2D CharacterController;
    public Text amount;
    public float Orbs = 0;

    private const string ORBS_KEY = "PlayerOrbs"; // Schlüssel für PlayerPrefs

    // AudioSource für den Orb-Sammel-Sound
    public AudioSource orbCollectSound; 

    void Start()
    {
        LoadOrbs(); // Beim Start Orbs laden
    }

    void Update()
    {
        amount.text = Orbs.ToString();
    }

    public void AddOrbs(float amountToAdd)
    {
        Orbs += amountToAdd;
        SaveOrbs();

        // Spielt den Sound ab, wenn Orbs gesammelt werden
        if (orbCollectSound != null)
        {
            orbCollectSound.Play();
        }
    }

    public void SaveOrbs()
    {
        PlayerPrefs.SetFloat(ORBS_KEY, Orbs);
        PlayerPrefs.Save(); // Daten dauerhaft speichern
    }

    public void LoadOrbs()
    {
        if (PlayerPrefs.HasKey(ORBS_KEY))
        {
            Orbs = PlayerPrefs.GetFloat(ORBS_KEY);
        }
        else
        {
            Orbs = 0;
        }
    }

    public void ResetOrbs()
    {
        Orbs = 0;
        SaveOrbs();
    }
}
