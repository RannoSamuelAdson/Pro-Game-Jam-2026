using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class BootAnim : MonoBehaviour
{
    public Sprite[] logos;
    private float delayTime;
    private bool isReady = false;
    private bool animOver = false;
    private int logoIndex = 0;
    private Image imageRenderer;
    private float startTime;
    private LogoState logoState = LogoState.Start;
    private bool mobileTouch = false;
    private bool STM = false;
    private bool evilSTM = false;
    private bool canSkip = false;
    // Start is called before the first frame update

    private void Awake()
    {
        var args = System.Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            var lowerArg = arg.ToLower();
            switch (lowerArg)
            {
                case "--straight-to-menu™️-by-tommy-tacore™️":
                case "-stm":
                case "--straight-to-menu":
                    STM = true;
                    break;
                case "-evilstm":
                case "--slow-to-menu":
                    evilSTM = true;
                    break;
            }
        }
    }
    void Start()
    {
        imageRenderer = GetComponent<Image>();
        LevelChanger.Instance.FadeIn();
        EnhancedTouchSupport.Enable();
        isReady = true;
        if (!evilSTM) InputSystem.onAnyButtonPress.CallOnce(_ => animOver = true);
        if (STM && !evilSTM) animOver = true;
    }

    private void OnEnable()
    {
        LoadBankAndScene.OnBanksLoaded += CanSkip;
    }
    private void OnDisable()
    {
        LoadBankAndScene.OnBanksLoaded -= CanSkip;
        EnhancedTouchSupport.Disable();
    }

    private void CanSkip()
    {
        canSkip = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady && animOver && canSkip)
        {
            isReady = false; // no need to call it multiple times
            LevelChanger.Instance.FadeToLevel("MainMenu");
        }

        if (Touch.activeTouches.Count > 0)
        {
            if (Touch.activeTouches[0].phase == TouchPhase.Began) mobileTouch = true;
            else mobileTouch = false;
        }

        if (mobileTouch) animOver = true;


        if (animOver) return;
        // handle state change
        if (Time.unscaledTime > startTime + delayTime)
        {
            startTime = Time.unscaledTime;
            switch (logoState)
            {
                case LogoState.Start:
                    logoState = LogoState.FadeIn;
                    delayTime = evilSTM ? 3f : 0f;
                    break;
                case LogoState.FadeIn:
                    logoState = LogoState.FadeOut;

                    imageRenderer.sprite = logos[logoIndex];
                    imageRenderer.color = Helper.TransparentColor(imageRenderer.color);
                    imageRenderer.DOFade(1f, evilSTM ? 15f : 1f).SetUpdate(true);
                    delayTime = evilSTM ? 30f : 2f;
                    break;
                case LogoState.FadeOut:
                    logoState = LogoState.End;

                    imageRenderer.DOFade(0f, evilSTM ? 15f : 1f).SetUpdate(true);
                    delayTime = evilSTM ? 15f : 1f;
                    break;
                case LogoState.End:
                    delayTime = evilSTM ? 3f : 0f;
                    logoState = LogoState.Start;
                    logoIndex++;
                    if (logoIndex >= logos.Length)
                    {
                        animOver = true;
                    }
                    break;
            }
        }
    }

    enum LogoState
    {
        Start,
        FadeIn,
        FadeOut,
        End
    }
}
