using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string Name;
    public GameObject Effect;
    
    public delegate void CardSelection(Card card);
    public static event CardSelection OnCardSelected;
    
    public void Select()
    {
        if (OnCardSelected != null)
            OnCardSelected(this);
    }
}
