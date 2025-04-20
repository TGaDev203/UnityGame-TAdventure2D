using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private float climbSpeed;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D rigidBody;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Climb();
    }

    private void Climb()
    {
        if (isPlayerOnClimbableLayer())
        {
            ApplyClimbSpeed();
            PerformJumpFromLadder();
        }

        else rigidBody.gravityScale = 5f;
    }

    private bool isPlayerOnClimbableLayer()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    private void ApplyClimbSpeed()
    {
        Vector2 inputVectorClimb = InputManager.Instance.GetInputVectorClimb();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, inputVectorClimb.y * climbSpeed);
        rigidBody.gravityScale = 0f;

    }

    private void PerformJumpFromLadder()
    {
        if (IsPlayerOnTopLadderPoint() && InputManager.Instance.IsJumping())
        {
            rigidBody.gravityScale = 5f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerMovement.GetJumpForce());
        }
    }

    private bool IsPlayerOnTopLadderPoint()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("TopLadder"));
    }
}