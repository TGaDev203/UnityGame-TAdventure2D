using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //! Component
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
        InitializeComponents();
    }

    //! Initialization
    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    //! Handle All Animations
    private void HandleAnimation()
    {
        FlipSprite();
        PlayerRunAnimation();
        PlayerClimbAnination();
        PlayerDyingAnimation();
    }

    //! Flip Sprite
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

    //! Run Animation
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

    //! Climb Animation
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

    //! Dying Animation
    private void PlayerDyingAnimation ()
    {
        if (playerCollider.IsTouchingLayers(_layerPlayerDieAnimation))
        {
            playerAnimation.SetTrigger("Dying");
        }
    }
}
