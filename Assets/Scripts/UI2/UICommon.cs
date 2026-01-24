using DG.Tweening;
using UnityEngine;

public class UICommon
{
    public static bool FadeInCG(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup.alpha > 0f) return false; // already fading in
        canvasGroup.DOKill(); // if fading out still
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, duration).SetUpdate(true).OnComplete(() => canvasGroup.interactable = true); // maybe make it interactable mid transition idk

        return true;
    }

    public static bool FadeOutCG(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup.alpha < 1f) return false; // already fading out
        canvasGroup.DOKill(); // if fading in still
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0f, duration).SetUpdate(true).OnComplete(()=>canvasGroup.gameObject.SetActive(false)); // maybe make it interactable mid transition idk

        return true;
    }
}
