using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform player;
    protected float initialSpeed;
    protected Rigidbody2D enemyBody;
    private SpriteRenderer spriteRenderer;

    protected abstract void Update();

    protected virtual void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            moveSpeed = -moveSpeed;
        }
    }

    protected virtual void Move()
    {
        enemyBody.velocity = new Vector2(moveSpeed, enemyBody.velocity.y);
    }

    protected void FlipSprite()
    {
        float moveDirection = enemyBody.velocity.x;

        if (moveDirection != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveDirection), 1f);
        }
    }

    protected void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        enemyBody.velocity = new Vector2(direction * Mathf.Abs(moveSpeed), enemyBody.velocity.y);
    }

    protected void StopChasing()
    {
        enemyBody.velocity = new Vector2(0f, enemyBody.velocity.y);
    }

    protected void ToggleZombie(bool isActive)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = isActive;
        if (enemyBody != null) enemyBody.simulated = isActive;
    }

    protected void FlipZombie()
    {
        float xDistance = player.position.x - transform.position.x;

        if (Mathf.Abs(xDistance) > 0.2f)
        {
            spriteRenderer.flipX = xDistance > 0f;
        }
    }
}