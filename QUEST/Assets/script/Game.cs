using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameObject Card;		//Card Prefab 
	public GameObject Hand; 	//Play Area Hand Reference
	private List<Player> _players = new List<Player>(); 	//List of players
	private int _numPlayers;								//Number of players
	private int _turnId; 									//Player ID of who's turn
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private CardArea _storyArea;
	private CardArea _playArea;
	private bool _running;

	public UnityEngine.UI.Button _drawCardButton; 

	//Draws Card 
	public void DrawCard(){
		//_storyDeck
		Debug.Log("draw card click");
	}

	//End Turn
	public void EndTurn(){
		
	}

	// Use this for initialization
	void Awake() {
		//assume 3 players
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));

		//Set up the decks
		_adventureDeck = new Deck(true);
		_storyDeck = new Deck(false);

		_turnId = 0;
		_numPlayers = 3;
		_running = true;
		_drawCardButton = null;
		
		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}

		/* Load Player 1 Cards */
		List<Card> currCard = _players[0]._hand;
		string currCardAsset;
		//Create Card Game Object
		for(int i = 0 ; i < currCard.Count; i++){
			currCardAsset = currCard[i]._asset;
			Debug.Log(currCardAsset);
			GameObject CardUI = Instantiate(Card, new Vector3(-7.5f, -3.5f, -0.5f), new Quaternion(0,0,0,0));
			Sprite card = Resources.Load<Sprite>(currCardAsset);
			
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(Hand.transform);
		}
		
		//debugPrint();
		 
	}

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
		//_drawCardButton.onClick.AddListener (() => {DrawCard();} );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}



}
