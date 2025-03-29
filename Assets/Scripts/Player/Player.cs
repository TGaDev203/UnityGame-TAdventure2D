using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerTakenDamage;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float damageCooldown;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private PauseButtonManager pauseButtonManager;
    private Vector2 initialPosition;
    private CapsuleCollider2D playerCollider;
    private HealthBarManager healthBar;
    private float lastDamageTime;
    private Rigidbody2D playerBody;
    private bool isDead = false;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        healthBar = GetComponent<HealthBarManager>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialPosition = transform.position;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        TouchingEnemy();
    }

    private void TouchingEnemy()
    {
        if (IsTouchingEnemy() && Time.time - lastDamageTime > damageCooldown)
        {
            TakeDamage(3);
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
            OnDeath();
            Invoke("Replay", 1.2f);
        }
    }

    private void OnDeath()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Die();
    }

    private bool IsTouchingEnemy()
    {
        return playerCollider.IsTouchingLayers(_layerTakenDamage);
    }

    public void Replay()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        optionButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(true);
    }

    public void ResetPlayer()
    {
        transform.position = initialPosition;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        playerBody.velocity = Vector2.zero;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}