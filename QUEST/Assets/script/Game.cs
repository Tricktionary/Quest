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

	public List<GameObject> Stages;

	public GameObject Prompt;								//Prompter
	public GameObject promptTxt;
	public GameObject gameStatus;

	public GameObject drawCardArea;							//DrawCardArea
	public GameObject Hand; 								//Play Area Hand Reference
 
	public GameObject playerIdTxt;							//Player Id
	public GameObject shieldCounterTxt;						//Shield Counter

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

	private QuestCard _questCard;
	private List<List<Card>> _Quest;
	private int _sponsorId;
	private int _questeeTurnId;
	private List<int> _playersIn; //We are using this to store questees in the quest and before the quest is sponsored, who is next to decide to sponsor
	private string featFoe;       //Could be unused
	private int numStages;

	//if the user can end their turn
	private bool _standardTurn;
	private bool _canEnd;


	public int promptAnswer;
	public int _askCounter;  //How many people you asked this prompt to
	
	private bool _questInPlay;
	private bool _questReady;

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
				storyCard = Instantiate (QuestCard);
				_questInPlay = true;
				_canEnd = true;
				_questCard = (QuestCard)currCard;
				numStages = _questCard.stages;				//Stages of the Quest
				/*
				for (int i = 0; i < _numPlayers; i++) {
					//all questees in
					_playersIn.Add ((_turnId + i) % _numPlayers);
				}
				*/
			}

			storyCard.gameObject.GetComponent<Image>().sprite = card;
			storyCard.transform.SetParent (drawCardArea.transform);
			_drawn = true;
			
		
		}
	}
	

	//TODO: Check to make sure the play area is successfully filled
	public int stageValid(List<Card> currStage){
		bool check = false;
		int foeCount = 0; 
		Card currCard = null;
		int power = 0;
		List<WeaponCard> weapons = new List<WeaponCard>();

		for(int i = 0 ; i < currStage.Count ; i++) {
			currCard = currStage[i];
			if(currCard.GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)currCard;	
				for(int x = 0 ; x < weapons.Count; x++){	//Duplicate Weapons Logic
					if(currWeapon.name == weapons[x].name){
						return -1;
					}
				}
				weapons.Add(currWeapon);
				power = power + currWeapon.power;

			}
			else if(currCard.GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)currCard;

				if(_questCard.featuredFoe == currFoe.type){	//Feature Foe Logic 
					power = power + currFoe.hiPower;
				}
				else if(_questCard.featuredFoe == "*"){
					power = power + currFoe.hiPower;
				}
				else{
					power = power + currFoe.loPower;
				}
				foeCount++;
			}
			else{ 			//Not Valid if you use anything other than weapons and foes (FOR NOW)
				return -1;   
			}
		}

		if(foeCount > 1 || foeCount <= 0){
			return -1;
		}

		return power;
	}

	// Get all the staged cards objects.
	public List<Card> getStagedCards() {
		List<Card> stagedCards = new List<Card> ();
		for (int i = 0; i < numStages; i++) {
			for (int j = 0; j < Stages[i].GetComponent<CardArea>().cards.Count; j++) {
				stagedCards.Add(Stages[i].GetComponent<CardArea>().cards[j]);
			}
		}
		return stagedCards;
	}

	// Get a list of all the stages.
	public List<List<Card>> getStages() {
		List<List<Card>> stages = new List<List<Card>> ();
		for (int i = 0; i < numStages; i++) {
			stages.Add(Stages[i].GetComponent<CardArea> ().cards);
		}
		return stages;
	}

	public bool checkQuest() {
		List<int> powerLevels = new List<int>();

		int currPower = 0;
		List<List<Card>> stages = getStages();

		for (int i = 0; i < stages.Count; i++) {
			currPower = stageValid (stages [i]);
			if(currPower == -1){
				return false;
			} else {
				powerLevels.Add(currPower);
			}
		} 

		// Check ascending power level.
		for(int i = 0; i < powerLevels.Count - 1; i++){
			if(powerLevels[i] > powerLevels[i + 1]){ return false; }
		}

		return true;
	}

	//Update The hand of turn ID based off of the user interface
	public void updateHand(int turnID){
		
		List<Card> tempHand = new List<Card>();
		tempHand = Hand.GetComponent<CardArea>().cards;
		_players[_turnId].hand  = new List<Card>();	
		for(int i = 0 ; i < tempHand.Count ;i++){
			_players[_turnId].addCard(tempHand[i]);
		}

		/*
		List<Card> tempPlay = new List<Card>();
		tempPlay = playArea.GetComponent<CardArea>().cards;
		_players[_turnId].inPlay = new List<Card>();
		_players[_turnId].inPlay = tempPlay;
		*/
	}


	//End Turn
	public void EndTurn() {
		//Debug.Log(_canEnd);
		//Debug.Log(_questInPlay);
		//Debug.Log(_sponsorId);
		//Debug.Log(_questReady);
		if (_canEnd) {
			if (_questInPlay) {			//Quest Currently in plays
				if (_sponsorId >= 0) {  //There is a sponsor
					
					if(_questReady == false){			//Quest is not Ready
						if(checkQuest()){	
							Debug.Log("Quest Here hehe");
							_questReady = true;

							// Flip the staged cards.


							updateHand(_turnId); 		 //Update Sponsor Hand based off of the UI
							nextTurn(true,false);
							prompt(_turnId,"playQuest"); //Start Asking if other players want to play
							statusPrompt("");
						}
						else{
							statusPrompt("Quest is Not Valid");
						}
					}
					if(_questReady == true){			//Quest is Ready
						if(_playersIn.Count > 0){		//If people are participating
							//Debug.Log("Quest In Play");
							//updateHand(_turnId);
							nextTurnQuest();
						}
					}
				}
				if(_sponsorId == -1){ 			//Find A Sponsor
					prompt(_turnId, "sponsor");
				}
					//TODO: Quest is already sponsored, move on to next player, if it is the last player then let the sponsor do any final actions
					//prompt(_turnId,"playQuest");

					/*
					for (int i = 0; i < _playersIn.Count; i++) {
						if (_playersIn [i] == _questeeTurnId) {
							if (i == _playersIn.Count - 1) {
								//TODO: SPONSOR'S TURN
							} else {
								_questeeTurnId = _playersIn [i + 1];
							}
							break;
						}
					}*/
				 
				else {
					//TODO: Check if quest pile is successfully filled and if it is then sponsor

					//TODO: Set questeeId turn, run init quest code here
					/*
					if (checkQuest ()) {
						initQuest ();
						loadHand (_questeeTurnId);
					} else {
						//TODO: Prompt here. if no then:
						//remove [0] from _playersIn
						//load _player[0] (the next player's hand
						_drawn = true;
						_canEnd = true;
					}
					_drawn = false;
					_canEnd = false;
					*/
				}
			}
			else {	//No Quest In Play
				nextTurn(false,false);
			}
		}
	}

/*
	Funtion : Litterally Itterates to the next turn 
	Input: drawn -> Boolean can the cards be drawn
	Input: canEnd -> Can the player end their current turn
*/
	public void nextTurn(bool drawn, bool canEnd){
		//Clear Old Hand
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}


		_turnId++;
		if (_turnId >= 3) {
			_turnId = 0;
		}

		loadHand(_turnId);
		_drawn = drawn;
		_canEnd = canEnd;
	}

/*
	Rotate through questeeTurnId

*/
	public void nextTurnQuest(){
		bool revealStage;
		
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		
		foreach (Transform child in playArea.transform) {	
			GameObject.Destroy (child.gameObject);
		}

		if(_questeeTurnId == -1){ //Instantiate
			_questeeTurnId = 0;
		}
		else{
			_questeeTurnId++;
		}
		
		if(_questeeTurnId >= _playersIn.Count){
			_questeeTurnId = 0;
		}

		_turnId =  _playersIn[_questeeTurnId]; 
		loadHand(_turnId);
		_drawn = true;	//Can't Draw cards while in quest
		_canEnd = true; //Can End turn
	}
/*
	Prompt User
	Input: turnID-> Integer The turn ID we are prompting (Not really being used)
	Input: messageType -> String The type of message we are tossing to the user 
*/
	public void prompt(int turnId, string messageType){
		if(messageType == "sponsor"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Sponsor This Quest?";
		}
		if(messageType == "playQuest"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Join the Quest?";
		}
	}

	//User Status Prompt
	public void statusPrompt(string message){
		gameStatus.GetComponent<UnityEngine.UI.Text>().text = message;
	}

	//Reset Values
	public void reset(){
		_playersIn = new List<int>(); //Reset Players In
		_questReady = false;
		_questInPlay = false;
		_sponsorId = -1;
		_questeeTurnId = -1;

		// Clean the stages..
		for (int i = 0; i < Stages.Count; i++) {
			Stages[i].SetActive(true);
			Stages[i].GetComponent<CardArea>().cards = new List<Card>();
			// Clears out draw card area.
			foreach (Transform child in Stages[i].transform) {
				GameObject.Destroy(child.gameObject);
			}
		}
	}


	//Handles Prompt Actions
	public void promptReceiv(int answer ,string type){
		if(type == "sponsor"){
			_askCounter++;
			if(answer == 1){							//Yes
				Prompt.SetActive(false);
				statusPrompt("Please Set Up Quest");
				createQuest(_turnId);					//Someone Has Sponsored
				_canEnd = true;
				_drawn = true;
				_askCounter = 0;
			}
			if(answer == 2){							//No
				if(_askCounter < _numPlayers){
					nextTurn(false,false);
				}
				else{									//No Sponsor
					_askCounter = 0;
					reset();
					_turnId++;							//Skip The original draw turn
					if (_turnId >= 3) {
						_turnId = 0;
					}
					nextTurn(false,false);
					Prompt.SetActive(false);
					Debug.Log(_turnId);
				}
			}
		}

		if(type == "playQuest"){
			_askCounter++;
			if(answer == 1){								//Yes
				_playersIn.Add(_turnId);					//Add if Player to _playersIn

				if(_askCounter < (_numPlayers - 1)){				//Continue Asking
					Debug.Log("Here3");
					nextTurn(false,false); 		
				}	
				else{										//Done Asking
					//Change To QuesteeTurns
					statusPrompt("Setup your Weapons");
					Debug.Log("Here4");
					nextTurnQuest();
					Prompt.SetActive(false);
				}		
			}
			else if(answer == 2){							//No
				if(_askCounter < (_numPlayers - 1)){				//Continue asking
					Debug.Log("Here5");
					nextTurn(false,false); 		
				}	
				else{										//Done Asking
					_askCounter = 0;
					_turnId++;								//Skip Sponsor
					if (_turnId >= 3) {
						_turnId = 0;
					}
					if(_playersIn.Count <= 0){				//No on joined so reset
						reset();
						nextTurn(false,false);
					}
					else{									//Someone joined
						Debug.Log("Here6");
						nextTurn(true,true);
					}
					Prompt.SetActive(false);				//Turn The Prompt Off
				}
			}
		}
	}
	
	//changes the number of stages based on numStages on the quest card
	public void createQuest(int sponsor){
		_playersIn = new List<int> ();
		_sponsorId = sponsor;
		_questReady = false;	//Quest is not ready yet
		Debug.Log("numStages " + numStages);

		for (int i = 0; i < (5 - numStages); i++) {
			Stages[4-i].SetActive(false);
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
		_canEnd = false;
		_questInPlay = false;

		_sponsorId = -1;
		_questeeTurnId = -1;
		_askCounter = 0;

		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
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
				CardUI = Instantiate (WeaponCard);
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
				
			}
			if (currCard.GetType () == typeof(FoeCard)) {
				 
				FoeCard currFoe = (FoeCard)currCard;
				CardUI = Instantiate (FoeCard);
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().type    = currFoe.type;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			}

			if (currCard.GetType () == typeof(AllyCard)) {

				AllyCard currAlly = (AllyCard)currCard;
				CardUI = Instantiate (AllyCard);
				CardUI.GetComponent<AllyCard>().name    = currAlly.name;
				CardUI.GetComponent<AllyCard>().asset   = currAlly.asset;
			}


			Sprite card = Resources.Load<Sprite>(currCard.asset); //Card Sprite

			//Debug.Log(card);
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(Hand.transform);

			// Set the cards obj to it's UI.
			// NOTE: There is probably a better built in Unity way to do this,
			// if so, we should definitely swap it out.
			currCard.obj = CardUI;
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

	//click function that changed the value of sponsorOrNot 
	//and calls the sponsorPrompt function which closes the prompt window and calls createQuest
	public void YesButton_Click(){
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Sponsor This Quest?"){
			//Debug.Log("clicked the Yes button in Sponsor");
			promptAnswer = 1;
			promptReceiv(promptAnswer,"sponsor");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join the Quest?"){
			//Debug.Log("clicked the Yes button in joinQuest");
			promptAnswer = 1;
			promptReceiv(promptAnswer,"playQuest");
		}
	}

	//click function that changed the value of sponsorOrNot 
	public void NoButton_Click(){
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Sponsor This Quest?"){
			//Debug.Log("clicked the No button in Sponsor");
			promptAnswer = 2;
			promptReceiv(promptAnswer,"sponsor");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join the Quest?"){
			//Debug.Log("clicked the No button in joinQuest");
			promptAnswer = 2;
			promptReceiv(promptAnswer,"playQuest");
		}
	}
		
	// Update is called once per frame
	void Update () {
	}

	void TwoPlayerMode() {
		
	}

	void ThreePlayerMode() {

	}



}
