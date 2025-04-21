using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinID;
    private CircleCollider2D _collider;
    private bool isCollected = false;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();

        if (string.IsNullOrEmpty(coinID))
        {
            coinID = $"{transform.position.x}_{transform.position.y}";
        }

        if (SaveManager.IsCoinCollected(coinID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        isCollected = true;

        Debug.Log($"[DEBUG] Coin {coinID} collected by player");

        if (_collider != null) _collider.enabled = false;

        SaveManager.MarkCoinCollected(coinID);

        Destroy(gameObject, 0.1f);
    }
}