using DG.Tweening;
using UnityEngine;

public class ScalableButton : CustomButtonBase
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    [SerializeField] private float scaleBy = 0.1f;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private Transform target; // optional
    Tween pulseTween;
    public override void Awake()
    {
        base.Awake();
        if (target == null) target = transform;
        originalScale = target.localScale;
        Vector3 scaleVector = new(Mathf.Sign(originalScale.x) * scaleBy, Mathf.Sign(originalScale.y) * scaleBy, Mathf.Sign(originalScale.z) * scaleBy);
        targetScale = originalScale + scaleVector;
        Vector3 halfScale = originalScale + scaleVector * 0.5f;
        // powered by gpt :))
        // Create the initial scale tween
        Tween initialScaleTween = target.DOScale(targetScale, duration).SetEase(Ease.InOutSine).Pause();

        // Create a sequence for pulsing
        Sequence pulseSequence = DOTween.Sequence();
        pulseSequence.AppendInterval(1f);
        // Add pulses
        int pulseCount = 3;
        for (int i = 0; i < pulseCount; i++)
        {
            pulseSequence.Append(target.DOScale(halfScale, 0.75f).SetEase(Ease.InOutSine))
                         .Append(target.DOScale(targetScale, 0.75f).SetEase(Ease.InOutSine));
            if (pulseCount != i - 1)
            {
                pulseSequence.AppendInterval(0.05f);
            }
        }

        // Loop the pulse sequence indefinitely
        pulseSequence.SetLoops(int.MaxValue);

        // Combine the initial scale tween and the pulse sequence
        pulseTween = DOTween.Sequence()
                         .Append(initialScaleTween)
                         .Append(pulseSequence)
                         .Pause().SetUpdate(true);
        pulseTween.SetLink(gameObject); // ensure tweens are killed
    }
    public override void Normal()
    {
        base.Normal();
        if (pulseTween.IsPlaying())
        {
            pulseTween.Pause();
            target.DOScale(originalScale, duration).SetEase(Ease.InOutSine).SetUpdate(true);
        }

    }

    public override void Pressed()
    {
        base.Pressed();
        pulseTween.Pause();
        target.DOScale(originalScale, duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public override void Selected()
    {
        base.Selected();
        pulseTween.Rewind();
        pulseTween.Play();
        //target.DOScale(targetScale, duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }
    private void OnDestroy()
    {
        DOTween.Kill(target);
    }
}