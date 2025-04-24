using UnityEngine;

public class GrootController : EnemyBase
{
    [SerializeField] private GameObject killPoint;
    [SerializeField] private Vector2 deathForce = new Vector2(0f, 0f);

    private void KillEnemy() => Destroy(gameObject);

    protected override void Update()
    {
        Move();
        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y > killPoint.transform.position.y)
            {
                Vector2 randomDeathForce = new Vector2(deathForce.x * (Random.Range(0, 2) * 2 - 1), deathForce.y);
                enemyBody.velocity = randomDeathForce;
                Invoke(nameof(KillEnemy), 0.5f);
            }

            else moveSpeed = -moveSpeed;
        }
    }
}