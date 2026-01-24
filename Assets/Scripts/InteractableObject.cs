using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isActive = false;
    // maybe an array

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
            Debug.Log("hi!!!");

        
    }
}
