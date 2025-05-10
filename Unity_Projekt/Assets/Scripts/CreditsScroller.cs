using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    [Header("Einstellungen")]
    public RectTransform creditsText;      // Der Text, der scrollt
    public float scrollSpeed = 50f;        // Geschwindigkeit in Einheiten pro Sekunde
    public float startY = 600f;            // Startposition Y (außerhalb des sichtbaren Bereichs)
    public float endY = -600f;             // Endposition Y (wenn komplett durchgelaufen)

    void Start()
    {
        if (creditsText != null)
        {
            Vector2 pos = creditsText.anchoredPosition;
            pos.y = startY;
            creditsText.anchoredPosition = pos;
        }
    }

    void Update()
    {
        if (creditsText == null) return;

        // Bewege den Text nach unten
        creditsText.anchoredPosition -= new Vector2(0, scrollSpeed * Time.deltaTime);

        // Wenn der Text unten ist, zurück zum Start
        if (creditsText.anchoredPosition.y <= endY)
        {
            Vector2 pos = creditsText.anchoredPosition;
            pos.y = startY;
            creditsText.anchoredPosition = pos;
        }
    }
}

