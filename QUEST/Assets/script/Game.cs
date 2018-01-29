using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

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
	void EndTurn(){
		
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

		_turnId = 1;
		_numPlayers = 3;
		_running = true;
		_drawCardButton = null;
	}

	void Start() {
		_drawCardButton.onClick.AddListener (() => {DrawCard();} );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}



}
