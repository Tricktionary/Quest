using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	private List<Card> cards = new List<Card>();

	//Constructor 
	public Deck(bool isAdventure){
		if (isAdventure) {
			//Fill Card with adventure cards
			List<AdventureCard> newCards = new List<AdventureCard>();

			/* Weapon Load */
			/* 2 Excaliburs */
			newCards.Add(new WeaponCard("Excalibur",30,"Asset/card_image/weapons/weaponCard3"));
			newCards.Add(new WeaponCard("Excalibur",30,"Asset/card_image/weapons/weaponCard3"));
			
			/* 6 Lances */
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));
			newCards.Add(new WeaponCard("Lance",20,"Asset/card_image/weapons/weaponCard4"));

			/* 8 Battle Axe */
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));
			newCards.Add(new WeaponCard("BattleAxe",15,"Asset/card_image/weapons/weaponCard5"));

			/* 16 Swords */
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));
			newCards.Add(new WeaponCard("Sword",10,"Asset/card_image/weapons/weaponCard1"));

			/* 11 Horses */
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));
			newCards.Add(new WeaponCard("Horse",10,"Asset/card_image/weapons/weaponCard6"));

			/* 6 Daggers */
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			newCards.Add(new WeaponCard("Dagger",5,"Asset/card_image/weapons/weaponCard2"));
			
			/* Foe Load */
			//FoeCard(string name, int loPower, int hiPower, bool special)
			
			/* 7 Robber Knight */
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));
			newCards.Add(new FoeCard("Robber Knight",15,15,false));

			
			Populate();
		} else {
			/* Story */

			/* Quest Load */
			//Populate();
		}

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Populate (List<Card> newCards) {
		cards.AddRange(newCards);
	}

	Card Draw () {
		if (cards.Count > 0) {
			Card card = cards [0];
			cards.RemoveAt (0);
			return card;
		}
		return null;
	}

	//READY TO TEST
	//made-up shuffling algorithm
	void Shuffle () {
		List<Card> newCards = new List<Card>();
		int count = cards.Count;
		List<int> randomList = new List<Int>();
		for (int i=0;i<count;i++) {
			randomList.Add(i);
		}
		for (int i = count; i > 0; i--) {
			int value = (int)Mathf.Floor(Random.value * i);
			newCards.Add(cards[randomList[value]]);
			randomList.RemoveAt(value);
		}
		cards = newCards;
	}

	int GetSize () {
		return cards.Count;
	}
}
