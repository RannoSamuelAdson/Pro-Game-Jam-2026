// from https://gist.github.com/emredesu/af597de14a4377e1ecf96b6f7b6cc506

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private readonly List<Selectable> m_Selectables = new();
    private ScrollRect m_ScrollRect;
    private InputAction uiNavigate;
    private Vector2 m_NextScrollPosition = Vector2.up;
    private Vector2Int groupSize;
    private bool isGridLayout = false;
    void OnEnable()
    {
        uiNavigate.Enable();
        StartCoroutine(DelayScroll());

    }

    IEnumerator DelayScroll()
    {
        yield return 0;
        if (m_ScrollRect.content.TryGetComponent<GridLayoutGroup>(out var layout))
        {
            isGridLayout = true;
            groupSize = layout.Size();
        }
        else
            groupSize = Vector2Int.zero;
        GetSelectables();
        ScrollToSelected(true);
    }
#nullable enable
    void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            if (uiNavigate.ReadValue<Vector2>().y != 0 || (isGridLayout && uiNavigate.ReadValue<Vector2>().x != 0))
            {
                mouseOver = false;
                MovePointerToCorner();
                ScrollToSelected(false);
            }
        }
    }
#nullable disable

    void MovePointerToCorner()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width, Screen.height));
    }

    void OnDisable()
    {
        uiNavigate.Disable();
    }

    void GetSelectables()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
    }
    void Awake()
    {
        var playerControls = new CustomInputActions();
        uiNavigate = playerControls.UI.Navigate;
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    void Update()
    {
        // If we are on mobile, do not do anything.
        if (BuildConsts.isMobile)
        {
            return;
        }
        if (m_Selectables.Count == 0) { GetSelectables(); }
        if (Mathf.Abs(Mouse.current.delta.x.ReadValue()) > 0.1f || Mathf.Abs(Mouse.current.delta.y.ReadValue()) > 0.1f)
        {
            mouseOver = true;
        }

        InputScroll();
        if (!mouseOver)
        {
            // Lerp scrolling code.
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
    }

    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        if (EventSystem.current == null) return;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;
        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            float rowCount = ((float)m_Selectables.Count - 1);
            if (isGridLayout) // grid layout code
            {
                selectedIndex = selectedIndex / groupSize.x;
                rowCount = groupSize.y - 1;
            }

            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / rowCount));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / rowCount));
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}