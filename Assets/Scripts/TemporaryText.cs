using DG.Tweening;
using TMPro;
using UnityEngine;

public class TemporaryText : MonoBehaviour
{
    public bool bob; // If true, text never shows
    public float delaySeconds = 15f;
    public float fadeDuration = 1f;

    private TMP_Text textComponent;

    void Awake()
    {
        if (SaveManager.Instance.runtimeData.currentLevel != null)
            bob = SaveManager.Instance.runtimeData.currentLevel.levelID > 0;
        else
        {
            Debug.Log("showign turorial");
            bob = false;
        }
            // Get component immediately
            textComponent = GetComponent<TMP_Text>();

        if (bob)
        {
            // Hide immediately before the frame renders
            textComponent.alpha = 0f;
        }
    }

    void Start()
    {
        // Only run the fade logic if bob is false
        if (!bob)
        {
            textComponent.DOFade(0f, fadeDuration).SetDelay(delaySeconds);
        }
    }
}