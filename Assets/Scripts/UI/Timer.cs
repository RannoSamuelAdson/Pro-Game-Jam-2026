using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public static event Action OnTimerEnd;

    // timer will count down time from a specified amount. when game is paused it shall not count time because why would it.
    // when timer runs 0 it will send an action saying everything is over we are all dead
    // when it feels like it it will either play some sound effect or mess with lantern lights (TODO)

    [SerializeField] private float initialTime = 20f; // TODO: unhardcode!!!!!!!
    private float gameTimer;
    bool timerActive;
    private GameState gameState;
    private void OnEnable()
    {
        GameController.ChangeGameState += HandleGameState;
        PuzzleController.OnPuzzleCompleted += ResetTimer;
    }

    private void HandleGameState(GameState state)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.Puzzle:
            case GameState.Active:
                timerActive = true;
                break;
            case GameState.Death:
            case GameState.Inactive:
                timerActive = false;
                break;
        }
    }

    private void OnDisable()
    {
        GameController.ChangeGameState -= HandleGameState;
        PuzzleController.OnPuzzleCompleted -= ResetTimer;
    }

    private void ResetTimer()
    {
        gameTimer = initialTime;
    }

    void Start()
    {
        gameTimer = initialTime;
        UpdateTimerDisplay(gameTimer);
    }

    void Update()
    {
        if (timerActive)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerDisplay(gameTimer);
            if (gameTimer <= 0f)
            {
                OnTimerEnd?.Invoke();
                gameTimer = initialTime + 1f;
            }
        }
    }

    void UpdateTimerDisplay(float time)
    {
        time = Mathf.Max(0, time);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
