using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class DisappearOnTouch : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Tag des Spielers")]
    public string playerTag = "Player";

    [Tooltip("Zeit nach Berührung, bevor Plattform verschwindet (Sekunden)")]
    public float delayBeforeDisappear = 0.5f;

    [Tooltip("Zeit, bis Plattform wieder erscheint (Sekunden)")]
    public float respawnDelay = 3f;

    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private bool isDisappearing = false;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDisappearing && collision.collider.CompareTag(playerTag))
        {
            StartCoroutine(DisappearAndRespawn());
        }
    }

    private IEnumerator DisappearAndRespawn()
    {
        isDisappearing = true;

        // Warten vor dem Verschwinden
        yield return new WaitForSeconds(delayBeforeDisappear);

        // Plattform "verschwinden" lassen
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (platformCollider != null)
            platformCollider.enabled = false;

        // Warten bis Wiedererscheinen
        yield return new WaitForSeconds(respawnDelay);

        // Plattform wiederherstellen
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (platformCollider != null)
            platformCollider.enabled = true;

        isDisappearing = false;
    }
}
