using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2, bool> OnMoveInput;
    public static event Action OnPauseInput; // bool is if paused or not, true is paused
    public static event Action<bool> OnJumpInput; // true - pressed, false - released
    private bool isPaused = false;
    private bool inputEnabled = false;
    InputAction movement;
    InputAction pause;
    private void Awake()
    {
        movement = InputSystem.actions.FindAction("Movement");
        pause = InputSystem.actions.FindAction("Pause");
        Debug.Log(movement);
    }

    private void OnEnable()
    {
        movement.canceled += OnMovement;
        movement.performed += OnMovement;
        pause.performed += OnPause;
        movement.Enable();
        pause.Enable();
        PauseMenu.OnPauseGame += SetPaused;
        GameController.ChangeGameState += ChangeInputByState;
    }

    private void OnDisable()
    {
        movement.performed -= OnMovement;
        movement.canceled -= OnMovement;
        pause.performed -= OnPause;
        movement.Disable();
        pause.Disable();
        PauseMenu.OnPauseGame -= SetPaused;
        GameController.ChangeGameState -= ChangeInputByState;
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
        Debug.Log("hi");
        if (isPaused || !inputEnabled) return;
        var moveDir = context.ReadValue<Vector2>();
        if (context.started || context.performed)
        {
            OnMoveInput?.Invoke(moveDir, true);
        }
        else if (context.canceled)
        {
            OnMoveInput?.Invoke(moveDir, false);
        }

    }
}
