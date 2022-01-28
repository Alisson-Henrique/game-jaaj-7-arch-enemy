using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    void OnEnable()
    {
        Card.OnCardSelected += Echo;
    }

    void OnDisable()
    {
        Card.OnCardSelected += Echo;
    }

    void Echo(Card card)
    {
        Debug.Log(card.Name);
    }
}
