using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckpointLight : MonoBehaviour
{
    public Light2D[] lights;
    public SpriteRenderer checkpointRenderer;
    public Sprite activeSprite;

    void Start()
    {
        checkpointRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = true;
            }
        }
    }

    void Update()
    {
        if (checkpointRenderer.sprite != activeSprite)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = false;
            }
        }
    }
}
