using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(Tilemap), typeof(TilemapCollider2D))]
public class DisappearTilemapWave : MonoBehaviour
{
    public float delayBeforeDisappear = 0.2f;    
    public float delayBetweenTiles = 0.05f;      
    public float respawnDelay = 3f;               
    public int range = 2;                         
    public string playerTag = "Player";

    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            Vector3 worldPos = collision.GetContact(0).point;
            Vector3Int center = tilemap.WorldToCell(worldPos);
            StartCoroutine(WaveDisappear(center));
        }
    }

    private IEnumerator WaveDisappear(Vector3Int center)
    {
        yield return new WaitForSeconds(delayBeforeDisappear);


        TileBase[,] savedTiles = new TileBase[range * 2 + 1, range * 2 + 1];

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int pos = new Vector3Int(center.x + x, center.y + y, center.z);
                savedTiles[x + range, y + range] = tilemap.GetTile(pos);
            }
        }

        for (int dist = 0; dist <= range; dist++)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) == dist)
                    {
                        Vector3Int pos = new Vector3Int(center.x + x, center.y + y, center.z);
                        if (tilemap.GetTile(pos) != null)
                        {
                            tilemap.SetTile(pos, null);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(delayBetweenTiles);
        }

        yield return new WaitForSeconds(respawnDelay);

        for (int dist = 0; dist <= range; dist++)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) == dist)
                    {
                        Vector3Int pos = new Vector3Int(center.x + x, center.y + y, center.z);
                        TileBase tile = savedTiles[x + range, y + range];
                        if (tile != null)
                        {
                            tilemap.SetTile(pos, tile);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(delayBetweenTiles);
        }
    }
}
