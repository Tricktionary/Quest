using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	protected List<Card> _hand;
	protected List<Card> _inPlay;
	protected int _playerId;							//Player ID
	protected int _rank;								//Players current ranks 0-2; 3 ends the game
	protected int _shieldCounter;      				//Shield Counter
	protected List<Card> _allies;  //List of cards
	protected int _bp;								//Current BP
	public static int limit = 12;


	public List<Card> inPlay{
		get{
			return this._inPlay;
		}
		set{
			this._inPlay = value;
		}
	}


	//gets and sets for all vars
	public List<Card> hand{
		get{
			return this._hand;
		}
		set{
			this._hand = value;
		}
	}


	public int playerId{
		get{
			return this._playerId;
		}
		set{
			this._playerId = value;
		}
	}

	public int rank{
		get{
			return this._rank;
		}
		set{
			this._rank = value;
		}
	}

	public int shieldCounter{
		get{
			return this._shieldCounter;
		}
		set{
			this._shieldCounter = value;
		}
	}

	public List<Card> allies{
		get{
			return this._allies;
		}
		set{
			this._allies = value;
		}
	}

	public int bp{
		get{
			return this._bp;
		}
		set{
			this._bp = value;
		}
	}

	//Constructor 
	public Player(int id){
		_playerId = id;
		_rank = 1;
		_shieldCounter = 0;
		_bp = 5;
		_hand = new List<Card>();
		_allies = new List<Card>();
	}
	
	//AI
	public Player(){
		_playerId = -1;
		_rank = 1;
		_shieldCounter = 0;
		_bp = 5;
		_hand = new List<Card>();
		_allies = new List<Card>();	
	}


	//Add Cards
	public void addCard(Card card){
		_hand.Add(card);	//Adds card to hand
	}
	
	public void addPlayCard(Card card){
		_inPlay.Add(card);
	}
	
	//s as number of shields to add, should we force rank up?
	void AddShields (int s) {
		_shieldCounter += s;
	}

/* 
	Rank-up:
		Squire = 5 BP
		Knight = 10 BP
		Champion Knight = 20 BP
*/
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
