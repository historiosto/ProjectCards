using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    public TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI matchesCountText;
    public TextMeshProUGUI turnCountText;


    
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
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
        matchesCountText.text = matches.ToString();   
    }

    public void ShowEndGameUI()
    {

    }

    public void InitHUD(int score, int turns, int matches, int comboMultiplier, int currentComboCount)
    {
        matchesCountText.text = matches.ToString();
        turnCountText.text = turns.ToString();
        scoreCountText.text = score.ToString();   

    }
}
