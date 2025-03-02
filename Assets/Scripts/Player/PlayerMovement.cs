using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask jumpableLayers;
    [SerializeField] LayerMask dealthLayers;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] LayerMask bouncingLayer;
    [SerializeField] private Vector2 deathKick = new Vector2(0f, 0f);
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float bounceForce;
    [SerializeField] private float waterDrag;
    private Rigidbody2D playerBody;
    private CapsuleCollider2D playerCollider;
    private BoxCollider2D feetCollider;
    public TilemapCollider2D ladderCollider;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        ladderCollider = GameObject.FindWithTag("Ladder").GetComponent<TilemapCollider2D>();
        feetCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start() => InputManager.Instance.OnJump += Jump;

    private void Update()
    {
        Move();
        CheckBouncing();
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
        if (!playerCollider.IsTouchingLayers(bouncingLayer)) return;

        SoundManager.Instance.PlayBouncingSound();
        playerBody.velocity = new Vector2(playerBody.velocity.x, bounceForce);
    }

    private bool CanJump() => feetCollider.IsTouchingLayers(jumpableLayers);

    private void HandleLadderCollision()
    {
        bool shouldIgnoreLadder = !playerCollider.IsTouchingLayers(platformLayer) || InputManager.Instance.IsJumping();
        Physics2D.IgnoreCollision(playerCollider, ladderCollider, shouldIgnoreLadder);
        ladderCollider.enabled = !shouldIgnoreLadder;

        if (shouldIgnoreLadder) Invoke(nameof(ResetLadderCollision), 1f);
    }

    private void ResetLadderCollision()
    {
        Physics2D.IgnoreCollision(playerCollider, ladderCollider, false);
        ladderCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) SetWaterState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water")) SetWaterState(false);
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

    public void Die()
    {
        if (!playerCollider.IsTouchingLayers(dealthLayers)) return;

        GetComponent<PlayerAnimation>()?.PlayerDeathAnimation();
        playerBody.velocity = deathKick;
        SoundManager.Instance.PlayerHitSound();
    }

    public float GetJumpForce()
    {
        return this.jumpForce;

    }
}