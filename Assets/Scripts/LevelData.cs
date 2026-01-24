
using FMODUnity;
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Level", menuName = "Levels/New Level")]
[Serializable]
public class LevelData : ScriptableObject
{
    [Header("Metadata")]
    public uint levelID;
    public string levelName;
    [Header("Data")]
    public int timer;
    public GameObject[] puzzles;
    public EventReference levelTheme;
    public int targetScore; // how many puzzles to do
    // TODO - handle enemy specific SFX somewhere, could just have it as a per-level setting, maybe an array where we pick random sfx from
}
