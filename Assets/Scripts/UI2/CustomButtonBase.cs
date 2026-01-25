using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CustomButtonBase : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public ButtonType type;
    public bool disableSelectAudio;
    public bool disableClickAudio;
    private bool pointerDown = false;
    private bool isSelected = false;

    private bool toggleFirst = true; // only for toggles
    private bool isFirstBase = true;
    Selectable currentItem;
    public virtual void Awake()
    {
        if (gameObject.GetComponent<CustomButtonBase>() != this) isFirstBase = false;
        currentItem = GetComponent<Selectable>();
        if (TryGetComponent(out Button currentButton))
        {
            currentButton.onClick.AddListener(ClickSound);
        }

        if (TryGetComponent(out Toggle currentToggle))
        {
            currentToggle.onValueChanged.AddListener(ToggleSound);
        }
    }

    private void ToggleSound(bool value)
    {
        if (toggleFirst)
        {
            toggleFirst = false;
            return;
        }

        if (!isFirstBase) return;
        if (value) AudioManager.PlayOneShot(FMODEvents.Instance.ButtonClick);
        else AudioManager.PlayOneShot(FMODEvents.Instance.ButtonBack);
    }

    private void ClickSound()
    {
        if (!isFirstBase) return;
        if (disableClickAudio) return;
        switch (type)
        {
            case ButtonType.Normal:
                AudioManager.PlayOneShot(FMODEvents.Instance.ButtonClick);
                break;
            case ButtonType.Back:
                AudioManager.PlayOneShot(FMODEvents.Instance.ButtonBack);
                break;
        }
    }
    void OnEnable()
    {
        //toggleFirst = false; // reset the value

        if (EventSystem.current.currentSelectedGameObject == gameObject) Selected();
        else Normal();
    }

    void OnDisable()
    {
        Normal();
    }

    public virtual void Selected()
    {
        isSelected = true;
        if (pointerDown) return;
    }

    public virtual void Normal()
    {

    }

    public virtual void Pressed()
    {
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        Selected();
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        if (pointerDown) return;
        Normal();
        if (!isFirstBase) return;
        if (!currentItem.interactable || disableSelectAudio) return;
        //AudioManager.PlayOneShot(FMODEvents.Instance.ButtonSelect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        Pressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        if (isSelected)
        {
            Selected();
        }
        else
        {
            Normal();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerDown) return;
        if (EventSystem.current.currentSelectedGameObject == gameObject) return;
        if (currentItem.interactable && currentItem.navigation.mode != Navigation.Mode.None)
        {
            currentItem.Select();
            if (!isFirstBase) return;
            AudioManager.PlayOneShot(FMODEvents.Instance.ButtonSelect); // TODO - should this be removed?
        }
    }
}

public enum ButtonType
{
    Normal,
    Back
}