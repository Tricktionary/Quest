﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameObject Card;									//Card Prefab 
	public GameObject Hand; 								//Play Area Hand Reference
	public GameObject drawCardArea;							//DrawCardArea
	public GameObject storyCard;							//Story Card
	public GameObject playerIdTxt;							//Player Id
	public GameObject shieldCounterTxt;						//Shield Counter
	private List<Player> _players = new List<Player>(); 	//List of players
	private int _numPlayers;								//Number of players
	private int _turnId; 									//Player ID of who's turn
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private Deck _discardPileAdventure;
	private Deck _discardPileStory;
	private CardArea _storyArea;
	private CardArea _playArea;
	private bool _running;
	private bool _drawn;

	private List<List<Card>> _Quest;
	private int _questSponsor;
	private List<int> _playersIn;

	//if the user can end their turn
	private bool _standardTurn;
	private bool _canEnd;

	public UnityEngine.UI.Button _drawCardButton; 

	//Draws Card 
	public void DrawCard(){
		//_storyDeck

		if (_drawn == false) {
			//Clears out drawCardArea
			foreach (Transform child in drawCardArea.transform) {	
				GameObject.Destroy (child.gameObject);
			}

			Card currCard = _storyDeck.Draw (); //We need discard pile
			_discardPileStory.Discard(currCard);
			string currCardAsset = currCard._asset; //Pulls the card asset

			Sprite card = Resources.Load<Sprite> (currCardAsset); //Card Sprite

			GameObject questCard = Instantiate (storyCard, new Vector3 (-10.5f, -3.5f, -10.5f), new Quaternion (0, 0, 0, 0));

			questCard.gameObject.GetComponent<Image> ().sprite = card;
			questCard.transform.SetParent (drawCardArea.transform);
			_drawn = true;
		}
	}

	//End Turn
	public void EndTurn(){
		if (_canEnd) {
			//Clear Old Hand
			foreach (Transform child in Hand.transform) {	
				GameObject.Destroy (child.gameObject);
			}

			_turnId++;
			if (_turnId >= 3) {
				_turnId = 0;
			}

			loadHand (_turnId);
			//Debug.Log("End Turn");
			_drawn = false;
			_canEnd = false;
		}
	}

	private void acceptSponsor() {
		_questSponsor = _turnId;
		_canEnd = false;
	}

	// Use this for initialization
	void Awake() {
		//assume 3 players
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));

		//Set up the decks
		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck("Story");
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");

		_turnId = 0;
		_numPlayers = 3;
		_running = true;
		_drawCardButton = null;
		_drawn = false;
		_standardTurn = true;
		_canEnd = true;

		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
		//debugPrint(); 
	}
	
	//Load Player Hand
	void loadHand(int playerId){
		
		List<Card> currCard = _players[playerId]._hand;
		string currCardAsset;

		//Set Player ID text
		playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID : "+ (playerId+1).ToString(); //For User Friendly

		//Get current players shield
		int currPlayerShield = _players[playerId]._shieldCounter;
		shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (currPlayerShield).ToString(); //For User Friendly

		//Create Card Game Object
		for(int i = 0 ; i < currCard.Count; i++){
			currCardAsset = currCard[i]._asset;
			//Debug.Log(currCardAsset);

			GameObject CardUI = Instantiate(Card, new Vector3(-10.5f, -3.5f, -10.5f), new Quaternion(0,0,0,0));
			
			Sprite card = Resources.Load<Sprite>(currCardAsset); //Card Sprite
			
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(Hand.transform);
		}
	}

	//Testing
	void debugPrint(){
		//Populate Hand of All Player
		Debug.Log(_players.Count);
 
		//Test Deck Printing
		Debug.Log("Adventure Deck:");
		for(int i = 0; i <_adventureDeck.GetSize();i++){
			Debug.Log(_adventureDeck.GetDeck()[i].ToString());
		}
		//Test Print Story
		Debug.Log("Story Deck:");
		for(int i = 0; i <_storyDeck.GetSize();i++){
			Debug.Log(_storyDeck.GetDeck()[i].ToString());
		}

		Debug.Log("Player Hands:");
		//Trying to print player hand
		for(int i = 0 ; i <_players.Count; i++){
			Debug.Log("Player:"+ i);
			//Debug.Log(_players[i]._hand.Count);
			for(int x = 0; x <_players[i]._hand.Count; x++){
				Debug.Log(_players[i]._hand[x].ToString());
			}
		}	
	}

	void Start() {
	//	_drawCardButton.onClick.AddListener (() => {DrawCard();} );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}



}
