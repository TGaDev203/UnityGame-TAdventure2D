using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private Vector2 deathForce = new Vector2(0f, 0f);
    [SerializeField] private LayerMask _damageLayers;
    [SerializeField] private float fallDamageCooldown;
    [SerializeField] private int fallDamage;
    [SerializeField] private float fallThreshold;
    [SerializeField] private float hitDamage;
    [SerializeField] private float hitDamageCooldown;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject pauseMenu;
    private HealthBarManager healthBar;
    private bool hasJustLanded = false;
    private bool isDead = false;
    private float lastFallDamageTime;
    private float lastDamageTime;
    private float lastGroundY;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerBody;

    public float GetHealth() => currentHealth;
    public float GetCurrentHealth() => currentHealth;
    private bool IsTouchingEnemy() => playerCollider.IsTouchingLayers(_damageLayers);
    public bool IsDead() => isDead;

    private void Awake()
    {
        healthBar = GetComponent<HealthBarManager>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
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
            Invoke(nameof(ReplayOn), 1.2f);
        }
    }

    public void Die()
    {
        SoundManager.Instance.PlayDeathSound();
        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null) anim.PlayerDeathAnimation();

        ApplyRandomDeathForce();
        GetComponent<PlayerMovement>().DisableInput();
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

    public void ReplayOn()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}