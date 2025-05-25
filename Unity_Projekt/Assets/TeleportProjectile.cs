using UnityEngine;

public class TeleportProjectile : MonoBehaviour
{
    private GameObject player;

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) return;

        if (!collision.isTrigger && player != null)
        {
            player.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
