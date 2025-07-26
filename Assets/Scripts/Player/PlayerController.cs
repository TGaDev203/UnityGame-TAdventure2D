using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float fallDamageCooldown;
    [SerializeField] private float fallThreshold;
    [SerializeField] private float hitDamage;
    [SerializeField] private float hitDamageCooldown;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private int fallDamage;
    [SerializeField] private LayerMask _damageLayers;
    [SerializeField] private Vector2 deathForce = new Vector2(0f, 0f);
    private bool hasJustLanded = false;
    private bool isDead = false;
    private float lastFallDamageTime;
    private float lastDamageTime;
    private float lastGroundY;
    private CapsuleCollider2D playerCollider;
    private HealthBarManager healthBar;
    private PlayerAnimation anim;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerBody;

    public float GetHealth() => currentHealth;
    public float GetCurrentHealth() => currentHealth;
    private bool IsTouchingEnemy() => playerCollider.IsTouchingLayers(_damageLayers);
    public bool IsDead() => isDead;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
        healthBar = GetComponent<HealthBarManager>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (SaveManager.SaveExists())
        {
            PlayerData data = SaveManager.LoadPlayerData();
            transform.position = new Vector2(data.positionX, data.positionY);
            currentHealth = data.currentHealth;
            CoinManager.Instance.SetCoin(data.coin);
        }
        else
        {
            currentHealth = maxHealth;
            CoinManager.Instance.SetCoin(0);
        }

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        lastGroundY = transform.position.y;
    }

    private void Update()
    {
        TouchingEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Platform")) return;
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) return;

        if (hasJustLanded) hasJustLanded = false;

        float fallDistance = lastGroundY - transform.position.y;
        bool isTooFast = fallDistance > fallThreshold;
        bool cooldownOver = Time.time - lastFallDamageTime > fallDamageCooldown;

        if (isTooFast && cooldownOver)
        {
            TakeDamage(fallDamage);
            lastFallDamageTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            lastGroundY = transform.position.y;
            hasJustLanded = true;
        }
    }

    private void TouchingEnemy()
    {
        if (IsTouchingEnemy() && Time.time - lastDamageTime > hitDamageCooldown)
        {
            TakeDamage(hitDamage);
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        SoundManager.Instance.PlayerHitSound();

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        SoundManager.Instance.PlayDeathSound();
        if (anim != null) anim.PlayerDeathAnimation();

        ApplyRandomDeathForce();
        playerMovement.DisableInput();

        Vector2 respawnPosition = new Vector2(0f, 0f);

        Invoke(nameof(RespawnAfterDelay), 2f);
    }

    private void RespawnAfterDelay()
    {
        SceneManager.LoadScene("Gameplay_Scene");
    }

    public void ApplyRandomDeathForce()
    {
        Vector2 randomDeathForce = new Vector2(deathForce.x * (Random.Range(0, 2) * 2 - 1), deathForce.y);
        playerBody.velocity = randomDeathForce;
    }

    public void SavePlayerData()
    {
        int coinAmount = CoinManager.Instance.GetCoin();
        SaveManager.SavePlayerData(transform.position.x, transform.position.y, currentHealth, coinAmount);
    }

    public void RespawnPlayer(Vector2 respawnPosition)
    {
        currentHealth = maxHealth;
        transform.position = respawnPosition;

        SaveManager.ResetCollectedItems();

        ReloadPickups();

        SavePlayerData();
    }

    private void ReloadPickups()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        foreach (var coin in coins)
        {
            if (!SaveManager.IsCoinCollected(coin.coinID))
            {
                coin.gameObject.SetActive(true);
            }
        }

        HealthPickup[] healthPickups = FindObjectsOfType<HealthPickup>();
        foreach (var healthPickup in healthPickups)
        {
            if (!SaveManager.IsHealthCollected(healthPickup.healthID))
            {
                healthPickup.gameObject.SetActive(true);
            }
        }
    }
}