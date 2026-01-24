using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private DraggablePiece[] pieces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (DraggablePiece piece in pieces) {

            if (!piece.isCorrect) return;
        }
        Debug.Log("CORRECT!!");
    }
    private void OnEnable()
    {
        pieces = UnityEngine.Object.FindObjectsByType<DraggablePiece>(FindObjectsSortMode.None);
    }
}
