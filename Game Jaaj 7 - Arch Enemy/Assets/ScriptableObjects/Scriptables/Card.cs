using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Card", menuName ="Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite image;
    public int damage;
    public double CriticalChance;
    public Sprite border;
}
