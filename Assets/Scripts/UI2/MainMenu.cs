using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup optionsMenuCG;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;
    private GameObject lastSelect;
    void Start()
    {
        if (BuildConsts.isMobile || BuildConsts.isWebGL) quitButton.SetActive(false);
        SaveManager.Instance.runtimeData.previousSceneName = SceneManager.GetActiveScene().name;
        LevelChanger.Instance.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelect);
        }
        else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void OnPlayPressed()
    {
        LevelChanger.Instance.FadeToLevel("MainScene");
    }

    public void OnQuitPressed()
    {
        LevelChanger.Instance.FadeToDesktop();
    }

    public void OnOptionsPressed()
    {
        UICommon.FadeInCG(optionsMenuCG, 0.2f);
        EventSystem.current.SetSelectedGameObject(optionsMenuCG.transform.GetChild(0).GetChild(1).gameObject);
    }

    public void OnLeaveSettings()
    {
        UICommon.FadeOutCG(optionsMenuCG, 0.2f);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void OnCreditsPressed()
    {
        LevelChanger.Instance.FadeToLevel("Credits");
    }
}
