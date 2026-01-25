using DG.Tweening;
using FMOD.Studio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject endPanel;
    private CanvasGroup endPanelCG; // fuck it idc
    public static event Action<GameState> ChangeGameState;
    private GameState currentState;
    public DogController dogController;
    private int dogs = 3; // three dogs,TODO: unify with other dog logic
    private int solvedPuzzles;
    private int puzzleTarget;
    private void OnEnable()
    {
        LevelChanger.OnFadeInFinished += StartGame;
        InteractablesHandler.OpenPuzzleMenu += EnterPuzzleMode;
        PuzzleController.OnLeavePuzzle += LeavePuzzleMode;
        LevelChanger.OnGameplayLevelLoaded += SetupLevel;
        ChangeGameState += HandleNewState;
        Timer.OnTimerEnd += HandleTimerEnd;
    }

    private void OnDisable()
    {
        LevelChanger.OnFadeInFinished -= StartGame;
        ChangeGameState -= HandleNewState;
        InteractablesHandler.OpenPuzzleMenu -= EnterPuzzleMode;
        PuzzleController.OnLeavePuzzle -= LeavePuzzleMode;
        LevelChanger.OnGameplayLevelLoaded -= SetupLevel;
        Timer.OnTimerEnd -= HandleTimerEnd;
    }

    private void SetupLevel(LevelData data)
    {
        if (data.levelID == 0) // first level 
        {
            dogs = 3;
            SaveManager.Instance.runtimeData.dogs = 3;
        }
        else
        {
            dogs = SaveManager.Instance.runtimeData.dogs;
        }
        dogController.spawnDogs(dogs);
        solvedPuzzles = 0;
        puzzleTarget = data.targetScore;
    }

    private void HandleTimerEnd()
    {
        dogs--;
        SaveManager.Instance.runtimeData.dogs = dogs;
        StartCoroutine(dogController.DelayDogErasure());
        Debug.Log($"dog lost.. {dogs} left..."); // TODO - fade to black here for a second
        if (dogs <= 0) // should we game over on 0 dogs or -1 dogs?
        {
            ChangeGameState.Invoke(GameState.Death);
            return;
        }
        LeavePuzzleMode(false);
    }

    private void EnterPuzzleMode()
    {
        ChangeGameState?.Invoke(GameState.Puzzle);
    }

    private void LeavePuzzleMode(bool finished)
    {
        ChangeGameState?.Invoke(GameState.Active);
        if (finished) // FIXME - running out of time counts as a "win" rn
        {
            solvedPuzzles++;
            if (solvedPuzzles >= puzzleTarget)
            {
                ChangeGameState?.Invoke(GameState.Win);
            }
        }
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
            case GameState.Win: // TODO - make it possible to win
                Time.timeScale = 1f;
                var currentLevel = SaveManager.Instance.runtimeData.currentLevel;
                SaveManager.Instance.runtimeData.currentLevel = LevelChanger.Instance.GetNextLevel(currentLevel);
                if (SaveManager.Instance.runtimeData.currentLevel != null)
                {
                    endPanelCG.alpha = 0f;
                    endPanel.gameObject.SetActive(true);
                    endPanelCG.DOFade(1f, 0.5f);
                }
                else
                {
                    LevelChanger.Instance.FadeToLevel("WinScenario"); // TODO - adjust text if you lose some dogs?
                }
                break;
        }
    }

    private void StartGame()
    {
        ChangeGameState?.Invoke(GameState.Active);
    }

    private void Start()
    {
        endPanelCG = endPanel.GetComponent<CanvasGroup>();
        LevelChanger.Instance.HandleLevelLoad();
    }

}

public enum GameState
{
    Inactive,
    Active,
    Puzzle,
    Death,
    Win
}