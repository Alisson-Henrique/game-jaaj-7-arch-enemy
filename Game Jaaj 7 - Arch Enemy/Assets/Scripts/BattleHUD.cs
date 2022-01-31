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

	public GameObject cardSelectedVFX;
	public AudioClip cardSelectedSFX;

	public GameObject hunter;
	public GameObject monster;

    private AudioSource audioSource;

	void Awake() 
	{
		audioSource = GetComponent<AudioSource>();
	}

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
       
		audioSource.clip = cardSelectedSFX;
	    audioSource.Play();
		
		var cardSelectedVFXInstance = Instantiate(cardSelectedVFX);
		yield return new WaitForSeconds(2f);
		Destroy(cardSelectedVFXInstance);	
		selectedCard.SetActive(false);

		// if (card.cardVFX != null)
		// {
		// 	var cardAnimationVFXTransform = card.target == 0 ? hunter.transform : monster.transform;
		// 	var cardAnimationVFXInstance = Instantiate(card.cardVFX, cardAnimationVFXTransform);
		// 	yield return new WaitForSeconds(4f);
		// 	Destroy(cardAnimationVFXInstance); 
		// }
	}

}