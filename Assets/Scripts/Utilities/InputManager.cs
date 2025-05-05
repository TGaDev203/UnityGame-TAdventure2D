using System;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI welcomeText;
    public event EventHandler OnJump;
    public PlayerAction PlayerInputAction => playerInputAction;
    private PlayerAction playerInputAction;

    public void HideWelcomeText() => welcomeText.text = string.Empty;
    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context) => OnJump?.Invoke(this, EventArgs.Empty);

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerAction();
    }

    private void Start()
    {
        ShowMouseInstruction();
        playerInputAction.Player.Move.Enable();
        playerInputAction.Player.Climb.Enable();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Jump.Enable();
        playerInputAction.Player.Jump.performed += Jump;
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

    private void ShowMouseInstruction()
    {
        welcomeText.text = "Collect the target coin to unlock the goal 😊\nGood luck!";
        Invoke(nameof(HideWelcomeText), 3f);
    }
}