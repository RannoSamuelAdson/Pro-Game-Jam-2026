using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public static event Action<InteractableObject> RegisterObject;
    public static event Action<InteractableObject, bool> CloseToObject; // object, isClose
    [SerializeField] private Sprite destroyedSprite;
    private bool isActive = false;
    private bool isPlayerClose;
    private bool isShaking = false;
    private float speed = 8.0f; //how fast it shakes
    private float amount = 0.05f; //how much it shakes
    Vector2 startingPos;

    // maybe an array, objecthandler or w/e
    private GameObject toolTip;
    private void Start()
    {
        toolTip = transform.GetChild(0).gameObject;
        toolTip.SetActive(false);
        RegisterObject?.Invoke(this);
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
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
        isShaking = true;
    }

    private void Update()
    {
        if (isShaking)
        {
            transform.position =  new Vector2((startingPos.x + Mathf.Sin(Time.time * speed) * amount ), (startingPos.y + (Mathf.Sin(Time.time * speed) * amount) ));
        }
    }
    public void DeactivateItem(bool success)
    {
        isActive = false;
        toolTip.SetActive(false);
        // stop SFX as well
        isShaking = false;
        transform.position = startingPos;
        if (success)
        {
            // all good we are safe, stop shaking
        }
        else
        {
            // destroyed, swap sprite etc etc
            GetComponent<SpriteRenderer>().sprite = destroyedSprite;
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
