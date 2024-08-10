using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int ReferenceID { get; private set; }  // Unique identifier for the card to match with others
    public Sprite FrontImage;                     
    public Sprite BackImage;                      

    private bool isFlipped = false;
    private bool isMatched = false;

    private Image cardImage; // The UI image component on the card                      
    private Button cardButton;                    

    private void Awake()
    {
        cardImage = GetComponent<Image>();
        cardButton = GetComponent<Button>();
        
        cardImage.sprite = BackImage;
       
    }

    public void OnCardClicked()
    {
        Debug.Log("Card Clicked!");

        if (isFlipped || isMatched || InteractionManager.Instance == null) return;

        if(InteractionManager.Instance.isInputLocked) return;

        Debug.Log("Card Selected!");
       
        StartCoroutine(FlipCard(true));

        MatchingManager.Instance.CardSelected(this);
    }

    // This method flips the card
    public IEnumerator FlipCard(bool showFront)
    {
        yield return new WaitForSeconds(0.15f); //  flip duration

        // Set the card's image to either the front or back
        cardImage.sprite = showFront ? FrontImage : BackImage;

        isFlipped = showFront;
    }

    public void SetMatched()
    {
        isMatched = true;

        // animation here 
        // sound effect

        // Disable the card's button to prevent further interaction
        cardButton.interactable = false;
    }

    // This method resets the card to its initial state
    public void ResetCard()
    {
        isFlipped = false;
        isMatched = false;

        cardImage.sprite = BackImage;

        // Reenable interaction
        cardButton.interactable = true;
    }

    public void SetReferenceID(int id)
    {
        ReferenceID = id;
    }

  

    
}
