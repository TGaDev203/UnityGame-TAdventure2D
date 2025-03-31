using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] LayerMask _layerPlayerRunAnimation;
    [SerializeField] LayerMask _layerPlayerClimbAnimation;
    [SerializeField] LayerMask _layerPlayerDieAnimation;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D playerCollider;
    private BoxCollider2D feetCollider;
    private Animator playerAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipSprite();
        PlayerRunAnimation();
        PlayerClimbAnination();    }

    private void FlipSprite()
    {
        bool isMovingLeft = rigidBody.velocity.x < 0;
        bool isMovingRight = rigidBody.velocity.x > 0;
        if (isMovingLeft)
        {
            spriteRenderer.flipX = true;
        }
        else if (isMovingRight)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void PlayerRunAnimation()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if (feetCollider.IsTouchingLayers(_layerPlayerRunAnimation) && playerHasHorizontalSpeed)
        {
            playerAnimation.SetBool("isRunning", true);
        }
        else
        {
            playerAnimation.SetBool("isRunning", false);
        }
    }

    private void PlayerClimbAnination()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
        if (playerCollider.IsTouchingLayers(_layerPlayerClimbAnimation) && playerHasVerticalSpeed)
        {
            playerAnimation.SetBool("isClimbing", true);
        }
        else
        {
            playerAnimation.SetBool("isClimbing", false);
        }
    }

    public void PlayerDeathAnimation()
    {
        if (playerCollider.IsTouchingLayers(_layerPlayerDieAnimation))
        {
            playerAnimation.SetTrigger("Dying");
        }
    }

    public void ResetAnimation()
    {
        playerAnimation.SetTrigger("Reset");
    }
}
