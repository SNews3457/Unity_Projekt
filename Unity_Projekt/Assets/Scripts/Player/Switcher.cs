// op dagobert
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class Switcher : MonoBehaviour
{
    public RectTransform levelUpPanel;
    public RectTransform achievementPanel;
    private bool showingLevelUp = true;

    public float shiftDistance = 50f;
    public float animTime = 0.3f;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            ToggleOverlays();
        }
    }

    public void ToggleOverlays()
    {
        if (showingLevelUp)
        {
            levelUpPanel.SetAsFirstSibling();
            achievementPanel.SetAsLastSibling();

            StartCoroutine(SmoothMove(levelUpPanel, shiftDistance));
            StartCoroutine(SmoothMove(achievementPanel, -shiftDistance));
        }
        else
        {
            achievementPanel.SetAsFirstSibling();
            levelUpPanel.SetAsLastSibling();

            StartCoroutine(SmoothMove(achievementPanel, shiftDistance));
            StartCoroutine(SmoothMove(levelUpPanel, -shiftDistance));
        }

        showingLevelUp = !showingLevelUp;
    }

    IEnumerator SmoothMove(RectTransform panel, float offsetY)
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 end = start + new Vector2(0, offsetY);
        float elapsed = 0;

        while (elapsed < animTime)
        {
            panel.anchoredPosition = Vector2.Lerp(start, end, elapsed / animTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = end;
    }
}

