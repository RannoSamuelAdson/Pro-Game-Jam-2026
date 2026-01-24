using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class AdjustableCanvasScaleMatch : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (canvasScaler == null) return;
        float ratio = (float)Screen.width / Screen.height;
        if (ratio > (16f / 9f)) canvasScaler.matchWidthOrHeight = 1f;
        else canvasScaler.matchWidthOrHeight = 0f;
    }
}
