using UnityEngine;

public class ZombieController : EnemyBase
{
    [SerializeField] private float chaseRange;
    private bool canChase = false;
    private Animator zombieAnimation;
    private SpriteRenderer spriteRenderer;
    private Collider2D zombieCollider;

    protected override void Awake()
    {
        base.Awake();
        zombieAnimation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        zombieCollider = GetComponent<Collider2D>();

        ToggleZombie(false);
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
            enemyBody.velocity = new Vector2(0f, enemyBody.velocity.y);
            zombieAnimation.SetBool("isWalking", false);
        }

        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zombieAnimation.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zombieAnimation.SetBool("isAttacking", true);
        }
    }


    private void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        enemyBody.velocity = new Vector2(direction * Mathf.Abs(moveSpeed), enemyBody.velocity.y);
    }

    private void ToggleZombie(bool isActive)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = isActive;
        if (zombieCollider != null) zombieCollider.enabled = isActive;
        if (zombieAnimation != null) zombieAnimation.enabled = isActive;
        if (enemyBody != null) enemyBody.simulated = isActive;
    }
}
