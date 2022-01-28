using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string Name;
    
    public delegate void CardSelection(Card card);
    public static event CardSelection OnCardSelected;

    // Start is called before the first frame update
    public void Select()
    {
        if (OnCardSelected != null)
            OnCardSelected(this);
    }
}
