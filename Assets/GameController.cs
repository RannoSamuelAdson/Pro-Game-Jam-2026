using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameState> ChangeGameState;
    private GameState currentState;
    public DogController dogController;
    private int dogs = 3; // three dogs,TODO: unify with other dog logic
    private void OnEnable()
    {
        LevelChanger.OnFadeInFinished += StartGame;
        InteractablesHandler.OpenPuzzleMenu += EnterPuzzleMode;
        PuzzleController.OnLeavePuzzle += LeavePuzzleMode;
        ChangeGameState += HandleNewState;
        Timer.OnTimerEnd += HandleTimerEnd;
    }

    private void OnDisable()
    {
        LevelChanger.OnFadeInFinished -= StartGame;
        ChangeGameState -= HandleNewState;
        InteractablesHandler.OpenPuzzleMenu -= EnterPuzzleMode;
        PuzzleController.OnLeavePuzzle -= LeavePuzzleMode;
        Timer.OnTimerEnd -= HandleTimerEnd;
    }

    private void HandleTimerEnd()
    {
        dogs--;
        dogController.removeDog();
        Debug.Log($"dog lost.. {dogs} left...");
        if (dogs <= 0) // should we game over on 0 dogs or -1 dogs?
        {
            ChangeGameState.Invoke(GameState.Death);
            return;
        }
        LeavePuzzleMode(true);
    }

    private void EnterPuzzleMode()
    {
        ChangeGameState?.Invoke(GameState.Puzzle);
    }

    private void LeavePuzzleMode(bool finished)
    {
        ChangeGameState?.Invoke(GameState.Active);
    }

    private void HandleNewState(GameState state)
    {
        Debug.Log($"Switching from state {currentState} to {state}.");
        currentState = state;

        switch (currentState)
        {
            case GameState.Inactive:
                Time.timeScale = 0f;
                break;
            case GameState.Puzzle:
                Time.timeScale = 1f;
                break;
            case GameState.Active:
                Time.timeScale = 1f;
                break;
            case GameState.Death:
                Time.timeScale = 0f;
                LevelChanger.Instance.FadeToLevel("LoseScenario");
                break;
        }
    }

    private void StartGame()
    {
        ChangeGameState?.Invoke(GameState.Active);
    }

    private void Start()
    {
        LevelChanger.Instance.FadeIn();
    }

}

public enum GameState
{
    Inactive,
    Active,
    Puzzle,
    Death
}