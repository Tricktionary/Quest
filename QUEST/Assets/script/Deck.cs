using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	private List<Card> cards;

	//Constructor 
	public Deck(bool isAdventure, bool isStory){
		cards = new List<Card>();
		if(isAdventure){
			
			// public WeaponCard(int cardId, int bp, bool adventure, bool story, string cardAsset)

			//2 Excaliburs
			Card card1 = new WeaponCard(1,30,true,false,"Asset/card_image/weapons/weaponCard3");
			cards.Add(card1);
			Card card2 = new WeaponCard(2,30,true,false,"Asset/card_image/weapons/weaponCard3");
			cards.Add(card2);
			
			//6 Lances
			Card card3 = new WeaponCard(3,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card3);
			Card card4 = new WeaponCard(4,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card4);
			Card card5 = new WeaponCard(5,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card5);
			Card card6 = new WeaponCard(6,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card6);
			Card card7 = new WeaponCard(7,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card7);
			Card card8 = new WeaponCard(8,20,true,false,"Asset/card_image/weapons/weaponCard4");
			cards.Add(card8);

			//8 Battle Axes
			Card card9 = new WeaponCard(9,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card9);
			Card card10 = new WeaponCard(10,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card10);
			Card card11 = new WeaponCard(11,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card11);
			Card card12 = new WeaponCard(12,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card12);
			Card card13 = new WeaponCard(13,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card13);
			Card card14 = new WeaponCard(14,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card14);
			Card card15 = new WeaponCard(15,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card15);
			Card card16 = new WeaponCard(16,15,true,false,"Asset/card_image/weapons/weaponCard5");
			cards.Add(card16);

			//16 Swords
			Card card17 = new WeaponCard(17,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card17);
			Card card18 = new WeaponCard(18,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card18);
			Card card19 = new WeaponCard(19,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card19);
			Card card20 = new WeaponCard(20,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card20);
			Card card21 = new WeaponCard(21,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card21);
			Card card22 = new WeaponCard(22,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card22);
			Card card23 = new WeaponCard(23,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card23);
			Card card24 = new WeaponCard(24,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card24);
			Card card25 = new WeaponCard(25,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card25);
			Card card26 = new WeaponCard(26,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card26);
			Card card27 = new WeaponCard(27,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card27);
			Card card28 = new WeaponCard(28,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card28);
			Card card29 = new WeaponCard(29,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card29);
			Card card30 = new WeaponCard(30,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card30);
			Card card31 = new WeaponCard(31,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card31);
			Card card32 = new WeaponCard(32,10,true,false,"Asset/card_image/weapons/weaponCard1");
			cards.Add(card32);
			
			//11 Horses
			Card card33 = new WeaponCard(33,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card33);
			Card card34 = new WeaponCard(34,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card34);
			Card card35 = new WeaponCard(35,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card35);
			Card card36 = new WeaponCard(36,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card36);
			Card card37 = new WeaponCard(37,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card37);
			Card card38 = new WeaponCard(38,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card38);
			Card card39 = new WeaponCard(39,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card39);
			Card card40 = new WeaponCard(40,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card40);
			Card card41 = new WeaponCard(41,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card41);
			Card card42 = new WeaponCard(42,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card42);
			Card card43 = new WeaponCard(43,10,true,false,"Asset/card_image/weapons/weaponCard6");
			cards.Add(card43);

			//6 Daggers
			Card card44 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card44);
			Card card45 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card45);
			Card card46 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card46);
			Card card47 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card47);
			Card card48 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card48);
			Card card49 = new WeaponCard(44,5,true,false,"Asset/card_image/weapons/weaponCard2");
			cards.Add(card49);

		}
		if(isStory){
			//Fill Cards with story cards
		}

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


	//made-up shuffling algorithm
	void Shuffle () {
		List<Card> newCards = new List<Card>();
		int count = cards.Count;
		int[] randomList = new int[count];
		for (int i=0;i<count;i++) {
			randomList [i] = 1;
		}
	}

	int GetSize () {
		return cards.Count;
	}
}
