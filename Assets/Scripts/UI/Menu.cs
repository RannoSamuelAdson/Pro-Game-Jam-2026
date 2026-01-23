using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button settingsButton;
    public Button creditsButton;
    public Button backButton;
    public Button quitButton;
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;
    public GameObject Overlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        backButton.onClick.AddListener(CloseMenu);
        quitButton.onClick.AddListener(Quit);
    }

    private void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void OpenCredits()
    {
        CreditsPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void Quit()
    {
        Application.Quit();
    }

    private void CloseMenu()
    {
        Debug.Log("Closing menu");
        Overlay.gameObject.SetActive(false);
        Player.SetGamePaused(false);

    }
    // Update is called once per frame
    void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }
}
