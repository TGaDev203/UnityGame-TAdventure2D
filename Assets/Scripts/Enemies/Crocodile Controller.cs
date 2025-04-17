using UnityEngine;

public class CrocodileController : EnemyBase
{
    private bool canChase = false;
    [SerializeField] private float chaseRange;
    // [SerializeField] private Transform player;

    // private SpriteRenderer spriteRenderer;
    private Animator crocodileAnimation;


    protected override void Awake()
    {
        base.Awake();
        crocodileAnimation = GetComponent<Animator>();
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
            crocodileAnimation.SetBool("isEmerging", true);
            ToggleZombie(true);
            ChasePlayer();
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
            // zombieAnimation.SetBool("isAttacking", true);
            StopChasing();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // zombieAnimation.SetBool("isAttacking", true);
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