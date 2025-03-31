using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingTile : MonoBehaviour
{
    private Tilemap tilemap;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector3Int tilePosition = tilemap.WorldToCell(collision.transform.position);
            
            if (tilemap.HasTile(tilePosition))
            {
                StartCoroutine(DestroyTile(tilePosition));
            }
        }
    }

    IEnumerator DestroyTile(Vector3Int tilePosition)
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        tilemap.SetTile(tilePosition, null);
    }
}
