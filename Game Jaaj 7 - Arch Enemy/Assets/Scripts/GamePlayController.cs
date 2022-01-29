using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    [SerializeField]
    private Card[] cards;
    private GameController gameController;
    private BattleSystem battleSystem;
    [SerializeField]
    private Card[] options;

    //HUD
    [SerializeField]
    private Text descriptionText; 

    //Panel Choice
    public GameObject choicePanel;

    public Image choiceContent01;
    public Image choiceContent02;
    public Image choiceBorder01;

    public Image choiceBorder02;

    public Text choiceDescription01;
    public Text choiceDescription02;

    //Player
    public Card[] playerCards;

    [SerializeField]
    private BattleHUD playerHUD;

    //Enemy
    public Card[] enemyCards;

    [SerializeField]
    private BattleHUD enemyHUD;

    private void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        battleSystem = FindObjectOfType(typeof(BattleSystem)) as BattleSystem;
        for (int i = 0; i < 2; i++)
        {
            int t = Random.Range(0, cards.Length);
            options[i] = cards[t];
        }

        choiceContent01.sprite = options[0].image;
        choiceContent02.sprite = options[1].image;

        choiceBorder01.sprite = options[0].border;
        choiceBorder02.sprite = options[1].border;

        choiceDescription01.text = options[0].description;
        choiceDescription02.text = options[1].description;
    }

    public void ShowInfoCard(int id)
    {
        descriptionText.text = playerCards[id].description;
    } 

    public void SelectOption(int id)
    {
        playerCards[2] = options[id];
        enemyCards[2] = id == 1 ? options[0] : options[1];
        Setup();
        choicePanel.SetActive(false);
        battleSystem.StartBattle();
    }

    public void Setup()
    {
        int index = 0;

        foreach(Card card in playerCards)
        {
            playerHUD.CardImage[index].sprite = card.image;
            playerHUD.BorderImage[index].sprite = card.border;
            index++;
        }
    }
}
