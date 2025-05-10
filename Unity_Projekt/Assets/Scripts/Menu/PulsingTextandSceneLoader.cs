using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PulsingTextAndSceneLoader : MonoBehaviour
{
    [Header("Text Animation")]
    public Text pulsingText;
    public float pulseSpeed = 1f;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;

    [Header("Szene")]
    public string menuSceneName = "MainMenu"; // Name deiner Menü-Szene

    private float timer;

    void Update()
    {
        AnimateText();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    void AnimateText()
    {
        if (pulsingText == null) return;

        timer += Time.deltaTime * pulseSpeed;
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(timer) + 1f) / 2f);
        pulsingText.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
