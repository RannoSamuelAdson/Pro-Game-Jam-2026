using UnityEngine;
using UnityEngine.UI;
public enum TutorialState
{
    StartPanel,
    MoveTutorial,
    MinigameTutorial,
    ReturnTutorial,
    TutorialOver
}
public class Tutorial : MonoBehaviour
{
    public Button startGameButton;
    public GameObject startPanel;
    public GameObject moveTutuorial;
    public GameObject minigameTutorial;
    public GameObject returnTutorial;
    public Timer timer;
    public Nest nest;
    public GameObject Collider;
    private TutorialState tutorialState = TutorialState.StartPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startGameButton.onClick.AddListener(StartTutorial);
        Player.SetGamePaused(true);
    }
    private void StartTutorial()
    {
        startPanel.gameObject.SetActive(false);
        Player.SetGamePaused(false);
        tutorialState = TutorialState.MoveTutorial;
        moveTutuorial.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        switch (tutorialState)
        {
            case TutorialState.MoveTutorial:
                {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) 
                    { 
                    moveTutuorial.SetActive(false);
                    //findPeopleTutorial.SetActive(true);
                    tutorialState = TutorialState.MinigameTutorial;
                    Collider.SetActive(false);
                    }

                

                break;
            }
            /*case TutorialState.FindPeopleTutorial:
            {
                Human[] humans1 = FindObjectsByType<Human>(FindObjectsSortMode.None);
                foreach (Human human in humans1) 
                {
                    human.setTutorialActiveState(true);
                }
                if (QTEmanager.gameObject.activeSelf) { 
                    minigameTutorial.SetActive(true);
                    tutorialState = TutorialState.MinigameTutorial;
                    Human[] humans2 = FindObjectsByType<Human>(FindObjectsSortMode.None);
                    foreach (Human human in humans2)
                    {
                        human.setTutorialActiveState(false);
                    }}
                  break;
            }*/
            case TutorialState.MinigameTutorial:
            {
                if (Input.GetKeyDown(KeyCode.Space)) { 
                    minigameTutorial.SetActive(false);
                    returnTutorial.SetActive(true);
                    tutorialState = TutorialState.ReturnTutorial;
                    }
                break;
            }
            case TutorialState.ReturnTutorial:
            {
                if (nest.netWorth > 0) { 
                    returnTutorial.SetActive(false);
                    tutorialState = TutorialState.TutorialOver;}
                break;
            }

            default: 
                break;
        }
    }
}
