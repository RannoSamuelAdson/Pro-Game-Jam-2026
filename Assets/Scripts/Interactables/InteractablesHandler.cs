using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesHandler : MonoBehaviour
{
    public static event Action OpenPuzzleMenu;
    private List<InteractableObject> interactableObjects = new();
    private InteractableObject currentSelectedObject;
    private List<InteractableObject> activatedObjects = new();
    private float lastCheck;
    private float waitTime = 3f; // time to wait until new incident. could be randomized later.
    private int maxActivatedObjects = 1; // can change if needed.
    private bool itemActivated = false; // hardcoded to one active situation atm
    private bool hasFoundAllPieces = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InteractableObject.RegisterObject += AddObject;
        lastCheck = Time.time;
    }

    private void OnEnable()
    {
        InteractableObject.CloseToObject += HandleBeingClose;
        InputHandler.OnInteractInput += HandleInteract;
        PuzzleController.OnLeavePuzzle += OnEndPuzzle;
        Timer.OnTimerEnd += OnFailPuzzle;
        PieceSpawner.OnAllPiecesCollected += EnablePuzzle;
    }

    private void OnDisable()
    {
        InteractableObject.CloseToObject += HandleBeingClose;
        InteractableObject.RegisterObject -= AddObject;
        InputHandler.OnInteractInput -= HandleInteract;
        PuzzleController.OnLeavePuzzle -= OnEndPuzzle;
        Timer.OnTimerEnd -= OnFailPuzzle;
        PieceSpawner.OnAllPiecesCollected -= EnablePuzzle;
    }

    private void EnablePuzzle()
    {
        hasFoundAllPieces = true;
    }

    private void HandleInteract()
    {
        if (currentSelectedObject == null || !hasFoundAllPieces) return;
        OpenPuzzleMenu?.Invoke();
        
    }

    private void HandleBeingClose(InteractableObject obj, bool isClose)
    {
        if (isClose)
            currentSelectedObject = obj;
        else if (currentSelectedObject == obj && !isClose)
            currentSelectedObject = null;
        // if currently selected is unrelated, ignore but log
        else if (currentSelectedObject == null)
        {
            Debug.LogWarning("walked away from obj that wasnt even selected");
        }
        else
            Debug.LogWarning($"Walked away from {obj.name}, even though current selected is {currentSelectedObject.name}");
    }

    // when the puzzle is either solved properly or quit. bool specifies that
    private void OnEndPuzzle(bool finished)
    {
        if (!finished) return;
        hasFoundAllPieces = false;
        interactableObjects.Remove(currentSelectedObject);
        activatedObjects.Remove(currentSelectedObject);
        currentSelectedObject.DeactivateItem(true);
        currentSelectedObject = null;
        itemActivated = false;
        lastCheck = Time.time;
    }

    private void OnFailPuzzle()
    {
        if (itemActivated)
        {
            interactableObjects.Remove(currentSelectedObject);
            hasFoundAllPieces = false;
            foreach (var item in activatedObjects)
            {
                item.DeactivateItem(false);
            }
            activatedObjects = new(); // empty the list
            itemActivated = false;
            lastCheck = Time.time;
            Debug.Log("puzzle failed...");
        }
    }
    private void AddObject(InteractableObject obj)
    {
        interactableObjects.Add(obj);
    }
    private void RemoveObject(InteractableObject obj)
    {
        interactableObjects.Remove(obj);
    }

    private void Update()
    {
        if (activatedObjects.Count < maxActivatedObjects && lastCheck + waitTime < Time.time)
        {
            if (interactableObjects.Count <= 0) return;
            var obj = interactableObjects[UnityEngine.Random.Range(0, interactableObjects.Count)]; // basic random activation logic
            obj.ActivateItem();
            activatedObjects.Add(obj);
            itemActivated = true;
        }
    }
}
