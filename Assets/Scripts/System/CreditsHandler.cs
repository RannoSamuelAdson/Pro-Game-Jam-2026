using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CreditsHandler : MonoBehaviour
{
    IDisposable disposable;

    private void Awake()
    {
        AudioManager.Instance.InitializeMusic(FMODEvents.Instance.CreditsMusic);
    }
    private void Start()
    {
        AudioManager.Instance.StartMusic();
        LevelChanger.Instance.FadeIn();
        disposable = InputSystem.onAnyButtonPress.CallOnce(ctrl => OnCreditsEnd());
        Time.timeScale = 1f;
    }
    public void OnCreditsEnd()
    {
        LevelChanger.Instance.FadeToLevel("MainMenu");
    }

    private void OnDisable()
    {
        if (disposable != null) disposable.Dispose();
    }
}
