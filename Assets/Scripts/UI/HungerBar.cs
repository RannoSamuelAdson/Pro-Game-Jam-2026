using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider bar;                       // UI Slider
    public Image fill;                       // fill image of slider
    public Color lowColor = Color.red;
    public Color midColor = Color.yellow;
    public Color highColor = Color.green;
    public GameObject restartPanel; // UI panel or button that appears when time runs out
    public Button quitButton;
    public Button restartButton;

    public float maxValue = 70f;             // Fully full bar
    public float currentValue = 30f;         // Start full

    public float drainRate = 5f;             // Drains X units per second

    private void Start()
    {
        quitButton.onClick.AddListener(Quit); 
        restartButton.onClick.AddListener(Restart);
    }
    void Update()
    {
        if (Player.playerPaused) return;

        // **Drain** over time
        currentValue -= drainRate * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0f, maxValue);

        UpdateUI();
        if (currentValue <= 0f) EndTimer();

    }

    public void AddNutrition(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);
        Debug.Log(amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        bar.value = currentValue / maxValue;

        float pct = bar.value; // 0–1

        // Color blend: red ? yellow ? green
        if (pct < 0.5f)
        {
            // red ? yellow
            fill.color = Color.Lerp(lowColor, midColor, pct * 2f);
        }
        else
        {
            // yellow ? green
            fill.color = Color.Lerp(midColor, highColor, (pct - 0.5f) * 2f);
        }
    }

    void EndTimer()
    {
        restartPanel.SetActive(true); // Show restart UI
        //Player.playerPaused = true;
        //qteManager.gameObject.SetActive(false);
        Player.SetGamePaused(true);
        Debug.Log("Time's up! Game paused.");
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Player.SetGamePaused(false);
        restartPanel.SetActive(false);
    }
    private void Quit() { Application.Quit(); }
    private void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
