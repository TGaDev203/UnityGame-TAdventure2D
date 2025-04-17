using UnityEngine;

public class ZombieController : EnemyBase
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange;
    private Animator zombieAnimation;
    private SpriteRenderer spriteRenderer;
    private bool canChase = false;

    protected override void Awake()
    {
        base.Awake();
        zombieAnimation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ToggleZombie(false);
    }

    private void Start()
    {
        initialSpeed = moveSpeed;
    }

    protected override void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        canChase = distanceToPlayer < chaseRange;

        if (canChase)
        {
            ToggleZombie(true);
            ChasePlayer();
            zombieAnimation.SetBool("isWalking", true);
        }
        else
        {
            StopChasing();
        }

        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zombieAnimation.SetBool("isAttacking", true);
            StopChasing();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zombieAnimation.SetBool("isAttacking", true);
            moveSpeed = 0f;
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveSpeed = initialSpeed;
        }
    }

    private void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        enemyBody.velocity = new Vector2(direction * Mathf.Abs(moveSpeed), enemyBody.velocity.y);
    }

    private void StopChasing()
    {
        enemyBody.velocity = new Vector2(0f, enemyBody.velocity.y);
        zombieAnimation.SetBool("isWalking", false);
        zombieAnimation.SetBool("isAttacking", false);
    }

    private void ToggleZombie(bool isActive)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = isActive;
        if (enemyBody != null) enemyBody.simulated = isActive;
    }
}
