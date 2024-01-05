using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Value of card, 2 of clubs, etc
    public int value = 0;
    public string type;

    public int GetValueOfCard()
    {
        return value;
    }

    public void SetType(string newType)
    {
        type = newType;
    }

    public string GetTypeOfCard()
    {
        return type;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    public void SetSprite(Sprite newSprite) 
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void ResetCard() 
    {
        Sprite back = GameObject.Find("Deck").GetComponent<DeckScript>().GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        gameObject.GetComponent<Renderer>().enabled = false;
        value = 0;
    }
}
