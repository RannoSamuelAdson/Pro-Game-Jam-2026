using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if !UNITY_EDITOR && !UNITY_ANDROID
using UnityEngine.InputSystem;
#endif

public class LevelChanger : MonoBehaviour
{
    public static event Action OnFadeInFinished;
    public float transitionTime = 1f; // couldnt get animation events to work right now
    private bool loadInProgess = false;
    private List<Animator> animators = new();
    private GameObject preloadBlack;

    public static LevelChanger Instance { get; private set; }

    private Coroutine co_HideCursor;

    void Update()
    {
#if !UNITY_EDITOR && !UNITY_ANDROID
        if (Mouse.current.delta.x.ReadValue() == 0 && (Mouse.current.delta.y.ReadValue() == 0))
        {
            if (co_HideCursor == null)
            {
                co_HideCursor = StartCoroutine(HideCursor());
            }
        }
        else
        {
            if (co_HideCursor != null)
            {
                StopCoroutine(co_HideCursor);
                co_HideCursor = null;
                Cursor.visible = true;
            }
        }
#endif
    }

    private Animator GetRandomAnimator()
    {
        foreach (Animator anim in animators) anim.gameObject.SetActive(false); // disable all beforehand
        Animator animator = animators[UnityEngine.Random.Range(0, animators.Count)];
        animator.gameObject.SetActive(true);
        return animator;
    }
    private IEnumerator HideCursor()
    {
        yield return new WaitForSeconds(5);
        Cursor.visible = false;
    }

    public void Awake()
    {
        Instance = this;
        Animator childAnimator;
        // find all transitions
        foreach (Transform child in transform)
        {
            if (child.name == "Preload")
            {
                preloadBlack = child.gameObject;
                preloadBlack.GetComponentInChildren<Image>().color = Color.black;
            }
            else if (child.TryGetComponent(out childAnimator))
            {
                animators.Add(childAnimator);
            }
        }
    }

    private void Start()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }


    private void SetTrigger(Animator animator, string trigger)
    {
        animator.SetTrigger(trigger);
        StartCoroutine(DisableBlack());
    }

    IEnumerator DisableBlack()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        preloadBlack.SetActive(false);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    public void FadeToLevel(string levelName)
    {
        if (!loadInProgess) StartCoroutine(LoadLevel(levelName));
        else Debug.LogWarning($"Already loading a level, cannot load {levelName}!");
    }
    public void FadeToDesktop()
    {
        if (!loadInProgess) StartCoroutine(QuitToDesktop());
        else Debug.LogWarning($"Already loading a level!");
    }
    IEnumerator LoadLevel(string levelToLoad, bool stopAudio = true)
    {
        if (SaveManager.Instance != null) SaveManager.Instance.runtimeData.previousSceneName = SceneManager.GetActiveScene().name;
        loadInProgess = true;
        if (AudioManager.Instance != null && stopAudio)
        {
            AudioManager.Instance.FadeOutMusic();
            AudioManager.Instance.StopSFX();
        }
        SetTrigger(GetRandomAnimator(), "FadeOut");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(transitionTime);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadInProgess = false;
    }
    IEnumerator QuitToDesktop()
    {
        Debug.Log("Quitting.");
        loadInProgess = true;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FadeOutMusic();
            AudioManager.Instance.StopSFX();
        }
        SetTrigger(GetRandomAnimator(), "FadeOut");
        yield return new WaitForSecondsRealtime(transitionTime);
        loadInProgess = false;
        Application.Quit();
    }
    IEnumerator FadeInCoroutine()
    {
        SetTrigger(GetRandomAnimator(), "FadeIn");
        yield return new WaitForSecondsRealtime(transitionTime);
        OnFadeInFinished?.Invoke();
    }
}