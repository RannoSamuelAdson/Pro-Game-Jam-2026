using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float SurvivalTime { get; private set; } = 0f;

    void Start()
    {
        UpdateTimerDisplay(0f);
    }

    void Update()
    {
        if (!Player.playerPaused)
        {
            SurvivalTime += Time.deltaTime;
            UpdateTimerDisplay(SurvivalTime);
        }
    }

    void UpdateTimerDisplay(float time)
    {
        time = Mathf.Max(0, time);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
