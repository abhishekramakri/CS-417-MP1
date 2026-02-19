using UnityEngine;
using TMPro;

public class EscapeRoomManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Text displaying progress (e.g. 'Systems Restored: 1/3')")]
    public TextMeshProUGUI scoreboardText;

    [Tooltip("Text displaying the countdown timer")]
    public TextMeshProUGUI timerText;

    [Tooltip("GameObject containing the Win message and effects")]
    public GameObject winDisplay;

    [Header("Settings")]
    [Tooltip("Total time in seconds before the player loses")]
    public float totalTime = 300f;

    [Tooltip("Total number of gates required to win")]
    public int totalGates = 3;

    [Header("Win Celebration Objects")]
    [Tooltip("Objects to enable when the player wins (e.g. 'You Win' text, fireworks)")]
    public GameObject[] winObjects;

    [Tooltip("Objects to disable when the player wins (e.g. barriers, red lights)")]
    public GameObject[] disableOnWin;

    private int gatesUnlocked = 0;
    private float timeRemaining;
    private bool gameOver = false;
    public bool gameWon { get; private set; } = false;

    void Start()
    {
        timeRemaining = totalTime;

        if (winDisplay != null)
            winDisplay.SetActive(false);

        foreach (var obj in winObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        UpdateScoreboard();
        UpdateTimer();
    }

    void Update()
    {
        if (gameOver) return;

        // Countdown timer
        timeRemaining -= Time.deltaTime;
        UpdateTimer();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameLost();
        }
    }

    public void OnGateUnlocked()
    {
        if (gameOver) return;

        gatesUnlocked++;
        UpdateScoreboard();

        if (gatesUnlocked >= totalGates)
        {
            GameWon();
        }
    }

    void UpdateScoreboard()
    {
        if (scoreboardText != null)
            scoreboardText.text = "Systems Restored: " + gatesUnlocked + " / " + totalGates;
    }

    void UpdateTimer()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = "O2 Remaining: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    void GameWon()
    {
        gameOver = true;
        gameWon = true;

        // Show win display
        if (winDisplay != null)
            winDisplay.SetActive(true);

        // Enable win celebration objects
        foreach (var obj in winObjects)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Disable barriers etc.
        foreach (var obj in disableOnWin)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void GameLost()
    {
        gameOver = true;

        if (timerText != null)
            timerText.text = "O2 DEPLETED - GAME OVER";
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
