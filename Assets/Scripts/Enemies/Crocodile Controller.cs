using UnityEngine;

public class CrocodileController : EnemyBase
{
    private Animator crocodileAnimation;

    protected override void Awake()
    {
        base.Awake();
        crocodileAnimation = GetComponent<Animator>();
    }

    protected override void Update()
    {
        crocodileAnimation.SetBool("isEmerging", true);
        Move();
        FlipSprite();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlayCrocodileSound();

            crocodileAnimation.SetBool("isAttacking", true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;

        if (collision.CompareTag("Player"))
        {
            crocodileAnimation.SetBool("isAttacking", false);

        }
    }

}