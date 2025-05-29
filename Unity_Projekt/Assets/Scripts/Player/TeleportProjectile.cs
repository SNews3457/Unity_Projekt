using UnityEngine;

public class TeleportProjectile : MonoBehaviour
{
    private GameObject player;

    public LayerMask obstacleLayers; 

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) return;

        if (!collision.isTrigger && player != null)
        {
            Vector2 targetPos = FindSafePosition(transform.position);

            if (targetPos != Vector2.zero)
            {
                player.transform.position = targetPos;
            }

            Destroy(gameObject);
        }
    }

    private Vector2 FindSafePosition(Vector2 origin)
    {
        float checkRadius = 0.25f;
        float heightStep = 0.2f;
        int maxSteps = 20; 

        for (int i = 0; i < maxSteps; i++)
        {
            Vector2 checkPos = origin + Vector2.up * (i * heightStep);
            Collider2D hit = Physics2D.OverlapCircle(checkPos, checkRadius, obstacleLayers);

            if (hit == null)
            {
                return checkPos;
            }
        }

        Debug.LogWarning("Kein sicherer Ort gefunden");
        return Vector2.zero;
    }
}
