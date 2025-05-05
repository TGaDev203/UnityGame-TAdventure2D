using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private LayerMask _damageLayers;
    private Animator playerAnimation;
    private BoxCollider2D feetCollider;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerBody;
    private SpriteRenderer spriteRenderer;

    public void PlayerDeathAnimation() => playerAnimation.SetTrigger("Dying");
    public void ResetAnimation() => playerAnimation.SetTrigger("Reset");

    private void Awake()
    {
        feetCollider = GetComponent<BoxCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FlipSprite();
        PlayerRunAnimation();
        PlayerClimbAnination();
    }

    private void FlipSprite()
    {
        if (playerBody.velocity.x < -0.05f)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerBody.velocity.x > 0.05f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void PlayerRunAnimation()
    {
        float runThreshold = 0.1f;
        bool playerHasHorizontalSpeed = Mathf.Abs(playerBody.velocity.x) > runThreshold;

        bool isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        bool isRunning = isGrounded && playerHasHorizontalSpeed;

        if (playerAnimation.GetBool("isRunning") != isRunning)
        {
            playerAnimation.SetBool("isRunning", isRunning);
        }
    }

    private void PlayerClimbAnination()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(playerBody.velocity.y) > Mathf.Epsilon;
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && playerHasVerticalSpeed)
        {
            playerAnimation.SetBool("isClimbing", true);
        }
        else
        {
            playerAnimation.SetBool("isClimbing", false);
        }
    }
}