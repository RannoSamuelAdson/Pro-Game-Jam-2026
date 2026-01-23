using UnityEngine;
using UnityEngine.UI;

public class OpenMenuButton : MonoBehaviour
{
    public GameObject Menu;
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OpenMenu);
    }
    private void OpenMenu()
    {
        Debug.Log("Opening Menu");
        Menu.SetActive(true);
        Player.SetGamePaused(true);
    }
    // Update is called once per frame
    void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && !Menu.activeSelf)
        {
            OpenMenu();
        }
    }
}
