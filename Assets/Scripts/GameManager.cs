using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    public Button doubleBtn;
    public Button splitBtn;
    public Button insuranceBtn;

    int standClicks = 0;
    [HideInInspector] public int index1value;

    // Access the player's and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // public text to access and update - hud
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dealerScoreText;
    public TextMeshProUGUI betsText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI standBtnText;
    public TextMeshProUGUI insuranceText;
    // Card hiding dealer's 2nd card
    public GameObject hideCard;
    [HideInInspector] public bool roundOver;
    // How much is bet
    [HideInInspector] public int pot = 0;
    public bool blackjack;
    public bool dealClicked;
    public bool betClicked;
    public bool hitClicked;
    public bool doubleBtnClicked;
    public bool splitBtnClicked;
    public bool insuranceBtnClicked;
    public bool standBtnClicked;

    float timer;
    
    void Start()
    {
        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        doubleBtn.onClick.AddListener(() => DoubleClicked());
        splitBtn.onClick.AddListener(() => SplitClicked());
        insuranceBtn.onClick.AddListener(() => InsuranceClicked());
    }
    public void DealClicked()
    {
        if(betClicked)
        {
            dealClicked = true;
            blackjack = false;
            // Play sound effect
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayDealCardClip();
            // Reset round, hide text, prep for new hand
            playerScript.ResetHand();
            dealerScript.ResetHand();
            // Hide deal hand score at start of deal
            mainText.gameObject.SetActive(false);
            dealerScoreText.gameObject.SetActive(false);
            GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
            playerScript.StartHand();
            dealerScript.StartHand();
            // Update the score displayed
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            // Enable to hide one of the dealer's cards
            hideCard.GetComponent<Renderer>().enabled = true;
            // Adjust buttons visibility
            if(dealerScript.cardIndex == 2 && dealerScript.haveAce)
            {
                insuranceBtn.gameObject.SetActive(true);
            }
            if(playerScript.GetComponentsInChildren<CardScript>()[0].type ==
                playerScript.GetComponentsInChildren<CardScript>()[1].type)
            {
                splitBtn.gameObject.SetActive(true);

            }
            doubleBtn.gameObject.SetActive(true);
            betBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(false);
            hitBtn.gameObject.SetActive(true);
            standBtn.gameObject.SetActive(true);
            standBtnText.text = "Stand";
            // Set standard pot size
            betsText.text = "Bets: $" + pot.ToString();
            cashText.text = "$" + playerScript.GetMoney().ToString();
            //Finish the round if black jack
            if(playerScript.handValue == 21 && playerScript.cardIndex == 2)
            {
                RoundOver();
            }
        }
    }

    public void HitClicked() 
    {
        if(doubleBtnClicked)
        {
            hitBtn.gameObject.SetActive(false);
        }
        hitClicked = true;
        doubleBtn.gameObject.SetActive(false);
        // Play sound effect
        GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayHitClip();
        // Check that there is still room on the table
        if(playerScript.cardIndex <= 10)
        {
            if(splitBtnClicked)
            {
                bool firstSplitActive = true;
                if (firstSplitActive) 
                {
                    index1value = playerScript.GetComponentsInChildren<CardScript>()[1].value;
                    playerScript.GetCard();
                    scoreText.text = "Hand: " + (playerScript.handValue - index1value).ToString();
                    if (playerScript.handValue > 20)
                    {
                        firstSplitActive = false;
                        RoundOver();
                    }
                }
            }
            else
            {
                playerScript.GetCard();
                scoreText.text = "Hand: " + playerScript.handValue.ToString();
                if(playerScript.handValue > 20) 
                {
                    RoundOver();
                }
            }
        }
    }
    public void StandClicked()
    {
        standClicks++;
        standBtnClicked = true;
        if(standClicks > 1) 
        {
            RoundOver();
        }
        HitDealer();
        standBtnText.text = "Call";
        // Play sound effect
        GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayStandClip();
    }

    void HitDealer()
    {
        while(dealerScript.handValue < 17 && dealerScript.cardIndex < 10 && !blackjack)
        {
            float duration = 1f;
            timer += Time.deltaTime;
            if(duration < timer)
            {
                dealerScript.GetCard();
                timer = 0;
            }
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if(dealerScript.handValue > 20) 
            {
                RoundOver();
            }
        }
    }

    // Check for winner and loser, hand is over
    void RoundOver()
    {
        // Booleans for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if(standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        roundOver = true;
        // All bust, bets returned
        if(playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
            blackjack = true;
        }
        // if both player and dealer has blackjack, it is tie
        else if(player21 && playerScript.cardIndex == 2 && 
                dealer21 && dealerScript.cardIndex == 2)
        {
            mainText.text = "Push";
            playerScript.AdjustMoney(pot/2);
            blackjack = true;
        }
        // if player has blackjack, player wins
        else if(player21 && playerScript.cardIndex == 2)
        {        
            mainText.text = "BLACKJACK!♠ Player wins!";
            playerScript.AdjustMoney(3 * pot/2);
            blackjack = true;
        }
        // if dealer has blackjack, dealer wins
        else if(dealer21 && dealerScript.cardIndex == 2)
        {
            if(insuranceBtnClicked)
            {
                playerScript.AdjustMoney(pot);
                cashText.text = "$" + playerScript.GetMoney().ToString();
                mainText.text = "Dealer has BLACKJACK!♠ You won the insurance!";
            }
            else
            {
                mainText.text = "BLACKJACK!♠ Dealer wins!";
            }
        }
        // if player busts, dealer didn't or if dealer has more points, deal wins
        else if(playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts, player didn't or player has more points, player wins
        else if(dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        // check for tie, return bets
        else if(playerScript.handValue == dealerScript.handValue) 
        {
            mainText.text = "Push";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set ui up for next move / hand / turn
        if(roundOver)
        {
            roundOver = false;
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayRoundOverClip();
            doubleBtn.gameObject.SetActive(false);
            betBtn.gameObject.SetActive(true);
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            insuranceBtn.gameObject.SetActive(false);
            mainText.gameObject.SetActive(true);
            insuranceText.gameObject.SetActive(false);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            pot = 0;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            betsText.text = "Bets: $" + pot.ToString();
            standClicks = 0;
            dealClicked = false;
            betClicked = false;
            hitClicked = false;
            standBtnClicked = false;
            doubleBtnClicked = false;
            splitBtnClicked = false;
            insuranceBtnClicked = false;
            dealerScript.haveAce = false;
            dealerScript.haveBigs = false;
            playerScript.haveSplit = false;
        }
    }

    // Add money to pot if clicked
    void BetClicked() 
    {
        if(!dealClicked)
        {
            betClicked = true;
            TextMeshProUGUI newBet = betBtn.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
            playerScript.AdjustMoney(-intBet);
            cashText.text = "$" + playerScript.GetMoney().ToString();
            pot += (intBet * 2);
            betsText.text = "Bets: $" + pot.ToString();
            // Play sound effect
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayBetClip();
        }
    }

    void DoubleClicked()
    {
        if(dealClicked && !hitClicked && !doubleBtnClicked)
        {
            doubleBtnClicked = true;
            pot *= 2;
            playerScript.AdjustMoney(-pot/4);
            cashText.text = "$" + playerScript.GetMoney().ToString();
            betsText.text = "Bets: $" + pot.ToString();
            doubleBtn.gameObject.SetActive(false);
            // Play sound effect
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayBetClip();
        }
    }
    void SplitClicked()
    {
        if(!splitBtnClicked) 
        {
            // Transfer splitted card to new position
            playerScript.GetComponentsInChildren<CardScript>()[1].gameObject.transform.position =
            new Vector3(playerScript.GetComponentsInChildren<CardScript>()[1].transform.position.x + 4.0f, 
            playerScript.GetComponentsInChildren<CardScript>()[1].transform.position.y);
            // Nullify second split value
            int index0value = playerScript.GetComponentsInChildren<CardScript>()[0].GetValueOfCard();
            int index1value = playerScript.GetComponentsInChildren<CardScript>()[1].GetValueOfCard();
            scoreText.text = "Hand: " + index0value.ToString();

            // Adjust click status
            splitBtnClicked = true;
            playerScript.haveSplit = true;
            // Play sound effect
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayBetClip();
        }
    }

    void InsuranceClicked() 
    {
        if(!insuranceBtnClicked)
        {
            insuranceText.gameObject.SetActive(true);
            insuranceBtnClicked = true;
            int insuranceMoney = pot/4;
            playerScript.AdjustMoney(-insuranceMoney);
            cashText.text = "$" + playerScript.GetMoney().ToString();
            if(!dealerScript.haveBigs)
            {
                insuranceText.text = "No BLACKJACK!♠ You lost the insurance!";
            }
            // Play sound effect
            GameObject.Find("AudioPlayer").GetComponent<AudioManager>().PlayBetClip();      
        }
    }
}
