using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private List<Card> _hand = new List<Card>();	//Players Cards
	private int _playerId;							//Player ID
	private int _rank;								//Players current ranks 0-2; 3 ends the game
	private int _shieldCounter; 					//Shield Counter
	private List<Card> _allies = new List<Card>();  //List of cards
	private int _bp;								//Current BP
	public static int limit = 12;

	// Plays Cards
	void playCard(List<Card> cards){
		
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//s as number of shields to add, should we force rank up?
	void AddShields (int s) {
		_shieldCounter += s;
	}

	void Rankup () {
		int[] requirements = new int[]{ 5, 7, 10 };
		if (_rank == 3) {
			//end game
		}
		while (_shieldCounter >= requirements [_rank]) {
			_shieldCounter -= requirements [_rank];
			_rank++;
			if (_rank == 3) {
				//END GAME HERE
				break;
			}
		}
	}

	//Game should draw and assign the cards
	void DrawCards(List<Card> cards) {
		
	}
}
