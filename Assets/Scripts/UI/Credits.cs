using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Button backButton;
    public GameObject mainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(CloseCredits);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CloseCredits()
    {
        this.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
