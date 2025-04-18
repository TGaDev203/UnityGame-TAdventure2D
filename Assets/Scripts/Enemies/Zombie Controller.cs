using UnityEngine;

public class ZombieController : EnemyBase
{
    [SerializeField] private float chaseRange;
    private Animator zombieAnimation;
    private bool canChase = false;

    protected override void Awake()
    {
        base.Awake();
        zombieAnimation = GetComponent<Animator>();
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
            zombieAnimation.SetBool("isAppearing", true);
            zombieAnimation.SetBool("isWalking", true);
            ToggleZombie(true);
            ChasePlayer();
        }
        else
        {
            StopChasing();
            zombieAnimation.SetBool("isAttacking", false);
            zombieAnimation.SetBool("isWalking", false);
        }

        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlayZombieSound();
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
}