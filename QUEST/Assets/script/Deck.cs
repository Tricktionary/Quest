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

			/* 4 Boar*/
			AddXCopies(new FoeCard("Boar",5,15,false,"Asset/card_image/foe/foeCard3"),4);

			/* 8 Thieves*/
			AddXCopies(new FoeCard("Thieves",5,5,false,"Asset/card_image/foe/foeCard4"),8);

			/* 2 Green Knight*/
			AddXCopies(new FoeCard("Green Knight",25,40,false,"Asset/card_image/foe/foeCard5"),2);

			/* 3 Black Knight */
			AddXCopies(new FoeCard("Black Knight",25,35,false,"Asset/card_image/foe/foeCard6"),3);

			/* 6 Evil Knight*/
			AddXCopies(new FoeCard("Evil Knight",20,30,false,"Asset/card_image/foe/foeCard7"),6);

			/* 8 Saxon Knight */
			AddXCopies(new FoeCard("Saxon Knight",15,25,false,"Asset/card_image/foe/foeCard8"),8);

			/* 1 Dragon */
			AddXCopies(new FoeCard("Dragon",50,70,false,"Asset/card_image/foe/foeCard9"),1);

			/* 2 Giant */
			AddXCopies(new FoeCard("Giant",40,40,false,"Asset/card_image/foe/foeCard10"),2);

			/* 4 Mordred */
			AddXCopies(new FoeCard("Mordred",30,30,true,"Asset/card_image/foe/foeCard11"),4);


			/* Amour load */

			/* */


		} else {
			//WHEN ADDING FEATURED FOE * means all
			// Fill the deck with story cards.
			AddXCopies(new QuestCard("Search for the Holy Grail",5,"*"),1);
			AddXCopies(new QuestCard("Test of the Green Knight",4,"Green Knight"),1);
			AddXCopies(new QuestCard("Search for the Questing Beast",4,""),1);
			AddXCopies(new QuestCard("Defend the Queen's Honor",4,"*"),1);
			AddXCopies(new QuestCard("Rescue the Fair Maiden",3,"Black Knight"),1);
			AddXCopies(new QuestCard("Journey Through the Enchanted Forest",3,"Evil Knight"),1);
			AddXCopies(new QuestCard("Vanquish King Arthur's Enemies",3,""),2);
			AddXCopies(new QuestCard("Slay the Dragon",3,"Dragon"),1);
			AddXCopies(new QuestCard("Boar Hunt",2,"Boar"),2);
			AddXCopies(new QuestCard("Repel the Saxxon Raiders",2,"Saxon"),2);

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