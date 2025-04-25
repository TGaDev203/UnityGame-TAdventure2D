using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    [SerializeField] private int healthValue;
    public string healthID;

    private CircleCollider2D _collider;
    private bool isCollected = false;

    private void DestroyHealth() => Destroy(gameObject);
    private void ScheduleDestroy() => Invoke(nameof(DestroyHealth), destroyDelay);

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();

        GenerateHealthIDIfNeeded();

        if (SaveManager.IsHealthCollected(healthID))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void GenerateHealthIDIfNeeded()
    {
        if (string.IsNullOrEmpty(healthID))
        {
            healthID = $"{transform.position.x}_{transform.position.y}";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || !other.CompareTag("Player")) return;

        isCollected = true;

        if (_collider != null) _collider.enabled = false;

        SaveManager.MarkHealthCollected(healthID);

        HandleHealthPickup();

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            SaveManager.SavePlayerData(
                player.transform.position.x,
                player.transform.position.y,
                player.GetCurrentHealth(),
                CoinManager.Instance.GetCoin()
            );
        }
    }

    private void HandleHealthPickup()
    {
        ApplyHealing();
        SoundManager.Instance.PlayHealthPickupSound();
        ScheduleDestroy();
    }

    private void ApplyHealing()
    {
        Player player = FindObjectOfType<Player>();
        float currentHealth = player.GetCurrentHealth();
        currentHealth += healthValue;
        player.GetComponent<HealthBarManager>().SetHealth(currentHealth);
        FindObjectOfType<HealthBarManager>().SetHealth(currentHealth);
    }
}
