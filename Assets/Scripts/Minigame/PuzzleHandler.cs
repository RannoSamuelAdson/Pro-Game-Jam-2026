using System;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    public static Action<bool> OnPuzzleCompleted; // bool is success state
    [SerializeField] private GameObject puzzleMenu;
    private void OnEnable()
    {
        InteractablesHandler.OpenPuzzleMenu += OpenMenu;
    }

    private void OnDisable()
    {
        InteractablesHandler.OpenPuzzleMenu -= OpenMenu;
    }

    private void OpenMenu()
    {
        puzzleMenu.SetActive(true);
    }


    public void PuzzleSuccess()
    {
        puzzleMenu.SetActive(false);
        OnPuzzleCompleted?.Invoke(true);
    }

    public void PuzzleFail()
    {
        puzzleMenu.SetActive(false);
        OnPuzzleCompleted?.Invoke(false);
    }
}
