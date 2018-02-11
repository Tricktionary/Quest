using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameObject Card;									//General Card Prefab 
	public GameObject WeaponCard;							//Weapon Card Prefab
	public GameObject FoeCard;								//Foe Card Prefab
	public GameObject AllyCard;								//Ally Card Prefab
	public GameObject QuestCard;

	public GameObject playArea;								//Play Zone
	
	public GameObject Stage1;							    //Stage1 of quest
	public GameObject Stage2;
	public GameObject Stage3;
	public GameObject Stage4;
	public GameObject Stage5;

	public GameObject Sponsor;

	public GameObject drawCardArea;							//DrawCardArea
	public GameObject Hand; 								//Play Area Hand Reference
 
	public GameObject playerIdTxt;							//Player Id
	public GameObject shieldCounterTxt;						//Shield Counter

	private CardArea _storyArea;
	private CardArea _playArea;


	private List<Player> _players = new List<Player>(); 	//List of players
	private int _numPlayers;								//Number of players
	private int _turnId; 									//Player ID of who's turn
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private Deck _discardPileAdventure;
	private Deck _discardPileStory;

	//CURRENT TURN
	private bool _running;
	private bool _drawn;

	private bool _questInPlay;
	private List<List<Card>> _Quest;
	private int _sponsorId;
	private int _questeeTurnId;
	private List<int> _playersIn;
	private string featFoe;  //Could be unused
	private int numStages;
	private int _questSponsor;	

	//if the user can end their turn
	private bool _standardTurn;
	private bool _canEnd;
	public int sponsorOrNot; //used in the sponsor click function 

	

	//Draws Card 
	public void DrawCard(){
		//_storyDeck
		if (_drawn == false) {
			 
			foreach (Transform child in drawCardArea.transform) {	//Clears out drawCardArea
				GameObject.Destroy (child.gameObject);
			}

			Card currCard = _storyDeck.Draw (); //We need discard pile
			
			_discardPileStory.Discard(currCard);
			
			string currCardAsset = currCard.asset; //Pulls the card asset

			Sprite card = Resources.Load<Sprite> (currCardAsset); //Card Sprite

			GameObject storyCard = null;

			if (currCard.GetType () == typeof(QuestCard)){	//Instantiate Quest Card Prefab
				storyCard = Instantiate (QuestCard, new Vector3 (-10.5f, -3.5f, -10.5f), new Quaternion (0, 0, 0, 0));
				_questInPlay = true;
			}

			storyCard.gameObject.GetComponent<Image> ().sprite = card;
			storyCard.transform.SetParent (drawCardArea.transform);
			_drawn = true;

			//THERE IS QUEST IN PLAY
			if(_questInPlay == true){
				QuestCard currQuest = (QuestCard)currCard;
				numStages = currQuest.stages;

				initQuest(_turnId, currQuest);		//Initialize Quest Should ALSO TAKE IN QUEST CARD
			}

			//Check What card i have drawn and initialize a quest
		
		}
	}
	
	IEnumerator waitRoutine() {
        yield return new WaitForSeconds(10);
        print(Time.time);
    }

	//TODO: Check to make sure the play area is successfully filled
	public bool checkQuest() {
		return false;
	}

	//Quest Initialization
	public void initQuest(int currPlayer , QuestCard card){		
		sponsorPopup(currPlayer, sponsorOrNot);
	}

	//End Turn
	public void EndTurn() {
		if (_canEnd) {

			if (_questInPlay) {
				if (_sponsorId >= 0) {
					//TODO: Quest is already sponsored, move on to next player, if it is the last player then let the sponsor do any final actions
					for (int i = 0; i < _playersIn.Count; i++) {
						if (_playersIn [i] == _questeeTurnId) {
							if (i == _playersIn.Count - 1) {
								//TODO: SPONSOR'S TURN
							} else {
								_questeeTurnId = _playersIn [i + 1];
							}
							break;
						}
					}

				} else {
					//TODO: Check if quest pile is successfully filled and if it is then sponsor
					//TODO: Set questeeId turn, run init quest code here
					_questeeTurnId = (_sponsorId + 1) % _numPlayers;
					loadHand(_questeeTurnId);
					_drawn = true;
					_canEnd = true;
				}
			} else {
				//Clear Old Hand
				foreach (Transform child in Hand.transform) {	
					GameObject.Destroy (child.gameObject);
				}

				_turnId++;
				if (_turnId >= 3) {
					_turnId = 0;
				}

				loadHand(_turnId);
				_drawn = false;
				_canEnd = false;
			}
			StartTurn ();
		}
	}

	//redundant function for now. We can use it to move code later from endTurn()
	
	public void StartTurn() {
		if (_questInPlay) {
			if (_sponsorId >= 0) { //was if (_sponsorId) changed to (_sponsorId >= 0) because was giving error
				//TODO: Normal turn, remember to use _questeeId!
			} else {
				//TODO: RUN popup asking if you want to sponsor
			}
		} else {
			
		}
	}
	
		
	//TODO: PHILIPPE: CONVERT these to simply a popup function that asks something and returns true for yes or false for no
	//Load prompt popop
	public bool sponsorPopup(int playerId, int sponsorOrNot){ //used to call sponsor prompt 
		loadHand(playerId);
		//Debug.Log(playerId +" Do you want to sponsor");
		Debug.Log("sponsorOrnot " + sponsorOrNot);
		Sponsor.SetActive(true); //sponsor prompt begins as hidden until set Active when quest card appears 
		//yield return StartCoroutine(WaitAndPrint(2.0F));
		return sponsorPrompt(playerId);
	}

	
	//Prompt Sponsor
	public bool sponsorPrompt(int playerId){
		if(sponsorOrNot == 1){ //using click bool value to know if quest was sponsored 
			Debug.Log("Quest was sponsored");
			Sponsor.SetActive(false);
			createQuest();
			return true;
		}
		else if(sponsorOrNot == 2){
			Debug.Log("Quest was not sponsored");
			return false;

		}
		else{
			return false;
		}
		
	}

	public void createQuest(){
		_questSponsor = _turnId;
		Debug.Log("numStages " + numStages);
		if(numStages == 4){ //alters board based on number of stages of the quest
			Debug.Log("making inactive");
			Stage5.SetActive(false);
		}
		else if(numStages == 3){
			Debug.Log("making inactive");
			Stage4.SetActive(false);
			Stage5.SetActive(false);
		}
		else if(numStages == 2){
			Debug.Log("making inactive");
			Stage3.SetActive(false);
			Stage4.SetActive(false);
			Stage5.SetActive(false);
		}

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
		_drawn = false;
		_standardTurn = true;
		_canEnd = true;
		_questInPlay = false;

		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
		//debugPrint(); 
	}
	
	//Input : Integer Player Id
	//Functionality : Loads player hand onto the UI
	void loadHand(int playerId){
		
		List<Card> currHand = _players[playerId].hand;
		Card currCard;
		

		//Set Player ID text
		playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID : "+ (playerId+1).ToString(); //For User Friendly

		//Get current players shield
		int currPlayerShield = _players[playerId].shieldCounter;
		shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (currPlayerShield).ToString(); //For User Friendly

		//Create Card Game Object
		for(int i = 0 ; i < currHand.Count; i++){
			currCard = currHand[i];

			Hand.GetComponent<CardArea>().addCard(currCard);
			GameObject CardUI = null; 

			if (currCard.GetType () == typeof(WeaponCard)) {
				//Is this convention ?
				WeaponCard currWeapon = (WeaponCard)currCard;
				CardUI = Instantiate (WeaponCard, new Vector3 (-10.5f, -3.5f, -10.5f), new Quaternion (0, 0, 0, 0));
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
				
			}
			if (currCard.GetType () == typeof(FoeCard)) {
				CardUI = Instantiate (FoeCard, new Vector3 (-10.5f, -3.5f, -10.5f), new Quaternion (0, 0, 0, 0));
				FoeCard currFoe = (FoeCard)currCard;
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			}

			if (currCard.GetType () == typeof(AllyCard)) {
				CardUI = Instantiate (AllyCard, new Vector3 (-10.5f, -3.5f, -10.5f), new Quaternion (0, 0, 0, 0));
				CardUI.GetComponent<AllyCard>().name    = currCard.name;
				CardUI.GetComponent<AllyCard>().asset   = currCard.asset;
			}


			Sprite card = Resources.Load<Sprite>(currCard.asset); //Card Sprite

			//Debug.Log(card);
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(Hand.transform);
		}
	}

	//Purpose is for printing deck values
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
			for(int x = 0; x <_players[i].hand.Count; x++){
				Debug.Log(_players[i].hand[x].ToString());
			}
		}	
	}

	public void YesButton_Click(){
		Debug.Log("clicked the Yes button");
		sponsorOrNot = 1;
		sponsorPopup(_turnId, sponsorOrNot);
			
	}

	public void NoButton_Click(){
		Debug.Log("clicked the No button");
		sponsorOrNot = 2;
		sponsorPopup(_turnId, sponsorOrNot);
	}
		
	// Update is called once per frame
	void Update () {

	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}



}
