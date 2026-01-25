using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class EndPanelScript : MonoBehaviour
{
    IDisposable inputListener;
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        switch(SaveManager.Instance.runtimeData.currentLevel.levelID)
        {
            case 1:
                text.text = "Survived the first night...\r\nWhat will the next bring?\r\n\r\nPress any key to continue.";
                break;
            case 2:
                text.text = "Survived the second night...\r\nWhen will they stop?\r\n\r\nPress any key to continue.";
                break;
            default:
                text.text = "Survived the third...? night...\r\nIf you see this, please report it to the developer <3\r\n\r\nPress any key to continue.";
                Debug.LogWarning($"Unknown Level ID {SaveManager.Instance.runtimeData.currentLevel.levelID}!");
                break;
        }
        // Delay enabling the input by 0.5 seconds to prevent misclicks
        Invoke(nameof(EnableInput), 0.5f);
    }

    void EnableInput()
    {
        // Start listening for any button press
        inputListener = InputSystem.onAnyButtonPress.CallOnce(ctrl => LoadNextLevel());
    }

    void LoadNextLevel()
    {
        // Call your level changer here
        LevelChanger.Instance.FadeToLevel(SceneManager.GetActiveScene().name, false);
    }

    private void OnDisable()
    {
        // Clean up the listener if the object is destroyed/disabled
        if (inputListener != null) inputListener.Dispose();
    }
}