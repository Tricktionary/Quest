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
	public GameObject currStageTxt;

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

	//GENERAL
	private bool _drawn;
	private bool _canEnd;
	public int promptAnswer;
	public int _askCounter;  //How many people you asked this prompt to

	//QUEST RELATED 
	private QuestCard _questCard;
	private int _sponsorId;
	private int _questeeTurnId;
	private List<int> _playersIn; //We are using this to store questees in the quest and before the quest is sponsored, who is next to decide to sponsor
	private List<int> _deadPlayers;
	private string featFoe;       //Could be unused
	private int numStages;
	private bool _questInPlay;
	private bool _questReady;
	private int _currQuestStage;
	private List<int> _stagePower;
	private bool _rumble;
	
	//TOURNAMENT RELATED
	private bool _tournamentInPlay;
	private TournamentCard _tournamentCard;
	private bool _tournamentPrompt;
	private int _tourneeID;
	private int _tourneeWinner;

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
			}

			if(currCard.GetType() == typeof(TournamentCard)){
				storyCard = Instantiate(QuestCard);
				//Debug.Log("TournamentCard");
				_tournamentInPlay = true;
				_tournamentCard = (TournamentCard)currCard;
				_canEnd = true;


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

	public bool playAreaValid(List<Card> cards){
		for(int i = 0 ; i < cards.Count ; i++){
			if(cards[i].GetType() == typeof(FoeCard)){
				return false;
			} 
		}
		return true;
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

		_stagePower = powerLevels;		//Get Calculated stage powers if valid
		return true;
	}

	//Update The hand of turn ID based off of the user interface
	public void updateHand(int turnID){
		
		//Update the players Hand 
		List<Card> tempHand = new List<Card>();
		tempHand = Hand.GetComponent<CardArea>().cards;
		_players[_turnId].hand  = new List<Card>();	
		for(int i = 0 ; i < tempHand.Count ;i++){
			_players[_turnId].addCard(tempHand[i]);
		}

		//Update the players play area
		List<Card> tempPlay = new List<Card>();
		tempPlay = playArea.GetComponent<CardArea>().cards;
		_players[_turnId].inPlay = new List<Card>();
		for(int i = 0 ; i < tempPlay.Count ;i++){
			_players[_turnId].addPlayCard(tempPlay[i]);
		}
		
	}

	//Did the player surive the stage
	public bool didYouSurvive(List<Card> cards){
		int power = 0;
		for(int i = 0 ; i < cards.Count ; i++){
			if(cards[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)cards[i];	
				power = power + currWeapon.power;
			}
			if(cards[i].GetType() == typeof(AllyCard)){
				AllyCard currAlly = (AllyCard)cards[i];
				power = power + currAlly.power;
			}
		}
		if(_players[_turnId].rank == 0 ){
			power = power + 5;
		}
		else if( _players[_turnId].rank == 1){
			power = power + 10;
		}
		else if(_players[_turnId].rank == 2){
			power = power + 20;
		}


		if(power > _stagePower[_currQuestStage]){
			return true;
		}
		else{
			return false;
		}
	}
	
	private void clearWeapons(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards = new List<Card>();
		
		for(int i = 0 ; i < oldCards.Count ; i++){
			if(oldCards[i].GetType() != typeof(WeaponCard)){
				filteredCards.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}
		
		playArea.GetComponent<CardArea>().cards = new List<Card>();
		
		for(int i =0 ; i < filteredCards.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards[i]);
		}

		//Debug.Log("Clears Weapons");
	}

	private int calculateTournamentWinner(){
		List<Card> currHand = new List<Card>();
		
		int currMax = 0;
		int currPower = 0;
		int winnerId = 0;

		for(int i = 0; i < _playersIn.Count; i++ ){
			currHand = _players[_playersIn[i]].hand;
			for(int x = 0 ; x < currHand.Count ; x++){
				if(currHand[i].GetType() == typeof(WeaponCard)){
					WeaponCard currWeapon = (WeaponCard)currHand[i];
					currPower = currWeapon.power + currPower;
				}	
				if(currHand[i].GetType() == typeof(AllyCard)){
					AllyCard currAlly = (AllyCard)currHand[i];
					currPower = currAlly.power + currPower ; 
				}
				currPower = currPower + _players[_playersIn[i]].bp;
			}
			if(currPower > currMax){
				currMax = currPower;
				winnerId = _playersIn[i];
				currPower = 0;
			}
		}
		Debug.Log(winnerId);
		return winnerId;
	}

	//End Turn
	public void EndTurn() {
		//Debug.Log(_canEnd);
		//Debug.Log(_questInPlay);
		//Debug.Log(_sponsorId);
		//Debug.Log(_questReady);
		//Debug.Log(_playersIn.Count);

		if (_canEnd) {
			if (_questInPlay) {				//Quest Currently in plays
				if (_sponsorId >= 0) {  //There is a sponsor
					
					if(_questReady == false){			//Quest is not Ready
						if(checkQuest()){	
							_questReady = true;

							int cardsUsed; //Get the number of cards used per stage


							// Flip the staged cards.
							/*
							List<Card> stagedCards = getStagedCards();
							for (int i = 0; i < stagedCards.Count; i++) {
								stagedCards[i].flipCard(true);
							}
							*/
							//Get the ammount of cards used in this stage and refund to sponsor

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
							

							if(playAreaValid(playArea.GetComponent<CardArea>().cards)){	//PlayZone is valid
								updateHand(_turnId);
								statusPrompt("");
								_askCounter++;	
								if(_askCounter > _playersIn.Count){
									if(_rumble == true){	//Done Checking all Questees go back to setup
									 
										for(int i = 0 ; i < _deadPlayers.Count; i++){				//Removing Dead players
											_playersIn.Remove(_deadPlayers[i]);
										}
										if(_playersIn.Count <= 0){ 									//Everyone is dead So reset
											clearWeapons();
											updateHand(_turnId);
											reset();
											nextTurn(false,false);
											_askCounter = 0;
											currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "";
											return;
										}
										else{														//Continue
											_rumble = false;
											_askCounter = 1;
											_currQuestStage++;
											currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "Current Stage: "+ (_currQuestStage+1).ToString();
											if(_currQuestStage >= numStages){	//Quest is Over
												currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "";
												statusPrompt("Quest Was Done successfully");
												for(int i = 0 ; i < _playersIn.Count ; i++){
													_players[_playersIn[i]].AddShields(numStages);
												}
												clearWeapons();
												updateHand(_turnId);
												reset();
												nextTurn(false,false);
												_askCounter = 0;
												return;
											}
										}
									}	
									else{
										_rumble = true;
										_askCounter = 1;
									}
								}

								if(_rumble == true){
									nextTurnQuest();
									if(didYouSurvive(playArea.GetComponent<CardArea>().cards)){		//YOU WON THE STAGE
										statusPrompt("You Lived!");
										clearWeapons();
										updateHand(_turnId);
 									}
									else{
										statusPrompt("You Have Perished R.I.P");					//YOU DIED 
										clearWeapons();
										updateHand(_turnId);
										for(int i = 0 ; i < _playersIn.Count; i++){
											if(_playersIn[i] == _turnId){
												_deadPlayers.Add(_playersIn[i]);
											}
										}
									}
									 
								}

							 
								else{										//SETUP
									updateHand(_turnId);
									nextTurnQuest();
									statusPrompt("Setup Your Weapons");
								}
							}	
							else{						//Not Valid
								statusPrompt("Play Zone is Not Valid");
							}
						}
						else{
							//reset();
							//nextTurn(false,false);
						}
					}
				}
				if(_sponsorId == -1){ 			//Find A Sponsor
					prompt(_turnId, "sponsor");
				}
			}

			else if(_tournamentInPlay){		//Tournament is in play
				

				if(_tournamentPrompt == false){
					prompt(_turnId,"playTournament");
				}

				else if (_tournamentPrompt == true){
					if(_playersIn.Count > 1 ){					//Players Have joined
						if(_askCounter >= _playersIn.Count){	//Everyone has setup their weapons
							if(_rumble == false){  
								_rumble = true;
								updateHand(_turnId);
								_tourneeWinner = calculateTournamentWinner();
								_askCounter = 0;
							}
						}
						if(_rumble == false){
							if(playAreaValid(playArea.GetComponent<CardArea>().cards)){
								statusPrompt("");
								updateHand(_turnId);
								_askCounter++;
								nextTurnTournament();
							}
							else{
								statusPrompt("Play Area is not valid");

							}
						}
						else{
							nextTurnTournament();
							if(_askCounter>= _playersIn.Count){
								statusPrompt("");
								reset();
								_askCounter = 0;
								nextTurn(false,false);
							}
							if(_turnId == _tourneeWinner){
								_askCounter++;
								statusPrompt("Winner");
								_players[_turnId].AddShields(_tournamentCard.shields + _playersIn.Count);
							}
							else{
								_askCounter++;
								statusPrompt("Lmao you lost");
							}
						}
					}
				}
			}
			else {						//Other
				nextTurn(false,false);
			}
		}
	}
	/*
		Tournament
	*/
	public void nextTurnTournament(){
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		
		foreach (Transform child in playArea.transform) {	
			GameObject.Destroy (child.gameObject);
		}



		if(_tourneeID == -1){ //Instantiate
			_tourneeID = 0;
		}
		else{
			_tourneeID++;
		}
		if(_tourneeID >= _playersIn.Count){
			_tourneeID = 0;
		}

		_turnId =  _playersIn[_tourneeID]; 
		loadHand(_turnId);
		_drawn = true;	//Can't Draw cards while in quest
		_canEnd = true; //Can End turn
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
		foreach (Transform child in playArea.transform) {	
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
		if(messageType == "playTournament"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Join This Tournament?";
		}
	}

	//User Status Prompt
	public void statusPrompt(string message){
		gameStatus.GetComponent<UnityEngine.UI.Text>().text = message;
	}

	//Reset Values
	public void reset(){
		_deadPlayers = new List<int>();
		_playersIn = new List<int>(); //Reset Players In
		_questReady = false;
		_questInPlay = false;
		_rumble = false;
		_sponsorId = -1;
		_questeeTurnId = -1;
		_currQuestStage = -1;
		_questCard = null;
		_tournamentCard = null;
		_tournamentInPlay = false;
		numStages = -1;	
		_tournamentPrompt = false;
		_tourneeID = -1;
		_tourneeWinner = -1;
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
					nextTurn(true,false);
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
					//Debug.Log(_turnId);
				}
			}
		}

		if(type == "playQuest"){
			_askCounter++;

			if(answer == 1){								//Yes
				_playersIn.Add(_turnId);					//Add if Player to _playersIn

				if(_askCounter < (_numPlayers - 1)){		//Continue Asking
					//Debug.Log("Here3");
					nextTurn(true,false);					//True Because player shouldn't be able to draw cards 		
				}	
				else{										//Done Asking
					//Change To QuesteeTurns
					_askCounter = 1;
					statusPrompt("Setup your Weapons");
					_currQuestStage = 0;
					currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "Current Stage: "+ (_currQuestStage+1).ToString();
					//Debug.Log("Here4");
					nextTurnQuest();
					Prompt.SetActive(false);
				}		
			}
			else if(answer == 2){							//No
				
				if(_askCounter < (_numPlayers - 1)){		//Continue asking
					//Debug.Log("Here5");
					nextTurn(true,false); 		
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
						//Change To QuesteeTurns
						_askCounter = 1;
						statusPrompt("Setup your Weapons");
						_currQuestStage = 0;
						currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "Current Stage: "+ (_currQuestStage+1).ToString();
						//Debug.Log("Here4");
						nextTurnQuest();
						Prompt.SetActive(false);
					}
					Prompt.SetActive(false);				//Turn The Prompt Off
				}
			}
		}

		if(type == "playTournament"){
			_askCounter++;
			
			if(answer == 1){
				
				_playersIn.Add(_turnId);
				nextTurn(true,false);
				
				if(_askCounter >= _numPlayers){	//Asked all the players
					if(_playersIn.Count <= 1){	//Not Enough Players
						if(_playersIn.Count == 1){
							_players[_playersIn[0]].AddShields(_tournamentCard.shields);
						}
						_askCounter = 0;
						reset();
						nextTurn(false,false);
						Prompt.SetActive(false); 
					}
					else{
						_askCounter = 1;
						_tournamentPrompt = true;
						Prompt.SetActive(false); 
						nextTurnTournament();
						statusPrompt("Setup Your Weapons");
					}
				}
			}

			else if(answer == 2){	
				nextTurn(true,false);
				if(_askCounter >= _numPlayers){	//Asked all the players
					if(_playersIn.Count <= 1){	//Not Enough Players
						if(_playersIn.Count == 1){
							_players[_playersIn[0]].AddShields(_tournamentCard.shields);
						}
						nextTurn(false,false);
						_askCounter = 0;
						reset();
						Prompt.SetActive(false); 
					}
					else{
						_askCounter = 1;
						_tournamentPrompt = true;
						Prompt.SetActive(false);
						nextTurnTournament();
						statusPrompt("Setup Your Weapons");
					}
				}
			}
		}
	}
	
	//changes the number of stages based on numStages on the quest card
	public void createQuest(int sponsor){
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

		_playersIn = new List<int> ();
		_deadPlayers = new List<int>();

		_turnId = 0;
		_numPlayers = _players.Count;
		_drawn = false;
		_canEnd = false;
		_questInPlay = false;
		_rumble = false;
		_tournamentInPlay = false;

		_sponsorId = -1;
		_questeeTurnId = -1;
		_askCounter = 0;
		_currQuestStage = -1;	
		promptAnswer = -1;

		_tourneeID = -1;

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
		

		playArea.GetComponent<CardArea>().cards =  new List<Card>();
		Hand.GetComponent<CardArea>().cards =  new List<Card>();

		 

		//Set Player ID text
		playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID : "+ (playerId+1).ToString(); //For User Friendly

		//Get current players shield
		int currPlayerShield = _players[playerId].shieldCounter;
		shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (currPlayerShield).ToString(); //For User Friendly

		List<Card> currHand = _players[playerId].hand;
		List<Card> currPlay = _players[playerId].inPlay;
		
		loadCards(currHand,Hand);
		loadCards(currPlay,playArea);

	}

	void loadCards(List<Card> cards, GameObject area){
		Card currCard;
		//Create Card Game Object
		for(int i = 0 ; i < cards.Count; i++){
			currCard = cards[i];

			area.GetComponent<CardArea>().addCard(currCard);
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
				CardUI.GetComponent<AllyCard>().special = currAlly.special;
				CardUI.GetComponent<AllyCard>().power   = currAlly.power;
				CardUI.GetComponent<AllyCard>().bid     = currAlly.bid;
				CardUI.GetComponent<AllyCard>().bonusPower = currAlly.bonusPower;
				CardUI.GetComponent<AllyCard>().bonusBid  =  currAlly.bonusBid;
				CardUI.GetComponent<AllyCard>().questCondition = currAlly.questCondition;
				CardUI.GetComponent<AllyCard>().allyCondition  = currAlly.allyCondition;
			}
				
			Sprite card = Resources.Load<Sprite>(currCard.asset); //Card Sprite

			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(area.transform);

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
		promptAnswer = 1;
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Sponsor This Quest?"){
			promptReceiv(promptAnswer,"sponsor");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join the Quest?"){
			promptReceiv(promptAnswer,"playQuest");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join This Tournament?"){
			promptReceiv(promptAnswer,"playTournament");
		}
	}

	//click function that changed the value of sponsorOrNot 
	public void NoButton_Click(){
		promptAnswer = 2;
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Sponsor This Quest?"){			 
			promptReceiv(promptAnswer,"sponsor");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join the Quest?"){
			promptReceiv(promptAnswer,"playQuest");
		}
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == "Do You Want To Join This Tournament?"){
			promptReceiv(promptAnswer,"playTournament");
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
