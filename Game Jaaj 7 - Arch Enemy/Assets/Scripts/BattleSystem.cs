using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public GameObject panelHUD;
	public GameObject fadeHUD;
	public GameObject fadeUpHUD;

	private GamePlayController gamePlayController;
	private ButtonAttackBehaviour attackBehaviour;
	private GameController gameController;

	public GameObject playerPrefab;
	private GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

	public bool isDefense;
	public bool isEnemyDefense;
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
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position + playerPrefab.transform.position, playerBattleStation.localRotation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position + enemyPrefab.transform.position, enemyBattleStation.localRotation);
		enemyUnit = enemyGO.GetComponent<Unit>();


		dialogueText.text = "Você enfrentara " + enemyUnit.unitName + "...";
		attackBehaviour = FindObjectOfType(typeof(ButtonAttackBehaviour)) as ButtonAttackBehaviour;

		attackBehaviour.setup();

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();

		fadeUpHUD.GetComponent<Animator>().SetBool("isHiden", true);
		fadeHUD.GetComponent<Animator>().SetBool("isHiden", true);
		yield return new WaitForSeconds(2f);

		fadeUpHUD.SetActive(false);
		fadeHUD.SetActive(false);
	}

	IEnumerator PlayerAttack(Card card)
	{
		panelHUD.SetActive(false);
		playerHUD.SetCardSelected(card);

		int damage = card.damage;

		if (isEnemyDefense)
		{
			isEnemyDefense = false;
			damage = Mathf.RoundToInt(damage - (damage * 0.5f));
		}

		bool isDead = enemyUnit.TakeDamage(damage);
		dialogueText.text = card.cardName + " !!!";
		enemyHUD.SetHP(enemyUnit.currentHP);

		yield return new WaitForSeconds(5f);

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
		playerHUD.SetCardSelected(card);
		panelHUD.SetActive(false);
		playerUnit.Heal(Mathf.RoundToInt(playerUnit.maxHP - (playerUnit.maxHP * 0.5f)));

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "Você recuperou sua vida!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		OnEnemyMoviment();
	}

	IEnumerator PlayerDefense(Card card)
	{
		playerHUD.SetCardSelected(card);
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

		EnemyTurn(choice);
	}

	public void EnemyTurn(Card card)
	{
		switch (card.attckType)
		{
			case ATTACK_TYPE.Attack:
				StartCoroutine(AttackEnemy(card));
				break;
			case ATTACK_TYPE.Defense:
				StartCoroutine(DefenseEnemy(card));
				break;
			case ATTACK_TYPE.Heal:
				StartCoroutine(HealEnemy(card));
				break;
		}

	}

	IEnumerator AttackEnemy(Card card)
    {
		dialogueText.text = enemyUnit.unitName + " Usou " + card.cardName + " !!!";
		enemyHUD.SetCardSelected(card);

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

	IEnumerator HealEnemy(Card card)
    {
		dialogueText.text = enemyUnit.unitName + " Usou " + card.cardName + " !!!";
		enemyHUD.SetCardSelected(card);
		enemyUnit.Heal(Mathf.RoundToInt(enemyUnit.maxHP - (enemyUnit.maxHP * 0.25f)));

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = enemyUnit.unitName + " recuperou a vida!";

		yield return new WaitForSeconds(3f);

		state = BattleState.PLAYERTURN;
		panelHUD.SetActive(true);
		attackBehaviour.Load();
		PlayerTurn();
	}

	IEnumerator DefenseEnemy(Card card)
    {
		dialogueText.text = enemyUnit.unitName + " Usou " + card.cardName + " !!!";
		enemyHUD.SetCardSelected(card);
		isEnemyDefense = true;
		yield return new WaitForSeconds(3f);

		state = BattleState.PLAYERTURN;
		panelHUD.SetActive(true);
		attackBehaviour.Load();
		PlayerTurn();
	}
}