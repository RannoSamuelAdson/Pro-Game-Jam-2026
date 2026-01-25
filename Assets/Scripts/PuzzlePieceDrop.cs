using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class PuzzlePieceDrop : MonoBehaviour
{
    public static event Action OnPieceCollected;
    public static event Action<PuzzlePieceDrop> RegisterObject;
    public static event Action<PuzzlePieceDrop, bool> CloseToObject;
    


    private void Start()
    {
        RegisterObject?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            CloseToObject?.Invoke(this, true);
            OnPieceCollected?.Invoke(); 
            Destroy(this.gameObject);
        
    }
}