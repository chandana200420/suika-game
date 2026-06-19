using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI bestScoreText;

    private int score = 0;
    private int bestScore = 0;

    private const string BestScoreKey = "BestScore";
    private const string FirstTimeKey = "IsFirstTime";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Check new user / old user
        bool isFirstTime = PlayerPrefs.GetInt(FirstTimeKey, 0) == 0;

        if (isFirstTime)
        {
            // New user
            bestScore = 0; // or set any default
            PlayerPrefs.SetInt(FirstTimeKey, 1);  // Mark user as "old" now
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
            Debug.Log("New user detected. Best score starting fresh.");
        }
        else
        {
            // Old user
            bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
            Debug.Log("Old user detected. Loaded best score = " + bestScore);
        }

        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "SCORE: " + score;
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = "BEST SCORE: " + bestScore;
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }
}
