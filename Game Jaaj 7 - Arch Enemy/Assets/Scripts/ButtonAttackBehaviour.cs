using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAttackBehaviour : MonoBehaviour
{
    private GamePlayController gamePlayController;

    public Button button;

    public GameObject content;
    public Text cooldownText;

    public Image contentImg;


    public Card card;

    public int currentCooldown;

    private void Start()
    {
        gamePlayController = FindObjectOfType(typeof(GamePlayController)) as GamePlayController;
      
    }

    public void cooldown()
    {
        currentCooldown = card.cooldown;
    }
    public void setup()
    {
        card = gamePlayController.playerCards[2];
    }

    public void Load()
    {
        if(currentCooldown != 0)
        {
            currentCooldown--;
            cooldownText.text = currentCooldown.ToString();
            if(currentCooldown == 0)
            {
                cooldownText.gameObject.SetActive(false);
                content.SetActive(true);
            }
            
        }
        else
        {
            cooldownText.gameObject.SetActive(false);
            content.SetActive(true);
        }
    }

    public bool isCooldown()
    {
        if(currentCooldown != 0)
        {
            content.SetActive(false);
            cooldownText.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

}
