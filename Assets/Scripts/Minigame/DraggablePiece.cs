using UnityEngine;

public class DraggablePiece : MonoBehaviour
{
    private bool isDragging;
    private Vector3 offset;
    private Camera cam;
    
    [Header("Snapping")]
    public Transform targetPosition;
    public float snapDistance = 0.5f;
    public bool isSnapped;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        offset = transform.position - mouseWorldPos;
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        TrySnap();
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        transform.position = mouseWorldPos + offset;
    }


    void TrySnap()
    {
        if (isSnapped) return;

        float distance = Vector3.Distance(transform.position, targetPosition.position);

        if (distance <= snapDistance)
        {
            transform.position = targetPosition.position;
            isSnapped = true;
        }
    }
}