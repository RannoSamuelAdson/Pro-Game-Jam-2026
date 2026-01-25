using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class EndScreenHandler : MonoBehaviour
{
    IDisposable disposable;

    [Header("Timing Settings")]
    public float fadeDuration = 0.4f; 
    public float delayBetween = 0.5f; 

    public TextMeshProUGUI[] texts;

    private int currentTextIndex = -1;
    private Tween currentFadeTween;
    private Coroutine autoAdvanceCoroutine;

    // FIXME: chatgpt generated code, transitions are too slow but i dont have time to fix it right now.
    void Start()
    {
        AudioManager.Instance.InitializeMusic(SceneManager.GetActiveScene().name.Contains("Win") ? FMODEvents.Instance.GoodEndingMusic : FMODEvents.Instance.BadEndingMusic);
        AudioManager.Instance.StartMusic();
        LevelChanger.Instance.FadeIn();

        foreach (var text in texts) text.alpha = 0f;

        disposable = InputSystem.onAnyButtonPress.Call(ctrl => HandleInput());

        Time.timeScale = 1f;
        AdvanceText();
    }

    private void HandleInput()
    {
        // Skip current fade and move to wait state immediately
        if (currentFadeTween != null && currentFadeTween.IsActive() && currentFadeTween.IsPlaying())
        {
            currentFadeTween.Complete();
            // The OnComplete callback in AdvanceText will naturally start the coroutine
            return;
        }

        // Skip the waiting period and go to next line immediately
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
            AdvanceText();
        }
    }

    private void AdvanceText()
    {
        currentTextIndex++;

        if (currentTextIndex >= texts.Length)
        {
            OnEndScreenEnd();
            return;
        }

        // Use a faster ease (OutCubic) to make the text feel like it pops in better
        currentFadeTween = texts[currentTextIndex].DOFade(1f, fadeDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => {
                autoAdvanceCoroutine = StartCoroutine(WaitAndAdvance());
            });
    }

    private IEnumerator WaitAndAdvance()
    {
        yield return new WaitForSeconds(delayBetween);
        autoAdvanceCoroutine = null;
        AdvanceText();
    }

    public void OnEndScreenEnd()
    {
        DisposeInput();
        LevelChanger.Instance.FadeToLevel("Credits");
    }

    private void OnDisable() => DisposeInput();

    private void DisposeInput()
    {
        if (disposable != null)
        {
            disposable.Dispose();
            disposable = null;
        }
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
    }
}