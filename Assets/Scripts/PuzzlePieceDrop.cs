using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class PuzzlePieceDrop : MonoBehaviour
{
    public static event Action<PuzzlePieceDrop> RegisterObject;
    public static event Action<PuzzlePieceDrop, bool> CloseToObject;
    private bool isPlayerClose;
    Vector2 startingPos;

    private GameObject toolTip;
    private void Start()
    {
        RegisterObject?.Invoke(this);
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
       
    }

  
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerClose = true;
        CloseToObject?.Invoke(this, true);
        Debug.Log("Touch");
        Destroy(this.gameObject);

    }

}
