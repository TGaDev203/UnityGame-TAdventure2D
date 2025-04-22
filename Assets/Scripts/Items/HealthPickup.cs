using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyHealing(healthValue);
            Destroy(gameObject);
        }
    }

    private void ApplyHealing(int healthValue)
    {
        int playerHealth = FindObjectOfType<Player>().GetCurrentHealth();
        playerHealth += healthValue;
        FindObjectOfType<HealthBarManager>().SetHealth(playerHealth);
    }
}
