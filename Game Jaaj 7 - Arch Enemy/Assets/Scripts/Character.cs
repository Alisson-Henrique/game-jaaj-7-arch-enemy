using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform Opponent;

    void OnEnable()
    {
        Card.OnCardSelected += Attack;
    }

    void OnDisable()
    {
        Card.OnCardSelected += Attack;
    }

    void Attack(Card card)
    {
        Instantiate(card.Effect, Opponent.position, Quaternion.identity);
    }
}
