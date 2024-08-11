using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;

    private int matches;
    private int turns;
    private int comboMultiplier;
    private int currentComboCount;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Initialize game state
        StartGame();
    }

    public void StartGame()
    {
        // Reset game and combo state
        LoadProgress(); // Load the saved progress (score and combo)

        UIManager.Instance.InitHUD(score,turns,matches,comboMultiplier,currentComboCount);
        BoardManager.Instance.SetupBoard();
    }

    public void EndGame()
    {
        // End the game and handle saving scores
        SaveProgress();
        UIManager.Instance.ShowEndGameUI();
    }

    public void RestartGame()
    {
        // Reset game state and restart
        // & Dekelete Progress
        StartGame();
    }

    public void UpdateTurns(int turns)
    {
        //Save Progress
        this.turns = turns;
        UIManager.Instance.UpdateTurnCount(turns);
    }

     public void UpdateMatches(int matches)
    {
       //Save Progress
       this.matches = matches;
       UIManager.Instance.UpdateMatches(matches);
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        SaveProgress(); // Save progress wheneever the score is updated
        UIManager.Instance.UpdateScore(score);
        Debug.Log("Score Updated: " + score);
    }

    public void TrackCombo()
    {
        comboMultiplier++;
        currentComboCount++;
        SaveProgress(); // Save progress whenever the combo changes
        Debug.Log("Combo Count: " + currentComboCount + " | Multiplier: " + comboMultiplier);
    }

    public void ResetCombo()
    {
        comboMultiplier = 1;
        currentComboCount = 0;
        SaveProgress(); // Save progress when the combo is reset
        Debug.Log("Combo Reset");
    }

    private void Update()
    {
        // Any logic that needs to run every frame, such as handling combo timeouts
    }

    public void SaveProgress()
    {
        // Logic for saving progress and score, e.g., using PlayerPrefs or a file system
      //  PlayerPrefs.SetInt("Score", score);
      //  PlayerPrefs.SetInt("ComboMultiplier", comboMultiplier);
      //  PlayerPrefs.SetInt("CurrentComboCount", currentComboCount);
      //  PlayerPrefs.Save();
        Debug.Log("Progress Saved");
    }

    public void LoadProgress()
    {
        // Logic for loading progress and score
        score = PlayerPrefs.GetInt("Score", 0);
        comboMultiplier = PlayerPrefs.GetInt("ComboMultiplier", 1);
        currentComboCount = PlayerPrefs.GetInt("CurrentComboCount", 0);
        matches = PlayerPrefs.GetInt("Matches", 0);
        turns = PlayerPrefs.GetInt("Turns", 0);
        Debug.Log("Progress Loaded: Score = " + score + ", ComboMultiplier = " + comboMultiplier + ", CurrentComboCount = " + currentComboCount);
    }

   
}
