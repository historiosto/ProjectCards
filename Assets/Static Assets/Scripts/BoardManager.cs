using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    [SerializeField, Range(2, 6)]
    public int Columns = 4;

    [SerializeField, Range(2, 4)]
    public int Rows = 4;
    public GameObject CardPrefab;
    public GameObject cardboard;

    private List<Card> cardsOnBoard = new List<Card>();
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
    }

    public void SetupBoard()
    {
        ConfigureGridLayout();

        ClearBoard();

        int totalCards = Columns * Rows;

        if (totalCards % 2 != 0)
        {
            totalCards -= 1;
        }

        GenerateCards(totalCards);
        ShuffleCards();

        remainingCards = totalCards;
    }

    private void ConfigureGridLayout()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = Columns;
    }

    private void GenerateCards(int totalCards)
    {
        for (int i = 0; i < totalCards; i += 2)
        {
            string cardSuit = Card.GetRandomSuit();

            for (int j = 0; j < 2; j++)
            {
                GameObject newCard = Instantiate(CardPrefab, cardboard.transform);
                Card cardComponent = newCard.GetComponent<Card>();
                cardsOnBoard.Add(cardComponent);

                int referenceID = i / 2;
                cardComponent.SetReferenceID(referenceID + 1, cardSuit);
            }
        }
    }

    public void ShuffleCards()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogError("Card parent does not have a GridLayoutGroup component.");
            return;
        }

        List<Transform> cardTransforms = new List<Transform>();
        foreach (Transform child in cardboard.transform)
        {
            cardTransforms.Add(child);
        }

        for (int i = 0; i < cardTransforms.Count; i++)
        {
            int randomIndex = Random.Range(i, cardTransforms.Count);
            Transform temp = cardTransforms[i];
             cardTransforms[i] = cardTransforms[randomIndex];
            cardTransforms[randomIndex] = temp;
        }

        for (int i = 0; i < cardTransforms.Count; i++)
        {
             cardTransforms[i].SetSiblingIndex(i);
        }

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
        MatchingManager.Instance.ResetCalculations();

        foreach (Card card in cardsOnBoard)
        {
            
            Destroy(card.gameObject);
        }

        cardsOnBoard.Clear();
    }

    public void SaveBoardState()
    {
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            Card card = cardsOnBoard[i];
             string cardKey = "Card_" + i;

            PlayerPrefs.SetInt(cardKey + "_ID", card.ReferenceID);
            PlayerPrefs.SetString(cardKey + "_Suit", card.cardSuit);
           
            PlayerPrefs.SetInt(cardKey + "_Matched", card.IsMatched ? 1 : 0);
            PlayerPrefs.SetInt(cardKey + "_Position", card.transform.GetSiblingIndex());
        }

        PlayerPrefs.SetInt("RemainingCards", remainingCards);
        
         PlayerPrefs.SetInt("TotalCards", cardsOnBoard.Count);
        PlayerPrefs.Save();

    }

    public void LoadBoardState()
    {
        ClearBoard();

        int totalCards = PlayerPrefs.GetInt("TotalCards", 0);
        remainingCards = PlayerPrefs.GetInt("RemainingCards", totalCards);

        for (int i = 0; i < totalCards; i++)
        {
            string cardKey = "Card_" + i;
            int referenceID = PlayerPrefs.GetInt(cardKey + "_ID");
            string suit = PlayerPrefs.GetString(cardKey + "_Suit");
            bool isMatched = PlayerPrefs.GetInt(cardKey + "_Matched") == 1;
            int position = PlayerPrefs.GetInt(cardKey + "_Position");

            GameObject newCard = Instantiate(CardPrefab, cardboard.transform);
            Card cardComponent = newCard.GetComponent<Card>();
            cardsOnBoard.Add(cardComponent);

            cardComponent.SetReferenceID(referenceID, suit);

            if (isMatched)
            {
                cardComponent.SetMatched();
            }

            newCard.transform.SetSiblingIndex(position);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardboard.GetComponent<RectTransform>());

    }
}
