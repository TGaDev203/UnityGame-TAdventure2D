using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button optionButton;
    [SerializeField] private Button replayButton;

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
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
            // lastGroundY = transform.position.y;
            // return;
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

    private void TakeDamage(int damage)
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

        optionButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(true);
    }

    public void Die()
    {
        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null) anim.PlayerDeathAnimation();

        ApplyRandomDeathForce();
        SoundManager.Instance.PlayerHitSound();

        GetComponent<PlayerMovement>().DisableInput();
        buttonManagerBase.ToggleButton(0);
        buttonManagerBase.ToggleButton(1);
        buttonManagerBase.SetMouseOn();
    }

    public void ApplyRandomDeathForce()
    {
        Vector2 randomDeathForce = new Vector2(deathForce.x * (Random.Range(0, 2) * 2 - 1), deathForce.y);
        playerBody.velocity = randomDeathForce;
    }
}
