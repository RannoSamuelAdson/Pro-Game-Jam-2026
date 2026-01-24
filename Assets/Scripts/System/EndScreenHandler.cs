using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class EndScreenHandler : MonoBehaviour
{
    IDisposable disposable;

    public TextMeshProUGUI[] texts;
    public float fadeDuration = 0.8f;
    public float delayBetween = 3f;

    void Start()
    {
        AudioManager.Instance.StartMusic();
        LevelChanger.Instance.FadeIn();
        disposable = InputSystem.onAnyButtonPress.CallOnce(ctrl => OnEndScreenEnd());
        Time.timeScale = 1f;
        
        
        Sequence seq = DOTween.Sequence();

        foreach (var text in texts)
        {
            text.alpha = 0f;
            seq.Append(text.DOFade(1f, fadeDuration));
            seq.AppendInterval(delayBetween);
        }
    }
    public void OnEndScreenEnd()
    {
        LevelChanger.Instance.FadeToLevel("Credits");
    }

    private void OnDisable()   
    {
        if (disposable != null) disposable.Dispose();
    }
}
