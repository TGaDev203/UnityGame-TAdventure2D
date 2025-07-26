using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    [SerializeField] private int healthValue;
    public string healthID;
    private bool isCollected = false;
    private CircleCollider2D _collider;
    private PlayerController playerController;

    private void DestroyHealth() => Destroy(gameObject);
    private void ScheduleDestroy() => Invoke(nameof(DestroyHealth), destroyDelay);

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
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

        if (playerController != null)
        {
            SaveManager.SavePlayerData(
                playerController.transform.position.x,
                playerController.transform.position.y,
                playerController.GetCurrentHealth(),
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
        float currentHealth = playerController.GetCurrentHealth();
        currentHealth += healthValue;
        playerController.GetComponent<HealthBarManager>().SetHealth(currentHealth);
        FindObjectOfType<HealthBarManager>().SetHealth(currentHealth);
    }
}
