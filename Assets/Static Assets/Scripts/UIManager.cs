using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    public TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI matchesCountText;
    public TextMeshProUGUI turnCountText;

    [Space]
    [Header("Gameplay Messages and Panels")]
    public TextMeshProUGUI comboMultiplierText;
    public GameObject gameOverTextObject;

    public GameObject menuPanel;


    [Space]
    [Header("Animators")]
    public Animator comboTextAnimator;
    public Animator cardsMatchedTextAnimator;
    

    
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

    public void UpdateScore(int score)
    {
        scoreCountText.text = score.ToString();
    }

    public void UpdateTurnCount(int turns)
    {
        turnCountText.text = turns.ToString();
    }

    public void UpdateMatches(int matches)
    {
        
        cardsMatchedTextAnimator.SetTrigger("Activate");
        matchesCountText.text = matches.ToString();   
    }

    public void ShowEndGameUI()
    {
        menuPanel.SetActive(true);
        gameOverTextObject.SetActive(true);
    }

    public void InitUI(int score, int turns, int matches, int comboMultiplier, int currentComboCount)
    {
        comboTextAnimator.SetBool("Activated",false);
        matchesCountText.text = matches.ToString();
        turnCountText.text = turns.ToString();
        scoreCountText.text = score.ToString();   

    }

    public void UpdateCombo(int comboCount, int comboMultiplier)
    {
        comboMultiplierText.text = "x"+comboMultiplier.ToString();


        comboTextAnimator.SetBool("Activated",comboMultiplier > 1);
        

    }
}
