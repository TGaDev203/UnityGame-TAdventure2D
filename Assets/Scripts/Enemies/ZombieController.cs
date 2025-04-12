using UnityEngine;

public class ZombieController : EnemyBase
{
    [SerializeField] private float chaseRange;
    private bool canChase = false;
    private Animator zombieAnimation;

    protected override void Awake()
    {
        base.Awake();
        zombieAnimation = GetComponent<Animator>();
    }

    protected override void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        canChase = distanceToPlayer < chaseRange;

        if (canChase)
        {
            // Invoke(nameof(ChasePlayer), 1f);
            ChasePlayer();
        }

        else
        {
            // Invoke(nameof(Move), 1f);
            Move();
            ZombieWalkingAnimation();
        }

        FlipSprite();
    }
    private void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        enemyBody.velocity = new Vector2(direction * Mathf.Abs(moveSpeed), enemyBody.velocity.y);
    }

    private void ZombieWalkingAnimation()
    {
        zombieAnimation.SetBool("isWalking", true);
    }
}
