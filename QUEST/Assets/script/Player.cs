using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private List<Card> _hand = new List<Card>();		//Players Cards
	private List<Card> _activeHand = new List<Card>();	//Players Hand of cards that are active (Usually allies)
	private int _playerId;		//Player ID
	private int _rank;			//Players current ranks
	private int _shieldCounter; //Shield Counter
	//private bool _inEvent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
