using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public static event Action<InteractableObject> RegisterObject;
    public static event Action<InteractableObject, bool> CloseToObject; // object, isClose
    private bool isActive = false;
    private bool isPlayerClose;
    // maybe an array, objecthandler or w/e
    private GameObject toolTip;
    private void Start()
    {
        toolTip = transform.GetChild(0).gameObject;
        toolTip.SetActive(false);
        RegisterObject?.Invoke(this);
    }

    public void ActivateItem()
    {
        // START PLAYING KNOCKING/WHATEVER SFX here
        isActive = true;
        // TODO - show it visually somehow
        GetComponent<SpriteRenderer>().color = Color.yellow;
        if (isPlayerClose)
        {
            CloseToObject.Invoke(this, true);
            toolTip.SetActive(true);
        }
    }

    public void DeactivateItem(bool success)
    {
        isActive = false;
        toolTip.SetActive(false);
        // stop SFX as well

        if (success)
        {
            // TODO - all good we are safe
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            // destroyed, swap sprite etc etc
            GetComponent<SpriteRenderer>().color = Color.orange;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerClose = true;
        if (isActive)
        {
            CloseToObject.Invoke(this, true);
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        isPlayerClose = false;
        if (isActive)
        {
            toolTip.SetActive(false);
            CloseToObject.Invoke(this, false);
        }
    }
}
