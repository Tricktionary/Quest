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
	public GameObject AmourCard;
	public GameObject TestCard;

	public GameObject EventCard;
	public GameObject RankCard;
	public GameObject playerPanel;

	public GameObject playArea;								//Play Zone
	public GameObject Menu;


	public List<GameObject> Stages;
	public List<GameObject> playerActive;
	public List<GameObject> numCardText;
	public List<GameObject> shieldCounterList;
	public List<GameObject> rankTextList;


	public GameObject Prompt;								//Prompter
	public GameObject promptTxt;
	public GameObject gameStatus;
	public GameObject currStageTxt;
	public GameObject discardPile;
	public GameObject rankCardArea;


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
	private EventCard _eventCard;
	public bool bonusQuestPoints = false; //used for event card "King's Recognition"
	private int nextTurnID;

	//AI
	private List<AIPlayer> Observers;


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
	private int testStage;
	private bool _testStagePlay;
	private bool _testTime;
	private List<int> _testWinners;
	
	//TOURNAMENT RELATED
	private bool _tournamentInPlay;
	private TournamentCard _tournamentCard;
	private bool _tournamentPrompt;
	private int _tourneeID;
	private List<int> _tourneeWinners;

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
				nextTurnID = _turnId;
				storyCard = Instantiate (QuestCard);
				_questInPlay = true;
				_canEnd = true;
				_questCard = (QuestCard)currCard;
				numStages = _questCard.stages;				//Stages of the Quest
			}
			else if(currCard.GetType() == typeof(TournamentCard)){
				nextTurnID = _turnId;
				storyCard = Instantiate(QuestCard);
				//Debug.Log("TournamentCard");
				_tournamentInPlay = true;
				_tournamentCard = (TournamentCard)currCard;
				_canEnd = true;
			}
			else if (currCard.GetType() == typeof(EventCard)){
				storyCard = Instantiate (EventCard);
				_eventCard = (EventCard)currCard;

				//Debug.Log(_eventCard.conditions);
				//condition for "lowest rank and shield receives 3 shields" event 
				if(_eventCard.conditions == "lowest rank and shield receives 3 shields"){
					//Debug.Log("inside conditions");
					int lowestVal = 30;
					int lowerCount = 0;
					int currPlayer = _turnId;
					//Debug.Log("currPlayer: " + currPlayer);
					List<int> lowestPlayers = new List<int>();	
					//compares all players to find the players with lowest value
					for(int i = 0; i< _numPlayers; i++){
						
							if(_players[i].calcRankShields() <= lowestVal){
									lowestVal = _players[i].calcRankShields();
									Debug.Log("lowest being added: " + lowestVal + " to player " + i);
									//Debug.Log("currPlayer: " +currPlayer);
									lowestPlayers.Add(i); 
							}
						
					}

					for(int i = 0; i<lowestPlayers.Count; i++){
						Debug.Log("lowest players: " + lowestPlayers[i]);
						_players[lowestPlayers[i]].shieldCounter += 3; //adds 3 shields to players with "lowestVal"
					}

					//Debug.Log("lowestVal: " + lowestVal);
					//_players[lowestPlayer].shieldCounter += 3; //adds 3 shields to player with "lowestVal"
					_canEnd = true;
			
				}
				else if(_eventCard.conditions == "All players except player drawing this card lose 1 shield"){//condition for "lowest rank and shield receives 3 shields" event 
					for(int i = 0; i < _numPlayers; i++){
						if(_players[i] != _players[_turnId]){ //checks that player isnt current player 
							if(_players[i].shieldCounter != 0){ //checks that player has at least 1 shield
								_players[i].shieldCounter--;
							}
						}
					}
				
					_canEnd = true;
				}
				else if(_eventCard.conditions == "Drawer loses 2 shields if possible"){
					if(_players[_turnId].shieldCounter >= 2){
						_players[_turnId].shieldCounter = _players[_turnId].shieldCounter-2;
					}
					_canEnd = true;
				}
				else if(_eventCard.conditions == "The next player(s) to complete a quest will receive 2 extra shields"){
					bonusQuestPoints = true;
					_canEnd = true;
				}
				else if(_eventCard.conditions == "The lowest ranked player(s) immediately receives 2 Adventure cards"){
					//same implementation as "chivalrous deed" to find lowest players 
					int lowestVal = 30; 
					int lowerCount = 0;
					int currPlayer = _turnId;
					//Debug.Log("currPlayer: " + currPlayer);
					List<int> lowestPlayers = new List<int>();	
					//compares all players to find the players with lowest value

					for(int i = 0; i<_numPlayers; i++){					
							if(_players[i].calcRankShields() <= lowestVal){
									lowestVal = _players[i].calcRankShields();
									Debug.Log("lowest being added: " + lowestVal + " to player " + i);
									//Debug.Log("currPlayer: " +currPlayer);
									lowestPlayers.Add(i); 						
							}		
					}

					Debug.Log("lowestPlayers count: " + lowestPlayers.Count);
					//for(int i = 0; i<lowestPlayers.Count )

					for(int i = 0; i < lowestPlayers.Count ; i++){
						Debug.Log("lowest player: " + lowestPlayers[i]); 
						for(int x = 0 ; x < 2 ; x++){
							_players[lowestPlayers[i]].addCard((_adventureDeck.Draw()));
						}
					}

					_canEnd = true;
				}
				else if(_eventCard.conditions == "All Allies in play must be discarded"){
		
				}
				else if(_eventCard.conditions == "Highest ranked player(s) must discard 1 weapon, if unable 2 foe cards must be discarded"){
					//need to implement discard 
				}
				else if(_eventCard.conditions == "All players may immediately draw 2 adventure Cards"){
					for(int i = 0; i < _players.Count ; i++){
						for(int x = 0 ; x < 2 ; x++){
							_players[i].addCard((_adventureDeck.Draw()));
						}
					}

					_canEnd = true;
				}



			

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
		int testCounter = 0;
		bool testStage = false;

		for(int i = 0 ; i < currStage.Count ; i++){
			currCard = currStage[i];
			if(currCard.GetType() == typeof(TestCard)){
				testStage = true;
				break;
			}
		}
		
		if(testStage){
			if(currStage.Count == 1){
				return 1;
			}
			else{
				return -1;
			}
		}
		else {
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
		int testCounter = 0;
		int currPower = 0;
		List<List<Card>> stages = getStages();
		int pTestStage = -1 ;

		for (int i = 0; i < stages.Count; i++) {
			currPower = stageValid (stages [i]);
			if(currPower == -1){
				Debug.Log("Break1");
				return false;
			} else {
				powerLevels.Add(currPower);
			}
		} 
		
		for(int i = 0; i < powerLevels.Count; i++){
			if(powerLevels[i] == 1){
				pTestStage = i;
				testCounter++;
				if(testCounter > 1){
					Debug.Log("Break2");
					return false;
				}
			}
		}
		testStage = pTestStage;

		for(int i = 0 ; i < powerLevels.Count; i++){
			powerLevels.Remove(1);
		}


		// Check ascending power level.
		for(int i = 0; i < powerLevels.Count - 1; i++){
			if(powerLevels[i] >= powerLevels[i + 1]){ Debug.Log("Break3"); return false; }	//Can't be equal
		}
		_stagePower = powerLevels;		//Get Calculated stage powers if valid
		
		for(int i = 0 ; i < _stagePower.Count ; i++){
			Debug.Log(_stagePower[i]);
		}
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
			if(cards[i].GetType() == typeof(AmourCard)){
				AmourCard currAmour = (AmourCard)cards[i];
				power = power + currAmour.power;
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
	private void clearQuestCards(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards1 = new List<Card>();
		List<Card> filteredCards2 = new List<Card>();
		for(int i = 0 ; i < oldCards.Count ; i++){
			if(oldCards[i].GetType() != typeof(AmourCard)){
				filteredCards1.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}
		for(int i = 0 ; i < filteredCards1.Count ; i++){
			if(filteredCards1[i].GetType() != typeof(WeaponCard)){
				filteredCards2.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(filteredCards1[i]);
			}
		}

		playArea.GetComponent<CardArea>().cards = new List<Card>();
		
		for(int i =0 ; i < filteredCards2.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards2[i]);
		}
	}

	private List<int> calculateTournamentWinner(){
		List<Card> currHand = new List<Card>();
		
		int currMax = 0;
		int currPower = 0;
		List<int> powerLevels = new List<int>();
		List<int> winnersId = new List<int>();

		for(int i = 0; i < _playersIn.Count; i++ ){				//Looping Through all players
			
			currHand = _players[_playersIn[i]].inPlay;			//Gets players Hand
			currPower = currPower + _players[_playersIn[i]].bp; // Rank Power
			

			for(int x = 0 ; x < currHand.Count ; x++){			//Loop Players Hand
				
				if(currHand[x].GetType() == typeof(WeaponCard)){
					WeaponCard currWeapon = (WeaponCard)currHand[x];
					//Debug.Log(currWeapon.power);
					currPower = currWeapon.power + currPower;
				}	
				if(currHand[x].GetType() == typeof(AmourCard)){
					AmourCard currAmour = (AmourCard)currHand[x];
					//Debug.Log(currWeapon.power);
					currPower = currAmour.power + currPower;
				}	

				if(currHand[x].GetType() == typeof(AllyCard)){
					AllyCard currAlly = (AllyCard)currHand[x];
					currPower = currAlly.power + currPower ; 
				}
				

			}
			//Debug.Log(currPower);
			powerLevels.Add(currPower);
			currPower = 0;
		}

		for(int i = 0 ; i < powerLevels.Count ; i++){
			Debug.Log(powerLevels[i]);
			if(powerLevels[i] > currMax){
				currMax = powerLevels[i];			//Gets current max
			}
		}

		for(int i = 0 ; i < powerLevels.Count ; i++){
			if(powerLevels[i] == currMax){
				winnersId.Add(_playersIn[i]);
				//Debug.Log(_playersIn[i]);
			}
		}

		return winnersId;
	}

	private List<int> calculateTestWinners(){
		List<int> winnersId = new List<int>();
		winnersId.Add(0);
		winnersId.Add(1);
		winnersId.Add(2);
		return winnersId;
	}

	private void discardCard(){
		List<Card> cards = discardPile.GetComponent<CardArea>().cards;
		for(int i = 0 ; i <cards.Count ; i++){
			_discardPileAdventure.Discard(cards[i]);
		}
		discardPile.GetComponent<CardArea>().cards = new List<Card>();

		foreach (Transform child in discardPile.transform) {	
			GameObject.Destroy (child.gameObject);
		}
	}

	//End Turn
	public void EndTurn() {
		//Debug.Log(_canEnd);
		//Debug.Log(_questInPlay);
		//Debug.Log(_sponsorId);
		//Debug.Log(_questReady);
		//Debug.Log(_playersIn.Count);
		//Debug.Log(_askCounter);
		if(discardPile.GetComponent<CardArea>().cards.Count > 0){
			discardCard();
			updateHand(_turnId);
		}
		if(Hand.GetComponent<CardArea>().cards.Count >= 13 ){
			statusPrompt("Too Many Cards, Please Discard or use");
		}

		else if (_canEnd) {
			if (_questInPlay) {					//Quest Currently in plays
				if (_sponsorId >= 0) {  		//There is a sponsor
					
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
							
							for(int i = 0 ; i < numStages ;i++){
								TurnOffDraggable(Stages[i]);
							}
							
							int count = getStagedCards().Count + 1;		//Get the ammount of cards used in this stage and refund to sponsor
							for(int x = 0 ; x < count ; x++){
								Hand.GetComponent<CardArea>().addCard(_adventureDeck.Draw());
								//_players[_turnId].addCard((_adventureDeck.Draw()));
							}
							updateHand(_turnId); 		 //Update Sponsor Hand based off of the UI
							nextTurn(true,false);
							prompt(_turnId,"playQuest"); //Start Asking if other players want to play
							statusPrompt("");
						}
						else{
							statusPrompt("Quest is Not Valid");
						}
					}
					else if(_questReady == true){						//Quest is Ready
						if(_playersIn.Count > 0){						//If people are participating
							_askCounter++;								
							if(_testStagePlay){							//TEST IS IN PLAY
								if(_testTime == false){					//Bidding 
									if(_askCounter > _playersIn.Count){
										_testWinners = calculateTestWinners();
										_askCounter = 0;
										_testTime = true;
									}
									else{
										updateHand(_turnId);
										nextTurnQuest();
									}
								}
								if(_testTime == true){
									if(_askCounter > _playersIn.Count - 1){
										//kill players that didnt make it
										for(int i = 0 ; i < _deadPlayers.Count; i++){				//Removing Dead players
											_playersIn.Remove(_deadPlayers[i]);
										} 
										statusPrompt("Setup Your Weapons");
										_rumble = false;
										_askCounter = 1;
										_currQuestStage++;
										_testTime = false;
										_testStagePlay = false;
										nextTurnQuest();
									}
									else{
										bool win = false;
										for(int i = 0 ; i < _testWinners.Count ; i++){
											if(_testWinners[i] == _turnId){
												win = true;
												break;
											}
										}
										if(win == true){
											statusPrompt("You Won the Bid");
											nextTurnQuest();
										}
										else{
											statusPrompt("You Lost bid");
											for(int i = 0 ; i < _playersIn.Count; i++){
											if(_playersIn[i] == _turnId){
													_deadPlayers.Add(_playersIn[i]);
												}
											}
											nextTurnQuest();
										}

									}
								}

							}							
							else if(playAreaValid(playArea.GetComponent<CardArea>().cards)){	//PlayZone is valid
								updateHand(_turnId);
								statusPrompt("");
								_askCounter++;	
								if(_askCounter > _playersIn.Count){
									if(_rumble == true){	//Done Checking all Questees go back to setup
									 
										for(int i = 0 ; i < _deadPlayers.Count; i++){				//Removing Dead players
											_playersIn.Remove(_deadPlayers[i]);
										}
										if(_playersIn.Count <= 0){ 									//Everyone is dead So reset
											clearQuestCards();
											updateHand(_turnId);
											reset();
											_turnId = nextTurnID;
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
											if(_currQuestStage == testStage){
												_testStagePlay = true;
											}
											else if(_currQuestStage >= numStages){	//Quest is Over
												currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "";
												statusPrompt("Quest Was Done successfully");
												for(int i = 0 ; i < _playersIn.Count ; i++){
													if(bonusQuestPoints == true){
														_players[_playersIn[i]].AddShields(numStages+2);			
													}
													else{
													_players[_playersIn[i]].AddShields(numStages);
													}
												}
												bonusQuestPoints = false;
												clearQuestCards();
												updateHand(_turnId);
												reset();
												_turnId = nextTurnID;
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
										clearQuestCards();
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
				else if(_sponsorId == -1){ 			//Find A Sponsor
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
								_tourneeWinners = calculateTournamentWinner();
								_askCounter = 0;
							}
						}
						if(_rumble == false){
							if(playAreaValid(playArea.GetComponent<CardArea>().cards)){
								statusPrompt("Setup Your Weapons");
								updateHand(_turnId);
								_askCounter++;
								nextTurnTournament();
							}
							else{
								statusPrompt("Play Area is not valid");

							}
						}
						else{										//Fight
							nextTurnTournament();
							bool win = false;
							for(int i = 0 ; i < _tourneeWinners.Count ; i++){
								if(_tourneeWinners[i] == _turnId){
									win = true;
									break;
								}
							}
							//Debug.Log(_turnId);
							if(_askCounter >= _playersIn.Count){
								statusPrompt("");

								reset();
								_askCounter = 0;
								clearQuestCards();
								updateHand(_turnId);
								_turnId = nextTurnID;
								nextTurn(false,false);
							}
							else if(win){
								_askCounter++;
								statusPrompt("Winner");
								Debug.Log(_turnId);
								clearWeapons();
								updateHand(_turnId);
								_players[_turnId].AddShields(_tournamentCard.shields + _playersIn.Count);
							}
							else{
								_askCounter++;
								clearWeapons();
								updateHand(_turnId);
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

	//reclaimPlaced cards
	private void reclaimCards() {
		List<List<Card>> stages = getStages();
		for (int i = 0; i < stages.Count; i++) {
			for (int j = 0; j < stages [i].Count; j++) {
				_players [_turnId].addCard (stages [i] [j]);
			}
		}
		for (int z = 0; z < Stages.Count; z++) {
			Stages[z].GetComponent<CardArea>().cards = new List<Card>();
			// Clears out draw card area.
			foreach (Transform child in Stages[z].transform) {
				GameObject.Destroy(child.gameObject);
			}
		}
	}

	/*
		Tournament
	*/
	public void nextTurnTournament(){
		rankUpPlayers();
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
		rankUpPlayers();
		//Clear Old Hand
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		foreach (Transform child in playArea.transform) {	
			GameObject.Destroy (child.gameObject);
		}


		_turnId++;
		if (_turnId >= _numPlayers) {
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
		
		rankUpPlayers();
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
		for(int i = 0 ; i < numStages ;i++){
			TurnOnDraggable(Stages[i]);
		}
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
		_tourneeWinners = new List<int>();
		_testStagePlay = false;
		_testTime = false;
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
			if(answer == 2) {							//No
				if(_askCounter < _numPlayers){
					reclaimCards ();
					nextTurn(true,false);
				}
				else{									//No Sponsor
					_askCounter = 0;
					reset();
					_turnId++;							//Skip The original draw turn
					if (_turnId >= _numPlayers) {
						_turnId = 0;
					}
					_turnId = nextTurnID;
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
					nextTurn(true,false);					//True Because player shouldn't be able to draw cards 		
				}	
				else{										//Done Asking
					//Change To QuesteeTurns
					_askCounter = 1;
					_currQuestStage = 0;
					if(testStage == _currQuestStage){
						_testStagePlay = true;
						statusPrompt("It's TEST TIME !!!!");
						currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "Current Stage: "+ (_currQuestStage+1).ToString();
					
					}
					else{
						statusPrompt("Setup your Weapons");
						currStageTxt.GetComponent<UnityEngine.UI.Text>().text = "Current Stage: "+ (_currQuestStage+1).ToString();
					}
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
					if (_turnId >= _numPlayers) {
						_turnId = 0;
					}
					if(_playersIn.Count <= 0){				//No on joined so reset
						reset();
						_turnId = nextTurnID;
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
						_turnId = nextTurnID;
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
						_turnId = nextTurnID;
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
		//assume 4 players

		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		//Set up the decks
		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck("Story");
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");

		_playersIn = new List<int> ();
		_deadPlayers = new List<int>();
		_testWinners = new List<int>();

		_turnId = 0;
		_numPlayers = _players.Count;
		_drawn = false;
		_canEnd = false;
		_questInPlay = false;
		_rumble = false;
		_tournamentInPlay = false;
		nextTurnID = -1;

		_sponsorId = -1;
		_questeeTurnId = -1;
		_askCounter = 0;
		_currQuestStage = -1;	
		promptAnswer = -1;

		_tourneeID = -1;
		testStage = -1;
		_testStagePlay = false;
		_testTime = false;

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
		
		foreach (Transform child in rankCardArea.transform) {	
			GameObject.Destroy (child.gameObject);
		}

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


		GameObject cardUI = Instantiate(RankCard);

		int rank = _players[playerId].rank;
		string rankAsset = null;

		if(rank == 0){
			rankAsset = "card_image/rank/rankCard1";
		}
		if(rank == 1){
			rankAsset = "card_image/rank/rankCard2";
		}
		if(rank == 2){
			rankAsset = "card_image/rank/rankCard3";
		}


		Sprite rankCard = Resources.Load<Sprite>(rankAsset); //Card Assets
		cardUI.gameObject.GetComponent<Image>().sprite = rankCard;
		cardUI.transform.SetParent(rankCardArea.transform);
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


			if (currCard.GetType () == typeof(AmourCard)) {

				AmourCard currAmour = (AmourCard)currCard;
				CardUI = Instantiate (AmourCard);
				CardUI.GetComponent<AmourCard>().name = currAmour.name;
				CardUI.GetComponent<AmourCard>().asset = currAmour.asset;
				CardUI.GetComponent<AmourCard>().power = currAmour.power;
				CardUI.GetComponent<AmourCard>().bid = currAmour.bid;
			}
			if(currCard.GetType () == typeof(TestCard)){
				TestCard currTest = (TestCard)currCard;
				CardUI = Instantiate (TestCard);
				CardUI.GetComponent<TestCard>().name = currTest.name;
				CardUI.GetComponent<TestCard>().asset = currTest.asset;
				CardUI.GetComponent<TestCard>().minBids = currTest.minBids;
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
		
	
	void TurnOffDraggable(GameObject area){
		

		area.GetComponent<CardArea>().acceptObj = false;
		/*
		List<Card> cardList = area.GetComponent<CardArea>().cards;
		for(int i = 0 ; i < cardList.Count ;i++){
			cardList[i].obj.GetComponent<Card>().draggable = false;
		} */

	}

	void TurnOnDraggable(GameObject area){
		area.GetComponent<CardArea>().acceptObj = true;
	}

	/*
	public List<GameObject> playerActive;
	public List<GameObject> numCardText;
	public List<GameObject> shieldCounterList;
	public List<GameObject> rankTextList;
	*/
	public void OpenShowPlayer(){
		playerPanel.SetActive(true);
		for(int i = 0 ; i < numCardText.Count ; i++){
			numCardText[i].GetComponent<UnityEngine.UI.Text>().text = "#Card: "+ _players[i].hand.Count.ToString();
		}
		for(int i = 0 ; i < shieldCounterList.Count ; i++){
			shieldCounterList[i].GetComponent<UnityEngine.UI.Text>().text = "#Shield: "+ _players[i].shieldCounter.ToString();
		}
		for(int i = 0 ; i < rankTextList.Count ; i++){
			int currRank = _players[i].rank;
			if(currRank == 0 ){
				rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Squire";
			}
			else if(currRank == 1 ){
				rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Knight";
			}
			else if(currRank == 2 ){
				rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Champion Knight";
			}
		}
			 
		for(int i = 0 ; i < playerActive.Count ; i++){
			loadCards(_players[i].inPlay,playerActive[i]);
		}
	}

	public void CloseShowPlayer(){
		playerPanel.SetActive(false);

		for(int i = 0 ; i < playerActive.Count ; i++){
			foreach (Transform child in playerActive[i].transform) {	
				GameObject.Destroy (child.gameObject);
			}
		}
	}

	public void rankUpPlayers(){
		for(int i = 0 ; i <_players.Count ; i++){
			_players[i].Rankup();
		}
	}

	public void NormalMode(){
		Menu.SetActive(false);
	}

	public void QuestOnlyMode(){
		Menu.SetActive(false);
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		_players = new List<Player>();
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));
		//Set up the decks
		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck("QuestOnly");
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");

		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
	}

	public void TournamentOnlyMode(){
		Menu.SetActive(false);
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		//Set up the decks
		_players = new List<Player>();
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck("TournamentOnly");
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");


		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
	}

	public void EventModeOnly(){
		Menu.SetActive(false);
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy (child.gameObject);
		}
		//Set up the decks
		_players = new List<Player>();
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck("EventOnly");
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");


		//Populates Player Hands
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}
		loadHand(_turnId);
	}

}
