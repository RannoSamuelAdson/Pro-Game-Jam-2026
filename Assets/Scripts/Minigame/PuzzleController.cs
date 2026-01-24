using System;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    private DraggablePiece[] pieces;
    public static Action<bool> OnLeavePuzzle; // bool is success state, true: clear, false: left
    private bool puzzleSolved = true; // true by default so a new puzzle gets spawned

    // TODO - maybe delay hiding the gameobject on success

    // Update is called once per frame
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

    public void PuzzleSuccess()
    {
        OnLeavePuzzle?.Invoke(true);
    }

    private void OnEnable()
    {
        if (puzzleSolved)
        {
            Instantiate(puzzles[UnityEngine.Random.Range(0, puzzles.Length)], transform.GetChild(0));
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
        if (puzzleSolved)
        {
            Destroy(transform.GetChild(0).GetChild(0).gameObject); // destroy old puzzle
        }
        GameController.ChangeGameState -= OnPuzzleEnd;
        InputHandler.OnPuzzleBack -= ClosePuzzle;
    }
}
