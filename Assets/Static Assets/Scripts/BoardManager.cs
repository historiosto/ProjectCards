using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public int Columns = 4;  
    public int Rows = 4;     
    public GameObject CardPrefab;  // Prefab for the card object
    
    public GameObject cardboard;
    private List<Card> cardsOnBoard = new List<Card>();  // List to store all the cards on the board
    private int remainingCards;  

    private GridLayoutGroup gridLayoutGroup;

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

        gridLayoutGroup = cardboard.GetComponent<GridLayoutGroup>();

    }

    private void Start()
    {
        SetupBoard();
    }

    public void SetupBoard()
    {

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = Columns;

        // Clear any existing cards from the previous game
        foreach (Transform child in cardboard.transform)
        {
            Destroy(child.gameObject);
        }
       
        int totalCards = Columns * Rows;

         if (totalCards % 2 != 0)
        {
            totalCards -= 1;
        }


        // Instantiate the cards and add them to the grid
        for (int i = 0; i < totalCards; i++)
        {
            GameObject newCard = Instantiate(CardPrefab, cardboard.transform);
            Card cardComponent = newCard.GetComponent<Card>();
            cardsOnBoard.Add(cardComponent);
            int referenceID = i / 2;
            cardComponent.SetReferenceID(referenceID);
            
            // Initialize the card (set reference ID, image, etc.)
            //cardComponent.InitializeCard(i % (Columns * Rows / 2)); 
        }
     
       
        // Notify other systems that the board is ready

        ShuffleCards();

        // Update the remaining cards counter
        remainingCards = totalCards;
       
    }

  public void ShuffleCards()
    {
        GridLayoutGroup gridLayoutGroup = cardboard.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            Debug.LogError("Card parent does not have a GridLayoutGroup component.");
            return;
        }

        // Get all child objects (cards) of the parent GameObject
        List<Transform> cardTransforms = new List<Transform>();
        foreach (Transform child in cardboard.transform)
        {
            cardTransforms.Add(child);
        }

        // Shuffle the list of card transforms
        for (int i = 0; i < cardTransforms.Count; i++)
        {
            Transform temp = cardTransforms[i];
            int randomIndex = Random.Range(i, cardTransforms.Count);
            cardTransforms[i] = cardTransforms[randomIndex];
            cardTransforms[randomIndex] = temp;
        }

        // Reposition the cards according to the new shuffled order
        for (int i = 0; i < cardTransforms.Count; i++)
        {
            cardTransforms[i].SetSiblingIndex(i);
        }

        // layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(cardboard.GetComponent<RectTransform>());
    }

    public void CardMatched()
    {
        remainingCards -= 2;

        if (remainingCards <= 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void ResetBoard()
    {
        foreach (Card card in cardsOnBoard)
        {
            card.ResetCard();
        }

        ShuffleCards();
        remainingCards = cardsOnBoard.Count;
    }

    public void ClearBoard()
    {
        foreach (Card card in cardsOnBoard)
        {
            Destroy(card.gameObject);
        }

        cardsOnBoard.Clear();
    }
}
