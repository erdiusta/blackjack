using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    string[] cardTypes = new string[53];
    public int currentIndex = 0;
    [HideInInspector] public string cardType1;
    [HideInInspector] public string cardType2;
    [HideInInspector] public string cardType;

    void Start()
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        int typeNum = 0;
        // Loop to assign values to the cards
        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            typeNum = i;
            // Count up to the amount of cards, 52
            num %= 13;
            typeNum %= 13;
            // if there is a remainder after x/13, then remainder
            // if used as the value, unless over 10, the use 10
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
            switch(typeNum)
            {
                case 1:
                    cardType = "ace";
                    break;
                case 2:
                    cardType = "two";
                    break;
                case 3:
                    cardType = "three";
                    break;
                case 4:
                    cardType = "four";
                    break;
                case 5:
                    cardType = "five";
                    break;
                case 6:
                    cardType = "six";
                    break;
                case 7:
                    cardType = "seven";
                    break;
                case 8:
                    cardType = "eight";
                    break;
                case 9:
                    cardType = "nine";
                    break;
                case 10:
                    cardType = "ten";
                    break;
                case 11:
                    cardType = "jack";
                    break;
                case 12:
                    cardType = "queen";
                    break;
                case 0:
                    cardType = "king";
                    break;
            }
            cardTypes[i] = cardType;
            typeNum++;          
        }        
    }

    public void Shuffle() 
    {
        // Standard array data swapping technique
        for(int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            while(j == 0) {j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;}
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;

            string type = cardTypes[i];
            cardTypes[i] = cardTypes[j];
            cardTypes[j] = type;
        }
        currentIndex = 1;
    }

    public int DealCard(CardScript cardScript) 
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        cardScript.SetType(cardTypes[currentIndex]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
