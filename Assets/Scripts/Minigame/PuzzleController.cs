using System;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    private DraggablePiece[] pieces;
    public static Action OnPuzzleCompleted; // bool is success state
    private bool puzzleSolved;

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
        OnPuzzleCompleted?.Invoke();
    }

    private void OnEnable()
    {
        Instantiate(puzzles[UnityEngine.Random.Range(0, puzzles.Length)], transform.GetChild(0));
        pieces = transform.GetComponentsInChildren<DraggablePiece>();
        GameController.ChangeGameState += OnPuzzleEnd;
    }

    private void OnPuzzleEnd(GameState state)
    {
        if (state == GameState.Active || state == GameState.Death)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Destroy(transform.GetChild(0).GetChild(0).gameObject); // destroy old puzzle
        puzzleSolved = false;
        GameController.ChangeGameState -= OnPuzzleEnd;
    }
}
