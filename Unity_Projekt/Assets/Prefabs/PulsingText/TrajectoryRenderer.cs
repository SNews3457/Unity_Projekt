using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotCount = 20;
    public float dotSpacing = 0.1f;
    public float launchForce = 10f;

    private List<Transform> dots = new List<Transform>();

    void Start()
    {
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot.transform);
        }
    }

    public void ShowTrajectory(Vector2 startPos, Vector2 direction, float force, float gravityScale = 1f)
    {
        HideTrajectory();

        float gravity = Physics2D.gravity.y * gravityScale;
        Vector2 velocity = direction.normalized * force;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * dotSpacing;
            Vector2 pos = startPos + velocity * t + 0.5f * new Vector2(0, gravity) * t * t;
            dots[i] = Instantiate(dotPrefab, pos, Quaternion.identity, transform).transform;
        }
    }


    public void HideTrajectory()
    {
        foreach (var dot in dots)
        {
            dot.gameObject.SetActive(false);
        }
    }
}
