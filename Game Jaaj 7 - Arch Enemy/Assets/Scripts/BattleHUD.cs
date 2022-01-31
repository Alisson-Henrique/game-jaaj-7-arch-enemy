using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	public Text nameText;
	public Slider hpSlider;

	public Image[] CardImage;
	public Image[] BorderImage;

	public GameObject selectedCard;
	public Image selectedBorder;

	public GameObject cardVFX;

	public GameObject hunter;
	public GameObject monster;

	public void SetHUD(Unit unit)
	{
		// nameText.text = unit.unitName;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}

	public void SetCardSelected(Card card)
    {
		StartCoroutine(CardSelected(card));
	}

	IEnumerator CardSelected(Card card)
    {
		selectedCard.SetActive(true);
		selectedCard.GetComponent<Image>().sprite = card.image;
		selectedBorder.sprite = card.border;

		var cardSelectionVFX = Instantiate(cardVFX);
		yield return new WaitForSeconds(2f);
		Destroy(cardSelectionVFX);	
		selectedCard.SetActive(false);

		// if (card.cardVFX != null)
		// {
		// 	var cardAnimationVFXTransform = card.target == 0 ? hunter.transform : monster.transform;
		// 	var cardAnimationVFX = Instantiate(card.cardVFX);
		// 	yield return new WaitForSeconds(4f);
		// 	Destroy(cardAnimationVFX); 
		// }
	}

}