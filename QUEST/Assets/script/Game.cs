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

	//Draws Card 
	void DrawCard(){
		//_storyDeck
	}

	//End Turn
	void EndTurn(){
		
	}

	// Use this for initialization
	void Start () {
		//assume 3 players
		_players.Add(new Player());
		_players.Add(new Player());
		_players.Add(new Player());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}
}
