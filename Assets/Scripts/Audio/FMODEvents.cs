using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference TestSound { get; private set; }
    [field: SerializeField] public EventReference NockNock { get; private set; }

    [field: SerializeField] public EventReference MudSteps { get; private set; }
    [field: SerializeField] public EventReference DogDeath { get; private set; }
    [field: SerializeField] public EventReference Thunder { get; private set; }
    [field: SerializeField] public EventReference MainAmbience { get; private set; }

    [field: SerializeField] public EventReference PuzzlePiece { get; private set; }
    [field: SerializeField] public EventReference PuzzleOpen { get; private set; }

    [field: SerializeField] public EventReference PuzzleCollect { get; private set; }

    [field: SerializeField] public EventReference Wolf { get; private set; }
    [field: SerializeField] public EventReference Walker { get; private set; }

    [field: Header("UI")]
    [field: SerializeField] public EventReference ButtonClick { get; private set; }
    [field: SerializeField] public EventReference ButtonBack { get; private set; }
    [field: SerializeField] public EventReference ButtonSelect { get; private set; }
    [field: SerializeField] public EventReference GameStartClick { get; private set; }


    [field: Header("Music")]
    [field: SerializeField] public EventReference MenuMusic { get; private set; }
    [field: SerializeField] public EventReference CreditsMusic { get; private set; }

    [field: SerializeField] public EventReference BadEndingMusic{ get; private set; }
    [field: SerializeField] public EventReference GoodEndingMusic { get; private set; }
    [field: SerializeField] public EventReference GameplaySong { get; private set; }
    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
}
