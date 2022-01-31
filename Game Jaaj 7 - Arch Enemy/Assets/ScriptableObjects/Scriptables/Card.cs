using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ATTACK_TYPE { Attack, Heal, Defense}

[CreateAssetMenu(fileName ="New Card", menuName ="Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite image;
    public int damage;
    public double CriticalChance;
    public Sprite border;
    public string description;
    public ATTACK_TYPE attckType;
    public int cooldown;
    public int currentCooldwon;
    public GameObject cardVFX;
    public int target;
}
