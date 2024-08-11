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
        //StartGame();
    }

    public void StartGame()
    {
        score = 0;
        turns = 0;
        matches = 0;
        comboMultiplier = 1;
        currentComboCount = 0;
        UIManager.Instance.InitHUD(score,turns,matches,comboMultiplier,currentComboCount);
        BoardManager.Instance.SetupBoard();
    }

    public void EndGame()
    {
        UIManager.Instance.ShowEndGameUI();
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void DeleteSavesAndRestart()
    {
        PlayerPrefs.DeleteAll();

        BoardManager.Instance.ClearBoard();

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
        
        UIManager.Instance.UpdateScore(score);
        Debug.Log("Score Updated: " + score);
    }

    public void TrackCombo()
    {
        comboMultiplier++;
        currentComboCount++;
        
        Debug.Log("Combo Count: " + currentComboCount + " | Multiplier: " + comboMultiplier);
    }

    public void ResetCombo()
    {
        comboMultiplier = 1;
        currentComboCount = 0;
       
        Debug.Log("Combo Reset");
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("ComboMultiplier", comboMultiplier);
        PlayerPrefs.SetInt("CurrentComboCount", currentComboCount);
        PlayerPrefs.SetInt("Matches", matches);
        PlayerPrefs.SetInt("Turns", turns);
        PlayerPrefs.Save();
        Debug.Log("Progress Saved");
    
    }

    public void LoadProgress()
    {
       
        score = PlayerPrefs.GetInt("Score", 0);
        comboMultiplier = PlayerPrefs.GetInt("ComboMultiplier", 1);
        currentComboCount = PlayerPrefs.GetInt("CurrentComboCount", 0);
        matches = PlayerPrefs.GetInt("Matches", 0);
        turns = PlayerPrefs.GetInt("Turns", 0);
        UIManager.Instance.InitHUD(score,turns,matches,comboMultiplier,currentComboCount);

        Debug.Log("Progress Loaded: Score = " + score + ", ComboMultiplier = " + comboMultiplier + ", CurrentComboCount = " + currentComboCount);
    }

        public void SaveGame()
    {
        BoardManager.Instance.SaveBoardState();
        SaveProgress(); 
        Debug.Log("Game state saved.");
    }

    public void LoadGame()
    {
        LoadProgress(); 
        BoardManager.Instance.LoadBoardState();
        Debug.Log("Game state loaded.");
    }
   
}
