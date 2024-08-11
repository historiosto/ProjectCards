using System.Collections;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    public static MatchingManager Instance { get; private set; }

    public int Turns { get; private set; }
    public int Matches { get; private set; }
    public int Score { get; private set; }

    private Card firstCard;  // First card selected by the player
    private Card secondCard; // Second card selected by the player
    private int currentCombo; // Tracks the current combo count

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void CardSelected(Card selectedCard)
    {
        if (firstCard == null)
        {
            firstCard = selectedCard;
        }
        else if (secondCard == null && selectedCard != firstCard)
        {
            secondCard = selectedCard;
            Turns++;

            GameManager.Instance.UpdateTurns(Turns);

            CheckForMatch();
        }
    }

    public void CheckForMatch()
    {
        if (firstCard.ReferenceID == secondCard.ReferenceID)
        {
            HandleMatch();
        }
        else
        {
            HandleMismatch();
        }
    }

    private void HandleMatch()
    {
        Matches++;
        currentCombo++;
        Score += CalculateScore();

        Debug.Log("Match! Score: " + Score);

        // Mark the cards as matched (could also trigger any match animation)
        firstCard.SetMatched();
        secondCard.SetMatched();

        // Notify BoardManager about the match
        BoardManager.Instance.CardMatched();

        // Notify GameManager about score and combo
        GameManager.Instance.UpdateScore(Score);
        GameManager.Instance.TrackCombo();
        GameManager.Instance.UpdateMatches(Matches);


        ResetTurn();
    }

    public void HandleMismatch()
    {
        Debug.Log("Mismatch! Resetting combo.");

        currentCombo = 0; // Reset combo on mismatch

        StartCoroutine(WaitThenFlipMismatchCards());

        // Notify GameManager to reset combo
        GameManager.Instance.ResetCombo();
    }

    private void ResetTurn()
    {
        firstCard = null;
        secondCard = null;
    }

    private int CalculateScore()
    {
        int baseScore = 10; // Base score for a match
        int comboMultiplier = currentCombo; // Multiplier based on current combo
        return baseScore * comboMultiplier;
    }

    public void ResetGame()
    {
        Turns = 0;
        Matches = 0;
        Score = 0;
        currentCombo = 1;

        BoardManager.Instance.ResetBoard();
    }

    public void EndGame()
    {
        GameManager.Instance.EndGame();
    }

    public IEnumerator WaitThenFlipMismatchCards()
    {
        InteractionManager.Instance.LockInput(.6f);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(firstCard.FlipCard(false));
        StartCoroutine(secondCard.FlipCard(false));
        ResetTurn();
    }
}
