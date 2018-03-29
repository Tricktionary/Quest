using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour {

	public CardFactory(){

	}

	public List<Card> createCardList(string[] cards){
		Deck placeHolderDeck = new Deck (cards);
		return placeHolderDeck.GetDeck ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
