using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

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
			AddXCopies (new WeaponCard ("Excalibur", 30, "card_image/weapons/weaponCard3"), 2);
			
			/* 6 Lances */
			AddXCopies (new WeaponCard ("Lance", 20, "card_image/weapons/weaponCard4"), 6);

			/* 8 Battle Axe */
			AddXCopies (new WeaponCard ("BattleAxe", 15, "card_image/weapons/weaponCard5"), 8);

			/* 16 Swords */
			AddXCopies (new WeaponCard ("Sword", 10, "card_image/weapons/weaponCard1"), 16);

			/* 11 Horses */
			AddXCopies (new WeaponCard ("Horse", 10, "card_image/weapons/weaponCard6"), 11);

			/* 6 Daggers */
			AddXCopies (new WeaponCard ("Dagger", 5, "card_image/weapons/weaponCard2"), 6);
			

			/* Foe Load */

			/* 7 Robber Knight */
			AddXCopies (new FoeCard ("Robber Knight", 15, 15, false, "card_image/foe/foeCard1"), 7);

			/* 5 Saxons */
			AddXCopies (new FoeCard ("Saxons", 10, 20, false, "card_image/foe/foeCard2"), 5);

			/* 4 Boar*/
			AddXCopies (new FoeCard ("Boar", 5, 15, false, "card_image/foe/foeCard3"), 4);

			/* 8 Thieves*/
			AddXCopies (new FoeCard ("Thieves", 5, 5, false, "card_image/foe/foeCard4"), 8);

			/* 2 Green Knight*/
			AddXCopies (new FoeCard ("Green Knight", 25, 40, false, "card_image/foe/foeCard5"), 2);

			/* 3 Black Knight */
			AddXCopies (new FoeCard ("Black Knight", 25, 35, false, "card_image/foe/foeCard6"), 3);

			/* 6 Evil Knight*/
			AddXCopies (new FoeCard ("Evil Knight", 20, 30, false, "card_image/foe/foeCard7"), 6);

			/* 8 Saxon Knight */
			AddXCopies (new FoeCard ("Saxon Knight", 15, 25, false, "card_image/foe/foeCard8"), 8);

			/* 1 Dragon */
			AddXCopies (new FoeCard ("Dragon", 50, 70, false, "card_image/foe/foeCard9"), 1);

			/* 2 Giant */
			AddXCopies (new FoeCard ("Giant", 40, 40, false, "card_image/foe/foeCard10"), 2);

			/* 4 Mordred */
			AddXCopies (new FoeCard ("Mordred", 30, 30, true, "card_image/foe/foeCard11"), 4);


			/* Ally load */

			/* Sir Pellinore */
			AddXCopies(new AllyCard("Sir Pellinore",10,0,0,4,"Search for the Questing Beast", null, false,  "card_image/special/specialCard1"),1);

			/* Sir Percival */
			AddXCopies(new AllyCard("Sir Percival",5,0,20,0,"Search for the Holy Grail", null, false,  "card_image/special/specialCard2"),1);

			/* Sir Tristan */
			AddXCopies(new AllyCard("Sir Tristan",10,0,20,0,null, "Queen Iseult", false,  "card_image/special/specialCard4"),1);

			/* King Arthur */
			AddXCopies(new AllyCard("King Arthur",10,2,0,0,null, null, false,  "card_image/special/specialCard5"),1);

			/* Queen Guinevere */
			AddXCopies(new AllyCard("Queen Guinevere",0,3,0,0,null, null, false,  "card_image/special/specialCard6"),1);

			/* Merlin */
			AddXCopies(new AllyCard("Merlin",0,0,0,0,null, null, true,  "card_image/special/specialCard7"),1);

			/* Queen Iseult */
			AddXCopies(new AllyCard("Queen Iseult",0,2,0,4,null, "Sir Tristan", false,  "card_image/special/specialCard8"),1);			

			/* Sir Lancelot */
			AddXCopies(new AllyCard("Sir Lancelot",15,0,25,0,"Defend the Queen's Honor", null, false,  "card_image/special/specialCard9"),1);
			
			/* Sir Galahad */
			AddXCopies(new AllyCard("Galahad",15,0,0,0,null, null, false,  "card_image/special/specialCard10"),1);

			/* Sir Gawain */
			AddXCopies(new AllyCard("Sir Gawain",10,0,20,0,"Test of the Green Knight", null, false,  "card_image/special/specialCard11"),1);		
	

			/* Amour load */
			AddXCopies(new AmourCard("Amour",10,1, "card_image/special/specialCard3" ),1);

			// Shuffe the deck of cards after adding.
			this.Shuffle();
		} else if (type.Equals ("Story")) {
			//WHEN ADDING FEATURED FOE * means all
			// Fill the deck with story cards.
			AddXCopies (new QuestCard ("Search for the Holy Grail", 5, "*", "card_image/quest/questCard9"), 1);
			AddXCopies (new QuestCard ("Test of the Green Knight", 4, "Green Knight", "card_image/quest/questCard10"), 1);
			AddXCopies (new QuestCard ("Search for the Questing Beast", 4, "", "card_image/quest/questCard5"), 1);
			AddXCopies (new QuestCard ("Defend the Queen's Honor", 4, "*", "card_image/quest/questCard6"), 1);
			AddXCopies (new QuestCard ("Rescue the Fair Maiden", 3, "Black Knight", "card_image/quest/questCard8"), 1);
			AddXCopies (new QuestCard ("Journey Through the Enchanted Forest", 3, "Evil Knight", "card_image/quest/questCard1"), 1);
			AddXCopies (new QuestCard ("Vanquish King Arthur's Enemies", 3, "", "card_image/quest/questCard2"), 2);
			AddXCopies (new QuestCard ("Slay the Dragon", 3, "Dragon", "card_image/quest/questCard7"), 1);
			AddXCopies (new QuestCard ("Boar Hunt", 2, "Boar", "card_image/quest/questCard4"), 2);
			AddXCopies (new QuestCard ("Repel the Saxxon Raiders", 2, "Saxon", "card_image/quest/questCard3"), 2);
			// Shuffe the deck of cards after adding.
			this.Shuffle();
		}
	}

	// Adds x copies of a Card to the deck.
	void AddXCopies(Card card, int x){
		for (int i = 0; i < x; i++) {
			_cards.Add(card);
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