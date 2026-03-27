using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float minutesOfLevel; //In Minutes
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;

    private void Start()
    {
        timeRemaining = minutesOfLevel * 60f; //Converting minutes to seconds
    }

    private void Update()
    {
        if (timeRemaining <= 0f) return;

        timeRemaining -= Time.deltaTime;

        UpdateUI();

        if (timeRemaining <= 10f)
        {
            timerText.color = Color.red;
        }

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            GameOver();
        }
    }

    private void GameOver()
    {
        timerText.gameObject.SetActive(false);

        gameOverScreen.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
