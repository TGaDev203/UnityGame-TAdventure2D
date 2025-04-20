using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _damageLayers;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int hitDamage;
    [SerializeField] private int fallDamage;
    [SerializeField] private float fallThreshold;
    [SerializeField] private float hitDamageCooldown;
    [SerializeField] private float fallDamageCooldown;
    [SerializeField] private Vector2 deathForce = new Vector2(0f, 0f);
    [SerializeField] private GameObject pauseMenu;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerBody;
    private HealthBarManager healthBar;
    private ButtonManagerBase buttonManagerBase;

    private float lastGroundY;
    private float lastDamageTime;
    private float lastFallDamageTime;
    private bool isDead = false;
    private bool hasJustLanded = false;

    public int GetHealth() => currentHealth;

    public bool IsDead() => isDead;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBarManager>();
        buttonManagerBase = FindObjectOfType<ButtonManagerBase>();
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

        if (hasJustLanded)
        {
            hasJustLanded = false;
        }

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

    public void TakeDamage(int damage)
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

    private bool IsTouchingEnemy()
    {
        return playerCollider.IsTouchingLayers(_damageLayers);
    }

    public void ReplayOn()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Die()
    {
        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null) anim.PlayerDeathAnimation();

        ApplyRandomDeathForce();
        SoundManager.Instance.PlayerHitSound();

        GetComponent<PlayerMovement>().DisableInput();
        buttonManagerBase.SetMouseOn();
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
}