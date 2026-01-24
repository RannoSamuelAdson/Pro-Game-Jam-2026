using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<float, bool> OnMoveInput;
    public static event Action OnPauseInput; // bool is if paused or not, true is paused
    public static event Action<bool> OnJumpInput; // true - pressed, false - released
    private bool isPaused = false;
    private bool inputEnabled = false;
    InputAction movement;
    InputAction pause;
    InputAction jump;
    private void Awake()
    {
        movement = InputSystem.actions.FindAction("Movement");
        jump = InputSystem.actions.FindAction("Jump");
        pause = InputSystem.actions.FindAction("Pause");
    }

    private void OnEnable()
    {
        movement.started += OnMovement;
        movement.canceled += OnMovement;
        pause.performed += OnPause;
        jump.started += OnJump;
        jump.canceled += OnJump;
        movement.Enable();
        pause.Enable();
        jump.Enable();
        PauseMenu.OnPauseGame += SetPaused;
        GameController.ChangeGameState += ChangeInputByState;
    }

    private void OnDisable()
    {
        movement.performed -= OnMovement;
        pause.performed -= OnPause;
        jump.performed += OnJump;
        jump.canceled += OnJump;
        jump.Disable();
        movement.Disable();
        pause.Disable();
        PauseMenu.OnPauseGame -= SetPaused;
        GameController.ChangeGameState -= ChangeInputByState;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isPaused || !inputEnabled) return;

        if (context.started)
        {
            OnJumpInput?.Invoke(true);
        }

        else if (context.canceled)
        {
            OnJumpInput?.Invoke(false);
        }

    }

    private void ChangeInputByState(GameState state)
    {
        switch (state)
        {
            case GameState.Death:
            case GameState.Inactive:
                inputEnabled = false;
                break;
            case GameState.Active:
                inputEnabled = true;
                break;
            default:
                Debug.LogWarning($"Invalid state {inputEnabled}!");
                break;
        }
    }

    private void SetPaused(bool val)
    {
        isPaused = val;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;
        OnPauseInput?.Invoke();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (isPaused || !inputEnabled) return;
        var moveDir = context.ReadValue<Vector2>();
        if (context.started)
        {
            OnMoveInput?.Invoke(moveDir.x, true);
        }
        else if (context.canceled)
        {
            OnMoveInput?.Invoke(moveDir.x, false);
        }

    }
}
