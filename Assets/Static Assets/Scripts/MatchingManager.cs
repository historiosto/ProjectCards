using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MatchingManager : MonoBehaviour
{
    public static MatchingManager Instance { get; private set; }

    public int Turns { get; private set; }
    public int Matches { get; private set; }
    public int Score { get; private set; }

    public List<AudioClip> ComboSounds = new List<AudioClip>();

    private AudioSource matchingCardsAudioSource;

    private Card firstCard;  // First card selected by the player
    private Card secondCard; // Second card selected by the player
    private int comboMultiplier; // Tracks the combo multiplier
    private int currentComboCount; // Tracks the current combo count

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

        matchingCardsAudioSource = GetComponent<AudioSource>();
    }

    public void InitializeGame(int initialScore, int initialComboMultiplier, int initialComboCount)
    {
        Score = initialScore;
        comboMultiplier = initialComboMultiplier;
        currentComboCount = initialComboCount;
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
        
        Score += CalculateScore();

        currentComboCount++;


       


        //We Increment the ComboMultiplier with every Match!
        if(comboMultiplier < 4)
            comboMultiplier++;
        

        firstCard.SetMatched();
        secondCard.SetMatched();

        // We Notify BoardManager about the match
        BoardManager.Instance.CardMatched();

        // We Update UI via GameManager
        GameManager.Instance.UpdateScore(Score);
        GameManager.Instance.UpdateCombo(currentComboCount, comboMultiplier);
        GameManager.Instance.UpdateMatches(Matches);

        PlaySound();

        ResetTurn();
    }

    private void HandleMismatch()
    {
        Debug.Log("Mismatch! Resetting combo.");

        currentComboCount = 0; 
        comboMultiplier = 1;

        StartCoroutine(WaitThenFlipMismatchCards());

        GameManager.Instance.UpdateCombo(currentComboCount, comboMultiplier);
    }

    private void ResetTurn()
    {
        firstCard = null;
        secondCard = null;
    }

    private int CalculateScore()
    {
        int baseScore = 10; // Base score for a match
        return baseScore * comboMultiplier;
    }

    public void ResetCalculations()
    {
        Turns = 0;
        Matches = 0;
        Score = 0;
        currentComboCount = 0;
        comboMultiplier = 1;
    }

    public IEnumerator WaitThenFlipMismatchCards()
    {
        InteractionManager.Instance.LockInput(.6f);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(firstCard.FlipCard(false));
        StartCoroutine(secondCard.FlipCard(false));
        ResetTurn();
    }


    private void PlaySound()
    {
        try
        {
             switch (currentComboCount)
            {
                case 1:
                    matchingCardsAudioSource.PlayOneShot(ComboSounds[0]);
                    break;
                case 2:
                    matchingCardsAudioSource.PlayOneShot(ComboSounds[1]);
                    break;
                default:
                    if (currentComboCount > 2)
                    {
                        matchingCardsAudioSource.PlayOneShot(ComboSounds[2]);
                    }
                break;
            }
        }
        catch (System.Exception e)
        {
            
            Debug.LogWarning("" + e.Message);
        }
       
    }
}
