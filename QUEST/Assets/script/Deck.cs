using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	private List<Card> cards = new List<Card>();

	//Constructor 
	public Deck(bool isAdventure, bool isStory){
		if(isAdventure){
			//Fill Card with adventure cards
		}
		if(isStory){
			//Fill Cards with story cards
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
