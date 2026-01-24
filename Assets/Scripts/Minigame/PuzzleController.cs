using System;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    private DraggablePiece[] pieces;
    public static Action<bool> OnPuzzleCompleted; // bool is success state
    public DraggablePiece[] pieces;
    private bool puzzleSolved;

    // TODO - lose condition, maybe delay hiding the gameobject on success

    // Update is called once per frame
    void Update()
    {
        if (puzzleSolved) return;

        foreach (DraggablePiece piece in pieces) {

            if (!piece.isCorrect) return;
        }
        puzzleSolved = true;
        PuzzleSuccess();
    }

    public void PuzzleSuccess()
    {
        OnPuzzleCompleted?.Invoke(true);
        gameObject.SetActive(false);
    }

    public void PuzzleFail()
    {
        OnPuzzleCompleted?.Invoke(false);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        pieces = UnityEngine.Object.FindObjectsByType<DraggablePiece>(FindObjectsSortMode.None);
    }
}
