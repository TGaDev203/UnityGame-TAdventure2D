using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator zombieAnimation;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        zombieAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        float moveDirection = rigidBody.velocity.x;

        if (moveDirection != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveDirection), 1f);
        }
    }

    public void ZombieWalkingAnimation()
    {
        zombieAnimation.SetBool("isWalking", true);
    }
}