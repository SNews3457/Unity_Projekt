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
        float verticalOffset = 0.5f; 

        if (player == null) return origin;

        float playerY = player.transform.position.y;

        if (origin.y > playerY)
        {
            origin.y -= verticalOffset;
        }


        if (origin.y < playerY)
        {
            origin.y += verticalOffset;
        }
        return origin;
    }





#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
#endif
}
