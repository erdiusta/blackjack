using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // This script is for both player and dealer
    // Get other scripts
    public CardScript cardScript;
    public DeckScript deckScript;
    // Total value of player/dealer's hand
    public int handValue = 0;
    // Betting money
    int money = 1000;
    // Array of card objects on table
    public GameObject[] hand;
    // Index of next card to be turned over
    public int cardIndex = 0;
    // Tracking aces for 1 to 11 conversions
    List<CardScript> aceList = new List<CardScript>();
    [HideInInspector] public bool haveAce;
    [HideInInspector] public bool haveBigs;
    [HideInInspector] public bool haveSplit;
    GameManager gameManager;
    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    // Add a hand to the player/dealer's hand
    public int GetCard()
    {
        // Get a card
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        // Show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        // Add card value to running total of the hand

        handValue += cardValue;
        // If value is 1, it is an ace
        if(cardValue == 1 || cardValue == 11) 
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
            if(cardIndex == 1)
            {
                haveAce = true;
            }
        }
        if(cardValue == 10)
        {
            haveBigs = true;
        }
        // Check if we should use an 11 instead of a 1
        AceCheck();
        cardIndex++;
        return handValue;
    }

    // Search for needed ace conversions, 1 to 11 or vice versa
    public void AceCheck()
    {
        // for each ace in the list check   
        foreach(CardScript ace in aceList)
        {
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                // if converting, adjust card object value and hand
                ace.SetValue(11);
                handValue += 10;
            }
            else if(handValue > 21 && ace.GetValueOfCard() == 11) 
            {
                // if converting, adjust game object value and hand value
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    // Add or subtract from money, for bets
    public void AdjustMoney(int amount) 
    {
        money += amount;
    }

    public void AdjustValue(int handValue) 
    {
        handValue -= handValue;
    }

    // Output players current money amount
    public int GetMoney()
    {
        return money;
    }

    // Hides all cards, resets the needed variables
    public void ResetHand()
    {
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<CardScript>().enabled = false;
        }
    }
}
