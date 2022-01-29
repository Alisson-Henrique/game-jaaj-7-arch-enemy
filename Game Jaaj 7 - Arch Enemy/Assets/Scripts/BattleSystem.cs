using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public GameObject panelHUD;


	private GamePlayController gamePlayController;
	 
	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

	// Start is called before the first frame update
	void Start()
	{
		gamePlayController = FindObjectOfType(typeof(GamePlayController)) as GamePlayController;
	}

	public void StartBattle()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position, playerBattleStation.localRotation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position, enemyBattleStation.localRotation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "Você enfrentara " + enemyUnit.unitName + "...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack(Card card)
	{
		panelHUD.SetActive(false);
		bool isDead = enemyUnit.TakeDamage(card.damage);
		dialogueText.text = card.cardName + " !!!";
		enemyHUD.SetHP(enemyUnit.currentHP);

		yield return new WaitForSeconds(4f);

		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			OnEnemyMoviment();
		}
	}

	IEnumerator EnemyTurn(Card card)
	{
		dialogueText.text = enemyUnit.unitName + " Usou " + card.cardName + " !!!";

		yield return new WaitForSeconds(3f);

		bool isDead = playerUnit.TakeDamage(card.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(2f);

		if (isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERTURN;
			panelHUD.SetActive(true);
			PlayerTurn();
		}

	}

	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = "Você venceu!";
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "Você perdeu...";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Escolha um ataque";
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "Você recuperou sua vida!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		OnEnemyMoviment();
	}

	public void OnAttack(Card card)
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack(card));
	}

	public void OnHeal()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

	public void OnMoviment(int id)
    {
		Card card = gamePlayController.playerCards[id];

		if(id != 0)
        {
			card.currentCooldwon = card.cooldown;
        }

        switch (card.attckType)
        {
			case ATTACK_TYPE.Attack:
				OnAttack(card);
				break;
			case ATTACK_TYPE.Defense:
				break;
			case ATTACK_TYPE.Heal:
				OnHeal();
				break;
		}

		gamePlayController.playerCards[id] = card;
	}

	public void OnEnemyMoviment()
    {
		Card[] cards = gamePlayController.enemyCards;
		int index = 0;
		Card choice = cards[0];

		foreach(Card card in cards)
        {
			if(card.cooldown == 0)
            {
				if(card.damage > choice.damage)
                {
					choice = card;
                }
            }
			index++;
		}

		if(cards[0].cardName != choice.cardName)
        {
			cards[index].currentCooldwon = cards[index].cooldown;
		}

		gamePlayController.enemyCards = cards;

		StartCoroutine(EnemyTurn(choice));
	}

	public void checkCooldown()
    {

    }
}