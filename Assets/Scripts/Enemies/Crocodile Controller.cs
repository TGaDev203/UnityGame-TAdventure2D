public class CrocodileController : EnemyBase
{
    // [SerializeField] private float chaseRange;
    // private Animator crocodileAnimation;
    // private bool canChase = false;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     crocodileAnimation = GetComponent<Animator>();
    //     ToggleZombie(false);
    // }

    // private void Start()
    // {
    //     initialSpeed = moveSpeed;
    // }

    // protected override void Update()
    // {
    //     float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    //     canChase = distanceToPlayer < chaseRange;

    //     if (canChase)
    //     {
    //         crocodileAnimation.SetBool("isEmerging", true);
    //         ToggleZombie(true);
    //         ChasePlayer();
    //     }
    //     else
    //     {
    //         StopChasing();
    //     }

    //     // Move();
    //     // FlipSprite();
    // }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         crocodileAnimation.SetBool("isAttacking", true);
    //         StopChasing();
    //     }
    // }

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         moveSpeed = 0f;
    //     }
    // }

    // protected override void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         moveSpeed = initialSpeed;
    //     }
    // }

    protected override void Update()
    {
        Move();
        FlipSprite();
    }
}
