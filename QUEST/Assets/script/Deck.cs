using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	// The list of cards that make up the deck.
	private List<Card> cards = new List<Card>();
	
	public List<Card> GetDeck(){
		return cards;
	}
	//Constructor. 
	public Deck(bool isAdventure){
		if (isAdventure) {
			//Fill deck with adventure cards.

			/* Weapon Load */
			/* 2 Excaliburs */
			AddXCopies(new WeaponCard("Excalibur", 30, "Asset/card_image/weapons/weaponCard3"), 2);
			
			/* 6 Lances */
			AddXCopies(new WeaponCard("Lance", 20, "Asset/card_image/weapons/weaponCard4"), 6);

			/* 8 Battle Axe */
			AddXCopies(new WeaponCard("BattleAxe", 15, "Asset/card_image/weapons/weaponCard5"), 8);

			/* 16 Swords */
			AddXCopies(new WeaponCard("Sword", 10, "Asset/card_image/weapons/weaponCard1"), 16);

			/* 11 Horses */
			AddXCopies(new WeaponCard("Horse", 10, "Asset/card_image/weapons/weaponCard6"), 11);

			/* 6 Daggers */
			AddXCopies(new WeaponCard("Dagger", 5, "Asset/card_image/weapons/weaponCard2"), 6);
			
			/* Foe Load */

			/* 7 Robber Knight */
			AddXCopies(new FoeCard("Robber Knight", 15, 15, false,"Asset/card_image/foe/foeCard1"),7);

			/* 5 Saxons */
			AddXCopies(new FoeCard("Saxons",10,20,false,"Asset/card_image/foe/foeCard2"),5);
			
		} else {
			// Fill the deck with story cards.

		}

		// Shuffe the deck of cards after adding.
		this.Shuffle();
	}


	// Adds x copies of a Card to the deck.
	void AddXCopies(Card card, int x){
		for (int i = 0; i < x; i++) {
			cards.Add(card);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Draw a card from the deck.
	public Card Draw () {
		if (cards.Count > 0) {
			Card card = cards [0];
			cards.RemoveAt (0);
			return card;
		}
		return null;
	}

	// Shuffe the deck of cards.
	void Shuffle () {
		List<Card> newCards = new List<Card>();
		int count = cards.Count;
		List<int> randomList = new List<int>();
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

	// Get the size of the deck.
	public int GetSize () {
		return cards.Count;
	}
}