using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameState> ChangeGameState;
    private GameState currentState;
    private void OnEnable()
    {
        LevelChanger.OnFadeInFinished += StartGame;
        ChangeGameState += HandleNewState;
    }

    private void OnDisable()
    {
        LevelChanger.OnFadeInFinished -= StartGame;
        ChangeGameState -= HandleNewState;
    }

    private void HandleNewState(GameState state)
    {
        currentState = state;
        switch (currentState)
        {
            case GameState.Inactive:
                Time.timeScale = 0f;
                break;
            case GameState.Active:
                Time.timeScale = 1f;
                break;
            case GameState.Death:
                Time.timeScale = 0f;
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
    Death
}