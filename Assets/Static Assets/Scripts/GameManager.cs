using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Game State Variables
    private int score;
    private int comboMultiplier;
    private int currentComboCount;

    // Combo System Variables
    private float comboTimer;
    private const float comboDuration = 5.0f;  // Time window to continue a combo
    private bool isComboActive;

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
        score = 0;
        comboMultiplier = 1;
        currentComboCount = 0;
        isComboActive = false;

        // Initialize other game components
        // E.g., BoardManager.Instance.SetupBoard();
    }

    public void EndGame()
    {
        // End the game and handle saving scores
        SaveProgress();
        // Additional end game logic, e.g., show game over screen
    }

    public void RestartGame()
    {
        // Reset game state and restart
        StartGame();
    }

    public void UpdateScore(int basePoints)
    {
        // Calculate score with combo multiplier
        int pointsEarned = basePoints * comboMultiplier;
        score += pointsEarned;
        Debug.Log("Score Updated: " + score);
    }

    public void TrackCombo()
    {
        if (!isComboActive)
        {
            isComboActive = true;
            comboTimer = comboDuration;
        }
        else
        {
            comboMultiplier++;
            currentComboCount++;
            Debug.Log("Combo Count: " + currentComboCount + " | Multiplier: " + comboMultiplier);
        }
    }

    public void ResetCombo()
    {
        isComboActive = false;
        comboMultiplier = 1;
        currentComboCount = 0;
        Debug.Log("Combo Reset");
    }

    private void Update()
    {
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void SaveProgress()
    {
        // Logic for saving progress and score, e.g., using PlayerPrefs or a file system
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        Debug.Log("Progress Saved");
    }

    public void LoadProgress()
    {
        // Logic for loading progress and score
        score = PlayerPrefs.GetInt("Score", 0);
        Debug.Log("Progress Loaded: Score = " + score);
    }
}
