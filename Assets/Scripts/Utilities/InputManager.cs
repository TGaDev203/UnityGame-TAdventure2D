using System;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI mouseOffText;
    private PlayerAction playerInputAction;
    public event EventHandler OnJump;
    public PlayerAction PlayerInputAction => playerInputAction;

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerAction();
    }

    private void Start()
    {
        mouseOffText.text = "Press Left Ctrl to hide the mouse cursor 😊\nGood luck!";
        Invoke(nameof(HideMouseOffText), 3f);
        playerInputAction.Player.Move.Enable();
        playerInputAction.Player.Climb.Enable();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Jump.Enable();
        playerInputAction.Player.Jump.performed += Jump;
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public bool IsJumping()
    {
        return playerInputAction.Player.Jump.IsPressed();
    }

    public Vector2 GetInputVectorMove()
    {
        Vector2 inputVectorMove = playerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVectorMove.normalized;
    }

    public Vector2 GetInputVectorClimb()
    {
        Vector2 inputVectorClimb = playerInputAction.Player.Climb.ReadValue<Vector2>();
        return inputVectorClimb.normalized;
    }

    private void HideMouseOffText()
    {
        mouseOffText.text = "";
    }
}