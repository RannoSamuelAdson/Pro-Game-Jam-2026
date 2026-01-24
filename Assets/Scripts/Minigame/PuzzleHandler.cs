using System;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
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
}
