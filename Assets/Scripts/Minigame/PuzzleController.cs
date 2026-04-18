using System;
using System.Collections.Generic; // Added for List support
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> puzzles = new List<GameObject>(); // Changed to List
    private DraggablePiece[] pieces;
    public static Action<bool> OnLeavePuzzle;
    private bool puzzleSolved = true;

    void Update()
    {
        if (puzzleSolved) return;

        foreach (DraggablePiece piece in pieces)
        {
            if (!piece.isCorrect) return;
        }
        puzzleSolved = true;
        PuzzleSuccess();
    }

    public void SetPuzzles(GameObject[] newPuzzles)
    {
        puzzles = new List<GameObject>(newPuzzles);
    }

    public void PuzzleSuccess()
    {
        OnLeavePuzzle?.Invoke(true);
    }

    private void OnEnable()
    {
        if (puzzleSolved && puzzles.Count > 0)
        {
            // 1. Pick a random index
            int randomIndex = UnityEngine.Random.Range(0, puzzles.Count);

            // 2. Instantiate the selected puzzle
            GameObject puzzle = Instantiate(puzzles[randomIndex], transform.GetChild(0));
            puzzle.transform.SetLocalPositionAndRotation(new Vector3(-400f, 0), puzzle.transform.localRotation);
            // 3. Remove from list so it doesn't repeat, UNLESS it's the last one
            if (puzzles.Count > 1)
            {
                puzzles.RemoveAt(randomIndex);
            }

            puzzleSolved = false;
        }

        pieces = transform.GetComponentsInChildren<DraggablePiece>();
        GameController.ChangeGameState += OnPuzzleEnd;
        InputHandler.OnPuzzleBack += ClosePuzzle;
    }

    private void OnPuzzleEnd(GameState state)
    {
        if (state == GameState.Active || state == GameState.Death)
        {
            gameObject.SetActive(false);
        }
    }

    public void ClosePuzzle()
    {
        OnLeavePuzzle?.Invoke(false);
    }

    private void OnDisable()
    {
        if (puzzleSolved && transform.GetChild(0).childCount > 0)
        {
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
        }
        GameController.ChangeGameState -= OnPuzzleEnd;
        InputHandler.OnPuzzleBack -= ClosePuzzle;
    }
}