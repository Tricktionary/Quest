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

	public GameObject Prompt;								//Prompter
	public GameObject promptTxt;
	public GameObject gameStatus;

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

				_playersIn = new List<int> ();
				for (int i = 0; i < _numPlayers; i++) {
					//all questees in
					_playersIn.Add ((_turnId + i) % _numPlayers);
				}
			}

			storyCard.gameObject.GetComponent<Image>().sprite = card;
			storyCard.transform.SetParent (drawCardArea.transform);
			_drawn = true;
			_canEnd = true;
		
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
				if(_questCard.featuredFoe == currFoe.name){	//Feature Foe Logic 
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
	public bool checkQuest() {
		List<List<Card>> allStages = new List<List<Card>>();
		List<int> powerLevels = new List<int>();

		int currPower = 0;
		if(numStages == 5){
			allStages.Add(Stage1.GetComponent<CardArea>().cards);
			allStages.Add(Stage2.GetComponent<CardArea>().cards);
			allStages.Add(Stage3.GetComponent<CardArea>().cards);
			allStages.Add(Stage4.GetComponent<CardArea>().cards);
			allStages.Add(Stage5.GetComponent<CardArea>().cards);
			
		}
		if(numStages == 4){
			allStages.Add(Stage1.GetComponent<CardArea>().cards);
			allStages.Add(Stage2.GetComponent<CardArea>().cards);
			allStages.Add(Stage3.GetComponent<CardArea>().cards);
			allStages.Add(Stage4.GetComponent<CardArea>().cards);
		}
		if(numStages == 3){
			allStages.Add(Stage1.GetComponent<CardArea>().cards);
			allStages.Add(Stage2.GetComponent<CardArea>().cards);
			allStages.Add(Stage3.GetComponent<CardArea>().cards);
		}
		if(numStages == 2){
			allStages.Add(Stage1.GetComponent<CardArea>().cards);
			allStages.Add(Stage2.GetComponent<CardArea>().cards);
		}
		if(numStages == 1){
			allStages.Add(Stage1.GetComponent<CardArea>().cards); 
		}

		for(int i = 0 ; i < allStages.Count ; i++){
			currPower = stageValid(allStages[i]);	
			Debug.Log(currPower);
			if(currPower == -1){
				return false;
			}
			else{
				powerLevels.Add(currPower);
			}
		}

		//Check Ascending Power Level
		for(int i = 0 ; i < powerLevels.Count - 1 ; i++){
			//Debug.Log(powerLevels[i]);
			if(powerLevels[i] > powerLevels[i+1]){
				return false;
			}
		}

		return true;
	}

	//Quest Initialization
	public void initQuest(){		
		_sponsorId = _turnId;
		_playersIn = new List<int> ();
		for (int i = 1; i < _numPlayers; i++) {
			//all questees in
			_playersIn.Add ((_sponsorId + i) % _numPlayers);
		}
		_questeeTurnId = _playersIn[0];
	}

	//End Turn
	public void EndTurn() {
		if (_canEnd) {

			if (_questInPlay) {			//Quest Currently in plays
				if (_sponsorId >= 0) {  //There is a sponsor
					if(_questReady == false){
						_questReady = checkQuest();	//Quest Validation
						if(_questReady == true){	
							nextTurn(false,false);
							prompt(_turnId,"playQuest"); //Start Asking if other players want to play
							statusPrompt("");
						}
						else{
							statusPrompt("Quest is Bad");
						}
					}
					if(_questReady == true){

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
				}
				if(_sponsorId == -1){ 			//Find A Sponsor
					prompt(_turnId, "sponsor");
				}
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

	public void statusPrompt(string message){
		gameStatus.GetComponent<UnityEngine.UI.Text>().text = message;
	}

	//Handles Prompt Actions
	public void promptReceiv(int answer ,string type){
		if(type == "sponsor"){
			_askCounter++;
			if(_askCounter >= 3){
				Prompt.SetActive(false);
				_askCounter = 0; //Reset
				nextTurn(false,false);
			}
			if(answer == 1){							//Yes
				Prompt.SetActive(false);
				statusPrompt("Please Set Up Quest");
				createQuest(_turnId);					//Someone Has Sponsored
			}
			if(answer == 2){							//No
				nextTurn(false,false);          
			}
		}
		if(type == "playQuest"){
			_askCounter++;
			if(_askCounter >= 3){
				Prompt.SetActive(false);
				_askCounter = 0; //Reset
				nextTurn(false,false);
			}
			if(answer == 1){							//Yes
				_playersIn.Add(_turnId);				//Add if Player says yes
				nextTurn(false,false); 
			}
			if(answer == 2){							//No
				nextTurn(false,false); 
			}
		}
	}
	
	//changes the number of stages based on numStages on the quest card
	public void createQuest(int sponsor){
		_sponsorId = sponsor;
		_questReady = false;	//Quest is not ready yet
		Debug.Log("numStages " + numStages);
		if(numStages == 4){ 
			//Debug.Log("making inactive");
			Stage5.SetActive(false);
		}
		else if(numStages == 3){
			//Debug.Log("making inactive");
			Stage4.SetActive(false);
			Stage5.SetActive(false);
		}
		else if(numStages == 2){
			//Debug.Log("making inactive");
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
