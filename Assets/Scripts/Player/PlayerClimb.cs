using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private float climbSpeed;
    private CapsuleCollider2D playerCollider;
    private PlayerMovement playerMovement;
    private Rigidbody2D rigidBody;

    private bool IsPlayerOnTopLadderPoint() => playerCollider.IsTouchingLayers(LayerMask.GetMask("TopLadder"));
    private bool IsPlayerOnClimbableLayer() => playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Climb();
    }

    private void Climb()
    {
        if (IsPlayerOnClimbableLayer())
        {
            ApplyClimbSpeed();
            PerformJumpFromLadder();
        }

        else rigidBody.gravityScale = 5f;
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
}