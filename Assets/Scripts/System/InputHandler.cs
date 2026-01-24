using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2, bool> OnMoveInput;
    public static event Action OnPauseInput;
    public static event Action OnPuzzleBack;
    public static event Action OnInteractInput; 
    private bool isPaused = false;
    private bool inputEnabled = false;
    private bool puzzleMode = false;
    InputAction movement;
    InputAction pause;
    InputAction interact;
    private void Awake()
    {
        movement = InputSystem.actions.FindAction("Movement");
        pause = InputSystem.actions.FindAction("Pause");
        interact = InputSystem.actions.FindAction("Interact");
    }

    private void OnEnable()
    {
        movement.canceled += OnMovement;
        movement.performed += OnMovement;
        pause.performed += OnPause;
        interact.performed += OnInteract;
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
        interact.performed -= OnInteract;
        movement.Disable();
        pause.Disable();
        PauseMenu.OnPauseGame -= SetPaused;
        GameController.ChangeGameState -= ChangeInputByState;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;
        OnInteractInput?.Invoke();
    }

    private void ChangeInputByState(GameState state)
    {
        switch (state)
        {
            case GameState.Puzzle:
                inputEnabled = false;
                puzzleMode = true;
                OnMoveInput?.Invoke(Vector2.zero, false); // force stop
                break;
            case GameState.Death:
            case GameState.Inactive:
                inputEnabled = false;
                puzzleMode = false;
                break;
            case GameState.Active:
                inputEnabled = true;
                puzzleMode = false;
                break;
            case GameState.Win:
                inputEnabled = false;
                puzzleMode = false;
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
        if (puzzleMode)
        {
            OnPuzzleBack?.Invoke();
            return;
        }
        if (!inputEnabled) return;
        OnPauseInput?.Invoke();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
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
