using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesHandler : MonoBehaviour
{
    public static event Action OpenPuzzleMenu;
    private List<InteractableObject> interactableObjects = new List<InteractableObject>();
    private InteractableObject currentSelectedObject;
    private float lastCheck;
    private float waitTime = 3f; // time to wait until new incident. could be randomized later.
    private bool itemActivated = false; // hardcoded to one active situation atm
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log("Hello...");
        InteractableObject.RegisterObject += AddObject;
        lastCheck = Time.time;
    }

    private void OnEnable()
    {
        InteractableObject.CloseToObject += HandleBeingClose;
        InputHandler.OnInteractInput += HandleInteract;
        PuzzleHandler.OnPuzzleCompleted += OnEndPuzzle;
    }

    private void OnDisable()
    {
        InteractableObject.CloseToObject += HandleBeingClose;
        InteractableObject.RegisterObject -= AddObject;
        InputHandler.OnInteractInput -= HandleInteract;
        PuzzleHandler.OnPuzzleCompleted -= OnEndPuzzle;
    }

    private void HandleInteract()
    {
        if (currentSelectedObject == null) return;
        OpenPuzzleMenu?.Invoke();
    }

    private void HandleBeingClose(InteractableObject obj, bool isClose)
    {
        if (isClose)
            currentSelectedObject = obj;
        else if (currentSelectedObject == obj && !isClose)
            currentSelectedObject = null;
        // if currently selected is unrelated, ignore but log
        else
            Debug.LogWarning($"Walked away from {obj.name}, even though current selected is {currentSelectedObject.name}");
    }

    // when the puzzle is either solved properly or failed. success specifies that
    private void OnEndPuzzle(bool success)
    {
        interactableObjects.Remove(currentSelectedObject);
        currentSelectedObject.DeactivateItem(success);
        itemActivated = false;
        lastCheck = Time.time;
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
        if (!itemActivated && lastCheck + waitTime < Time.time)
        {
            if (interactableObjects.Count <= 0) return;
            interactableObjects[UnityEngine.Random.Range(0, interactableObjects.Count)].ActivateItem(); // basic random activation logic
            itemActivated = true;
        }
    }
}
