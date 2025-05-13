using UnityEngine;
using System.Collections;

public class KillObject : MonoBehaviour
{
    [Tooltip("Abklingzeit in Sekunden, bevor der Spieler erneut durch diesen Spike sterben kann.")]
    public float cooldownTime = 5f;

    private bool canKill = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canKill) return;

        if (collision.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.GetComponent<CharacterController2D>();
            if (playerController != null)
            {
                playerController.life = 0;
                playerController.StartCoroutine("WaitToDead");
                StartCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Cooldown()
    {
        canKill = false;
        yield return new WaitForSeconds(cooldownTime);
        canKill = true;
    }
}
