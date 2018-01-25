using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private List<Card> _hand = new List<Card>();	//Players Cards
	private int _playerId;							//Player ID
	private int _rank;								//Players current ranks
	private int _shieldCounter; 					//Shield Counter
	private List<Card> _allies = new list<Card>();  //List of cards
	private int _bp;								//Current BP
	public static int limit = 12;

	// Plays Cards
	void playCard(List<card> cards){
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
