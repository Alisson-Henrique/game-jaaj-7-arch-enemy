using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public GameObject panelHUD;
	public GameObject fadeHUD;

	private GamePlayController gamePlayController;
	private ButtonAttackBehaviour attackBehaviour;
	private GameController gameController;

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

	public bool isDefense;
	// Start is called before the first frame update
	void Start()
	{
		gamePlayController = FindObjectOfType(typeof(GamePlayController)) as GamePlayController;
		gameController = FindObjectOfType(typeof(GameController)) as GameController;
		attackBehaviour = FindObjectOfType(typeof(ButtonAttackBehaviour)) as ButtonAttackBehaviour;
	}

	public void StartBattle()
    {
		state = BattleState.START;
		enemyPrefab = gameController.EnemyPrefabs[gameController.curretLevel];
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position, playerBattleStation.localRotation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position, enemyBattleStation.localRotation);
		enemyUnit = enemyGO.GetComponent<Unit>();


		dialogueText.text = "Você enfrentara " + enemyUnit.unitName + "...";
		attackBehaviour = FindObjectOfType(typeof(ButtonAttackBehaviour)) as ButtonAttackBehaviour;

		attackBehaviour.setup();

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);
		fadeHUD.SetActive(false);
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
			StartCoroutine(EndBattle());
		}
		else
		{
			state = BattleState.ENEMYTURN;
			OnEnemyMoviment();
		}
	}

	IEnumerator EnemyTurn(Card card)
	{
		bool con = true;
		dialogueText.text = enemyUnit.unitName + " Usou " + card.cardName + " !!!";

		yield return new WaitForSeconds(3f);

		int damage = card.damage;

		if (isDefense)
        {
			isDefense = false;
			damage = Mathf.RoundToInt(damage - (damage * 0.5f));
		}

		bool isDead = playerUnit.TakeDamage(damage);
		isDefense = false;
		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(2f);

		if (isDead)
		{
			state = BattleState.LOST;
			StartCoroutine(EndBattle());
		}
		else
		{
			state = BattleState.PLAYERTURN;
			panelHUD.SetActive(true);
			attackBehaviour.Load();
			PlayerTurn();
		}
		
	}

	IEnumerator EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = "Você venceu!";
			yield return new WaitForSeconds(2f);
			gameController.LevelUp();
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "Você perdeu...";
			yield return new WaitForSeconds(2f);
			gameController.Lost();
		}

		yield return new WaitForSeconds(1f);
	}

	void PlayerTurn()
	{
		dialogueText.text = "Escolha um ataque";
	}

	IEnumerator PlayerHeal(Card card)
	{
		panelHUD.SetActive(false);
		playerUnit.Heal(card.damage);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "Você recuperou sua vida!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		OnEnemyMoviment();
	}

	IEnumerator PlayerDefense(Card card)
	{
		panelHUD.SetActive(false);
		isDefense = true;
		dialogueText.text = "Você Prepara Defesa!";
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

	public void OnHeal(Card card)
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal(card));
	}

	public void OnDefense(Card card)
    {
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerDefense(card));
    }

	public void OnMoviment(int id)
    {
		Card card = gamePlayController.playerCards[id];

		if (id == 2 && attackBehaviour.isCooldown())
        {
			return;
        }
        else
        {
			if (id == 2)
			{
				attackBehaviour.cooldown();
			}

			attackBehaviour.isCooldown();
			switch (card.attckType)
			{
				case ATTACK_TYPE.Attack:
					OnAttack(card);
					break;
				case ATTACK_TYPE.Defense:
					OnDefense(card);
					break;
				case ATTACK_TYPE.Heal:
					OnHeal(card);
					break;
			}
		}
	}

	public void OnEnemyMoviment()
    {
		Card[] cards = gamePlayController.enemyCards;
		Card choice = cards[Random.Range(0, cards.Length)];

		StartCoroutine(EnemyTurn(choice));
	}
}