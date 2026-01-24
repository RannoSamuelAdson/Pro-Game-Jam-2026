using FMODUnity;
using System.Collections;
using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    private int targetScore;
    private int previousTransitionScore;
    private int currentParam;
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        //LevelProgressHandler.UpdateProgressTarget += UpdateTarget;
        //PlayerScore.OnScoreUpdated += CheckScore;
        LevelChanger.OnGameplayLevelLoaded += SetupLevel;
    }

    private void OnDisable()
    {
        //LevelProgressHandler.UpdateProgressTarget -= UpdateTarget;
        //PlayerScore.OnScoreUpdated -= CheckScore;
        LevelChanger.OnGameplayLevelLoaded -= SetupLevel;
    }

    private void CheckScore(int score)
    {
        if ((score - previousTransitionScore) >= targetScore)
        {
            UpdateParameters();
            previousTransitionScore = score;
        }
    }

    private void UpdateTarget(int val)
    {
        targetScore = val / 5;
    }

    private void SetupLevel(LevelData data)
    {
        InitializeMusic(data.levelTheme);
    }

    private void InitializeMusic(EventReference song)
    {
        AudioManager.Instance.InitializeMusic(song);
        AudioManager.Instance.StartMusic();
    }

    // code from prev game for fading in parameters
    private void UpdateParameters()
    {
        if (currentParam > 4) return;

        switch (currentParam)
        {
            case 0:
                //AudioManager.Instance.SetMusicParameter("MiscTrack", 1f);
                FadeParameter("MiscTrack", 0.3f);
                break;
            case 1:
                //AudioManager.Instance.SetMusicParameter("TambTrack", 1f);
                FadeParameter("TambTrack", 0.3f);
                break;
            case 2:
                //AudioManager.Instance.SetMusicParameter("RoboTrack", 1f);
                FadeParameter("RoboTrack", 0.3f);
                break;
            case 3:
                //AudioManager.Instance.SetMusicParameter("CoinTrack", 1f);
                FadeParameter("CoinTrack", 0.3f);
                break;
            case 4:
                //AudioManager.Instance.SetMusicParameter("GuitarTrack", 1f);
                FadeParameter("GuitarTrack", 0.3f);
                break;
        }
        currentParam++;
    }

    public void FadeParameter(string parameterName, float duration)
    {
        StartCoroutine(FadeCoroutine(parameterName, duration));
    }

    private IEnumerator FadeCoroutine(string parameterName, float duration)
    {
        float elapsedTime = 0f;
        float startValue = 0f;
        float endValue = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            AudioManager.Instance.SetMusicParameter(parameterName, currentValue);
            yield return null;
        }

        // Ensure the final value is set precisely
        AudioManager.Instance.SetMusicParameter(parameterName, endValue);
    }
}
