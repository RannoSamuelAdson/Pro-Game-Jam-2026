using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private GameObject puzzleMenu;

    private void OnEnable()
    {
        InteractablesHandler.OpenPuzzleMenu += OpenMenu;
        LevelChanger.OnGameplayLevelLoaded += SetupLevel;
    }

    private void OnDisable()
    {
        InteractablesHandler.OpenPuzzleMenu -= OpenMenu;
        LevelChanger.OnGameplayLevelLoaded -= SetupLevel;
    }

    private void SetupLevel(LevelData data)
    {
        puzzleMenu.GetComponent<PuzzleController>().SetPuzzles(data.puzzles);
    }

    private void OpenMenu()
    {
        puzzleMenu.SetActive(true);
        AudioManager.PlayOneShot(FMODEvents.Instance.PuzzleOpen);
    }
}
