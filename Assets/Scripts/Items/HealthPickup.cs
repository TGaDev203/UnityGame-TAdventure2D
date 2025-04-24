using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlayHealthPickupSound();
            ApplyHealing(healthValue);
            Destroy(gameObject);
        }
    }

    private void ApplyHealing(int healthValue)
    {
        float playerHealth = FindObjectOfType<Player>().GetCurrentHealth();
        playerHealth += healthValue;
        FindObjectOfType<HealthBarManager>().SetHealth(playerHealth);
    }
}
