using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePiece : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isDragging;
    private PuzzlePieceSpot currentSnap;


    public bool isCorrect = false;

    private Vector2 offset;
    public PuzzlePieceSpot CorrectTarget;
    public float snapDistance = 50f; // pixels
    private PuzzlePieceSpot[] snapTargets;

    void TrySnap()
    {
        foreach (PuzzlePieceSpot snapTarget in snapTargets) {

            
            float distance = Vector2.Distance(
                rectTransform.transform.position,
                snapTarget.transform.position
            );

            if (distance <= snapDistance)
            {
                //Debug.Log("Snapping");
                if (snapTarget.IsOccupied) 
                {
                    SwitchPuzzlePieces(snapTarget);
                    return;
                }
                //Debug.Log("Ordinary Sanp");
                snapTarget.IsOccupied = true;
                snapTarget.currentPuzzlePiece = this;
                
                if (currentSnap != null) 
                { 
                    currentSnap.currentPuzzlePiece = null;
                    currentSnap.IsOccupied = false;
                }

                currentSnap = snapTarget;
                rectTransform.transform.position = snapTarget.transform.position;

                if (snapTarget == CorrectTarget) isCorrect = true;

                return;
            }

        }
        if (currentSnap != null)
        {
            currentSnap.IsOccupied = false;
            currentSnap = null;
        }
    }

    void SwitchPuzzlePieces(PuzzlePieceSpot newTarget)
    {
        DraggablePiece otherPiece = newTarget.currentPuzzlePiece;
        
        
        otherPiece.currentSnap = currentSnap; 

        

        if (currentSnap != null) 
        {         
            otherPiece.rectTransform.transform.position = currentSnap.transform.position;
        }
        else
        {
            otherPiece.rectTransform.transform.position = rectTransform.transform.position;
        }

        rectTransform.transform.position = newTarget.transform.position;
        newTarget.currentPuzzlePiece = this;
        currentSnap = newTarget;

        otherPiece.rectTransform.SetAsLastSibling();

    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    private void OnEnable()
    {
        snapTargets = UnityEngine.Object.FindObjectsByType<PuzzlePieceSpot>(FindObjectsSortMode.None);

    }
    private void Update()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        isCorrect = false;

        rectTransform.SetAsLastSibling();

        // Convert mouse position to local UI space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        rectTransform.localPosition = localPoint - offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        TrySnap();
    }

}
