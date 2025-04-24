using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    [SerializeField] private float jumpForce;
    [SerializeField] LayerMask _jumpableLayers;
    [SerializeField] private float runSpeed;
    [SerializeField] private float waterDrag;
    private BoxCollider2D feetCollider;
    public TilemapCollider2D ladderCollider;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerBody;

    private bool CanJump() => feetCollider != null && feetCollider.IsTouchingLayers(_jumpableLayers);
    public void DisableInput() => this.enabled = false;
    public void EnableInput() => this.enabled = true;
    public float GetJumpForce() => this.jumpForce;

    private void Awake()
    {
        feetCollider = gameObject.GetComponent<BoxCollider2D>();
        ladderCollider = GameObject.FindWithTag("Ladder").GetComponent<TilemapCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InputManager.Instance.OnJump += Jump;
    }

    private void Update()
    {
        Move();
        CheckBouncing();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            SetWaterState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            SetWaterState(false);
        }
    }

    private void Move()
    {
        float moveInput = InputManager.Instance.GetInputVectorMove().x;
        playerBody.velocity = new Vector2(moveInput * runSpeed, playerBody.velocity.y);
    }

    private void Jump(object sender, EventArgs e)
    {
        if (!CanJump()) return;

        playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
        HandleLadderCollision();
    }

    private void CheckBouncing()
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Mushroom"))) return;

        SoundManager.Instance.PlayBouncingSound();
        playerBody.velocity = new Vector2(playerBody.velocity.x, bounceForce);
    }


    private void HandleLadderCollision()
    {
        bool shouldIgnoreLadder = !playerCollider.IsTouchingLayers(LayerMask.GetMask("Platform")) || InputManager.Instance.IsJumping();
        Physics2D.IgnoreCollision(playerCollider, ladderCollider, shouldIgnoreLadder);
        ladderCollider.enabled = !shouldIgnoreLadder;

        if (shouldIgnoreLadder) Invoke(nameof(ResetLadderCollision), 1f);
    }

    private void ResetLadderCollision()
    {
        Physics2D.IgnoreCollision(playerCollider, ladderCollider, false);
        ladderCollider.enabled = true;
    }

    private void SetWaterState(bool isInWater)
    {
        if (isInWater)
        {
            SoundManager.Instance.PlayWaterSplashSound();
            SoundManager.Instance.PlayWaterWalkingSound();
            jumpForce = 40;
            playerBody.drag = waterDrag;
        }
        else
        {
            SoundManager.Instance.StopWaterWalkingSound();
            playerBody.drag = 0;
            playerBody.angularDrag = 0;
            jumpForce = 17;
        }
    }
}