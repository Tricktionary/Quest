using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	private List<Card> cards = new List<Card>();

	//Constructor 
	public Deck(bool isAdventure){
		if (isAdventure) {
			//Fill Card with adventure cards
			List<Card> newCards = new List<Card>();
			//newCards.Add(new FoeCard());
			//Populate();
		} else {
			//story
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
		List<int> randomList = new List<Int>;
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
