﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck{

	// The list of cards that make up the deck.
	private List<Card> _cards = new List<Card>();
	
	public List<Card> GetDeck(){
		return _cards;
	}
	//Constructor. 
	public Deck(string type){
		if (type.Equals ("Adventure")) {
			//Fill deck with adventure cards.

			/* Weapon Load */
			/* 2 Excaliburs */
			addWeapon("Excalibur", 30, "card_image/weapons/weaponCard3", 2);

			/* 6 Lances */
			addWeapon("Lance", 20, "card_image/weapons/weaponCard4", 6);

			/* 8 Battle Axe */
			addWeapon("BattleAxe", 15, "card_image/weapons/weaponCard5", 8);

			/* 16 Swords */
			addWeapon("Sword", 10, "card_image/weapons/weaponCard1", 16);

			/* 11 Horses */
			addWeapon("Horse", 10, "card_image/weapons/weaponCard6", 11);

			/* 6 Daggers */
			addWeapon("Dagger", 5, "card_image/weapons/weaponCard2", 6);

			/* Foe Load */

			/* 7 Robber Knight */
			addFoe("Robber Knight","Robber Knight", 15, 15, false, "card_image/foe/foeCard1", 7);

			/* 5 Saxons */
			addFoe("Saxons","Saxon", 10, 20, false, "card_image/foe/foeCard2", 5);

			/* 4 Boar*/
			addFoe("Boar","Boar", 5, 15, false, "card_image/foe/foeCard3", 4);

			/* 8 Thieves*/
			addFoe("Thieves","Thieves", 5, 5, false, "card_image/foe/foeCard4", 8);

			/* 2 Green Knight*/
			addFoe("Green Knight","Green Knight", 25, 40, false, "card_image/foe/foeCard5", 2);

			/* 3 Black Knight */
			addFoe("Black Knight","Black Knight", 25, 35, false, "card_image/foe/foeCard6", 3);

			/* 6 Evil Knight*/
			addFoe("Evil Knight","Evil Knight", 20, 30, false, "card_image/foe/foeCard7", 6);

			/* 8 Saxon Knight */
			addFoe("Saxon Knight","Saxon", 15, 25, false, "card_image/foe/foeCard8", 8);

			/* 1 Dragon */
			addFoe("Dragon","Dragon", 50, 70, false, "card_image/foe/foeCard9", 1);

			/* 2 Giant */
			addFoe("Giant","Giant", 40, 40, false, "card_image/foe/foeCard10", 2);

			/* 4 Mordred */
			addFoe("Mordred","Mordred", 30, 30, true, "card_image/foe/foeCard11", 4);

			/* Ally load */

			/* Sir Pellinore */
			addAlly("Sir Pellinore",10,0,0,4,"Search for the Questing Beast", null, false, "card_image/special/specialCard1", 1);

			/* Sir Percival */
			addAlly("Sir Percival",5,0,20,0,"Search for the Holy Grail", null, false, "card_image/special/specialCard2", 1);

			/* Sir Tristan */
			addAlly("Sir Tristan", 10, 0, 20, 0, null, "Queen Iseult", false, "card_image/special/specialCard4", 1);

			/* King Arthur */
			addAlly("King Arthur", 10, 2, 0, 0, null, null, false, "card_image/special/specialCard5", 1);

			/* Queen Guinevere */
			addAlly("Queen Guinevere", 0, 3, 0, 0, null, null, false, "card_image/special/specialCard6", 1);

			/* Merlin */
			addAlly("Merlin", 0, 0, 0, 0, null, null, true, "card_image/special/specialCard7", 1);

			/* Queen Iseult */
			addAlly("Queen Iseult", 0, 2, 0, 4, null, "Sir Tristan", false, "card_image/special/specialCard8", 1);            

			/* Sir Lancelot */
			addAlly("Sir Lancelot", 15, 0, 25, 0, "Defend the Queen's Honor", null, false, "card_image/special/specialCard9", 1);

			/* Sir Galahad */
			addAlly("Galahad", 15, 0, 0, 0, null, null, false, "card_image/special/specialCard10", 1);

			/* Sir Gawain */
			addAlly("Sir Gawain", 10, 0, 20, 0, "Test of the Green Knight", null, false, "card_image/special/specialCard11", 1);        

			/* Amour load */
			// NOTE: Make an addArmour method for loading these.

			// Shuffe the deck of cards after adding.
			this.Shuffle();
		} else if (type.Equals ("Story")) {
			//WHEN ADDING FEATURED FOE * means all
			// Fill the deck with story cards.
			addQuest("Search for the Holy Grail", 5, "*", "card_image/quest/questCard9", 1);
			addQuest("Test of the Green Knight", 4, "Green Knight", "card_image/quest/questCard10", 1);
			addQuest("Search for the Questing Beast", 4, "", "card_image/quest/questCard5", 1);
			addQuest("Defend the Queen's Honor", 4, "*", "card_image/quest/questCard6", 1);
			addQuest("Rescue the Fair Maiden", 3, "Black Knight", "card_image/quest/questCard8", 1);
			addQuest("Journey Through the Enchanted Forest", 3, "Evil Knight", "card_image/quest/questCard1", 1);
			addQuest("Vanquish King Arthur's Enemies", 3, "", "card_image/quest/questCard2", 2);
			addQuest("Slay the Dragon", 3, "Dragon", "card_image/quest/questCard7", 1);
			addQuest("Boar Hunt", 2, "Boar", "card_image/quest/questCard4", 2);
			addQuest("Repel the Saxxon Raiders", 2, "Saxon", "card_image/quest/questCard3", 2);

			//adding event cards
			addEvent("Chivalrous Deed", "lowest rank and shield receives 3 shields", "card_image/events/eventCard1", 1);
			addEvent("Pox", "All players except player drawing this card lose 1 shield", "card_image/events/eventCard2", 1);		
			addEvent("Plague", "Drawer loses 2 shields if possible", "card_image/events/eventCard3", 1);
			addEvent("King's Recognition", "The next player(s) to complete a quest will receive 2 extra shields", "card_image/events/eventCard4", 2);
			addEvent("Queen's Favor", "The lowest ranked player(s) immediately receives 2 Adventure cards", "card_image/events/eventCard5", 2);
			addEvent("Court Called to Camelot", "All Allies in play must be discarded", "card_image/events/eventCard6", 2);
			addEvent("King's Call to Arms", "Highest ranked player(s) must discard 1 weapon, if unable 2 foe cards must be discarded", "card_image/events/eventCard7", 1);
			addEvent("Prosperity Throughout the Realm", "All players may immediately draw 2 adventure Cards", "card_image/events/eventCard8", 1);
			
			// Shuffe the deck of cards after adding.
			this.Shuffle();
		}

	}

	// Add a given amount of a weapon to the deck.
	void addWeapon(string name, int power, string asset, int copies){
		for (int i = 0; i < copies; i++) {
			_cards.Add(new WeaponCard(name, power, asset));
		}
	}

	// Add a given amount of a foe to the deck.
	void addFoe(string name, string type, int loPower, int hiPower, bool special, string asset, int copies){
		for (int i = 0; i < copies; i++) {
			_cards.Add(new FoeCard(name, type, loPower, hiPower, special, asset));
		}
	}

	// Add a given amount of an ally to the deck.
	void addAlly(string name, int power, int bid, int bonusPower, int bonusBid, string questCondition, string allyCondition, bool special, string asset, int copies){
		for (int i = 0; i < copies; i++) {
			_cards.Add(new AllyCard(name, power, bid, bonusPower, bonusBid, questCondition, allyCondition, special, asset));
		}
	}

	// Add a given amount of a quest to the deck.
	void addQuest(string name, int stages, string featuredFoe, string asset, int copies){
		for (int i = 0; i < copies; i++) {
			_cards.Add(new QuestCard(name, stages, featuredFoe, asset));
		}
	}

	void addEvent(string name, string conditions, string asset, int copies){
		for(int i = 0; i<copies; i++){
			_cards.Add(new EventCard(name, conditions, asset));
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
		if (_cards.Count > 0) {
			Card card = _cards [0];
			_cards.RemoveAt(0);
			return card;
		}
		return null;
	}

	public void Discard (Card card) {
		_cards.Add (card);
	}

	// Shuffe the deck of cards.
	void Shuffle () {
		List<Card> newCards = new List<Card>();
		int count = _cards.Count;
		List<int> randomList = new List<int>();
		for (int i=0;i<count;i++) {
			randomList.Add(i);
		}
		for (int i = count; i > 0; i--) {
			int value = (int)Mathf.Floor(Random.value * i);
			newCards.Add(_cards[randomList[value]]);
			randomList.RemoveAt(value);
		}
		_cards = newCards;
	}

	// Get the size of the deck.
	public int GetSize () {
		return _cards.Count;
	}
}