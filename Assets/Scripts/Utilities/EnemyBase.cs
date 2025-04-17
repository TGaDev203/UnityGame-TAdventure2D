using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    protected float initialSpeed;
    protected Rigidbody2D enemyBody;

    protected virtual void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Move()
    {
        enemyBody.velocity = new Vector2(moveSpeed, enemyBody.velocity.y);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
    }

    protected void FlipSprite()
    {
        float moveDirection = enemyBody.velocity.x;

        if (moveDirection != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveDirection), 1f);
        }
    }

    protected abstract void Update();
}