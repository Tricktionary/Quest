﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	// Prefabs.
	public GameObject Card;
	public GameObject WeaponCard;
	public GameObject FoeCard;
	public GameObject AllyCard;
	public GameObject QuestCard;
	public GameObject AmourCard;
	public GameObject TestCard;
	public GameObject EventCard;
	public GameObject RankCard;

	// Panels.
	public GameObject playerPanel;
	public GameObject playArea;
	public GameObject Menu;


	public List<GameObject> Stages;
	public List<GameObject> playerActive;
	public List<GameObject> numCardText;
	public List<GameObject> shieldCounterList;
	public List<GameObject> rankTextList;

	// Misc GameObject's.
	public GameObject Prompt;
	public GameObject promptTxt;
	public GameObject gameStatus;
	public GameObject currStageTxt;
	public GameObject discardPile;
	public GameObject rankCardArea;
	public GameObject drawCardArea;

	// Play area hand reference.
	public GameObject Hand;
 
	// Text fields.
	public GameObject playerIdTxt;
	public GameObject shieldCounterTxt;

	// List of players.
	private List<Player> _players = new List<Player>();

	// TODO: remove this...
	private int _numPlayers;

	// Id of player who is currently playing.
	private int _turnId;

	// Decks.
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private Deck _discardPileAdventure;
	private Deck _discardPileStory;

	// General.
	private bool _drawn;
	private bool _canEnd;
	public int promptAnswer;
	public int _askCounter;  //How many people you asked this prompt to
	private EventCard _eventCard;
	public bool bonusQuestPoints = false; //used for event card "King's Recognition"
	private int nextTurnID;

	// AI.
	private List<AIPlayer> Observers;

	// Quest related.
	private QuestCard _questCard;
	private int _sponsorId;
	private int _questeeTurnId;
	private List<int> _playersIn;
	private List<int> _deadPlayers;
	private string featFoe;
	private int numStages;
	private bool _questInPlay;
	private bool _questReady;
	private int _currQuestStage;
	private List<int> _stagePower;
	private bool _rumble;

	// Tournament related.
	private bool _tournamentInPlay;
	private TournamentCard _tournamentCard;
	private bool _tournamentPrompt;
	private int _tourneeID;
	private List<int> _tourneeWinners;

	// Draw a card.
	public void DrawCard(){
		
		if (!_drawn){

			// Clear out the draw card area.
			foreach (Transform child in drawCardArea.transform) {
				GameObject.Destroy (child.gameObject);
			}

			// Draw a card.
			Card currCard = _storyDeck.Draw();

			// Discard.
			_discardPileStory.Discard(currCard);

			// Load the card sprite.
			string currCardAsset = currCard.asset;
			Sprite card = Resources.Load<Sprite> (currCardAsset);

			GameObject storyCard = null;

			if (currCard.GetType () == typeof(QuestCard)){
				nextTurnID = _turnId;
				storyCard = Instantiate (QuestCard);
				_questInPlay = true;
				_canEnd = true;
				_questCard = (QuestCard)currCard;
				numStages = _questCard.stages;
			}
			else if(currCard.GetType() == typeof(TournamentCard)){
				nextTurnID = _turnId;
				storyCard = Instantiate(QuestCard);
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
					for(int i = 0; i < _numPlayers; i++){
						
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
	

	// Check to make sure a stage is valid.
	public int stageValid(List<Card> currStage){
		bool check = false;
		int foeCount = 0; 
		Card currCard = null;
		int power = 0;

		List<WeaponCard> weapons = new List<WeaponCard>();
		for(int i = 0; i < currStage.Count; i++) {
			currCard = currStage[i];

			// If the card is a weapon card.
			if(currCard.GetType() == typeof(WeaponCard)){
				
				WeaponCard currWeapon = (WeaponCard)currCard;	
				for(int x = 0 ; x < weapons.Count; x++){	//Duplicate Weapons Logic
					if(currWeapon.name == weapons[x].name){
						return -1;
					}
				}
				weapons.Add(currWeapon);
				power = power + currWeapon.power;

			// Otherwise, it's a foe card.
			} else if(currCard.GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)currCard;

				// Handle feature foe.
				if(_questCard.featuredFoe == currFoe.type){
					power = power + currFoe.hiPower;
				} else if(_questCard.featuredFoe == "*") {
					power = power + currFoe.hiPower;
				} else {
					power = power + currFoe.loPower;
				}
				foeCount++;

			// Invalid card.
			} else {
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

	// Determine if the cards in the play area are valid.
	public bool playAreaValid(List<Card> cards){
		for(int i = 0; i < cards.Count; i++){
			// Return false if there are any foe cards.
			if(cards[i].GetType() == typeof(FoeCard)){
				return false;
			} 
		}
		return true;
	}

	// Check if a quest is valid.
	public bool checkQuest() {
		List<int> powerLevels = new List<int>();
		int testCounter = 0;
		int currPower = 0;
		List<List<Card>> stages = getStages();

		// Grab the power levels from all the cards within the stages.
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
			if(powerLevels[i] >= powerLevels[i + 1]){ return false; }	//Can't be equal
		}
		_stagePower = powerLevels;		//Get Calculated stage powers if valid
		
		return true;
	}
		
	// Update The hand of turn ID based off of the user interface.
	public void updateHand(int turnID){
		
		// Update the players Hand.
		List<Card> tempHand = new List<Card>();
		tempHand = Hand.GetComponent<CardArea>().cards;
		_players[_turnId].hand  = new List<Card>();	
		for(int i = 0 ; i < tempHand.Count ;i++){
			_players[_turnId].addCard(tempHand[i]);
		}

		// Update the players play area.
		List<Card> tempPlay = new List<Card>();
		tempPlay = playArea.GetComponent<CardArea>().cards;
		_players[_turnId].inPlay = new List<Card>();
		for(int i = 0 ; i < tempPlay.Count ;i++){
			_players[_turnId].addPlayCard(tempPlay[i]);
		}	
	}

	// Get the rank power for a specific rank.
	public int getRankPower(int rank){
		if(rank == 0){
			return 5;
		} else if(rank == 1) {
			return 10;
		} else if(rank == 2) {
			return 20;
		}
		return 0;
	}

	// Get the rank asset for a specific rank.
	public string getRankAsset(int rank){
		if(rank == 0){
			return "card_image/rank/rankCard1";
		} else if(rank == 1) {
			return "card_image/rank/rankCard2";
		} else if(rank == 2) {
			return "card_image/rank/rankCard3";
		}
		return null;
	}

	// Get power from card.
	private int getPowerFromCard(Card c){
		if(c.GetType() == typeof(WeaponCard)){
			WeaponCard currWeapon = (WeaponCard)c;
			return currWeapon.power;
		}

		if(c.GetType() == typeof(AmourCard)){
			AmourCard currAmour = (AmourCard)c;
			return currAmour.power;
		}

		if(c.GetType() == typeof(AllyCard)){
			AllyCard currAlly = (AllyCard)c;
			return currAlly.power;
		}

		return 0;
	}

	// Check if a player survived the stage.
	public bool didYouSurvive(List<Card> cards){
		int power = 0;
		for(int i = 0 ; i < cards.Count ; i++){
			power += getPowerFromCard(cards[i]);
		}

		// Add the rank bonus.
		power += getRankPower(_players [_turnId].rank);

		return power > _stagePower[_currQuestStage];
	}

	// Clear weapons from the play area.
	private void clearWeapons(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards = new List<Card>();

		// Filter the cards.
		for(int i = 0; i < oldCards.Count; i++){
			if(oldCards[i].GetType() != typeof(WeaponCard)){
				filteredCards.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}

		// Empty the card list.
		playArea.GetComponent<CardArea>().cards = new List<Card>();

		// Repopulate the card list.
		for(int i =0 ; i < filteredCards.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards[i]);
		}
	}

	// Clear quest cards.
	private void clearQuestCards(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards1 = new List<Card>();
		List<Card> filteredCards2 = new List<Card>();

		// Filter amour cards.
		for(int i = 0 ; i < oldCards.Count ; i++){
			if(oldCards[i].GetType() != typeof(AmourCard)){
				filteredCards1.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}

		// Filter weapon cards.
		for(int i = 0 ; i < filteredCards1.Count ; i++){
			if(filteredCards1[i].GetType() != typeof(WeaponCard)){
				filteredCards2.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(filteredCards1[i]);
			}
		}

		// Empty the list.
		playArea.GetComponent<CardArea>().cards = new List<Card>();

		// Repopulate the list.
		for(int i =0 ; i < filteredCards2.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards2[i]);
		}
	}

	// Find the winner of the tournament.
	private List<int> calculateTournamentWinner(){
		List<Card> currHand = new List<Card>();
		
		int currMax = 0;
		int currPower = 0;
		List<int> powerLevels = new List<int>();
		List<int> winnersId = new List<int>();

		// Loop through all participating players.
		for(int i = 0; i < _playersIn.Count; i++ ){

			// Get the players hank.
			currHand = _players[_playersIn[i]].inPlay;

			// Apply player battle points.
			currPower += _players[_playersIn[i]].bp;
			
			// Loop through the players hand.
			for(int x = 0; x < currHand.Count; x++){
				currPower += getPowerFromCard(currHand[x]);
			}

			// Add to power levels and reset.
			powerLevels.Add(currPower);
			currPower = 0;
		}

		// Find the max power level.
		for(int i = 0 ; i < powerLevels.Count ; i++){
			if(powerLevels[i] > currMax){
				currMax = powerLevels[i];
			}
		}

		// Find the player is corresponds too.
		for(int i = 0 ; i < powerLevels.Count ; i++){
			if(powerLevels[i] == currMax){
				winnersId.Add(_playersIn[i]);
			}
		}
			
		return winnersId;
	}

	// Discard a card.
	private void discardCard(){
		// Get the desicarded cards.
		List<Card> cards = discardPile.GetComponent<CardArea>().cards;

		// Discard.
		for(int i = 0; i < cards.Count; i++){
			_discardPileAdventure.Discard(cards[i]);
		}

		// Create a new list.
		discardPile.GetComponent<CardArea>().cards = new List<Card>();

		foreach (Transform child in discardPile.transform) {	
			GameObject.Destroy (child.gameObject);
		}
	}

	public void handleQuestInPlay(){
		// If there is a sponsor.
		if (_sponsorId >= 0) {

			// Quest has been set up, people are now participating.
			if (_questReady) {

				// If people are participating.
				if (_playersIn.Count > 0) {

					// Check if the play area is valid.
					if (playAreaValid (playArea.GetComponent<CardArea> ().cards)) {

						// Update the players hand.
						updateHand (_turnId);

						statusPrompt ("");

						// Increment the number of players asked.
						_askCounter++;	

						// If we have checked all the questees.
						if (_askCounter > _playersIn.Count) {

							// If everyone has setup their weapons.
							if (_rumble) {

								// Remove dead players.
								for (int i = 0; i < _deadPlayers.Count; i++) {
									_playersIn.Remove (_deadPlayers [i]);
								}

								// If everyone is dead, reset.
								if (_playersIn.Count <= 0) {
									clearQuestCards ();
									updateHand (_turnId);
									reset ();
									_turnId = nextTurnID;
									nextTurn (false, false);
									_askCounter = 0;
									currStageTxt.GetComponent<UnityEngine.UI.Text> ().text = "";
									return;
								} else {
									_rumble = false;
									_askCounter = 1;

									// Flip cards in the revealed stage.
									Debug.Log ("Flipping Stage: " + _currQuestStage);
									List<List<Card>> currStages = getStages ();
									for (int i = 0; i < currStages [_currQuestStage].Count; i++) {
										currStages [_currQuestStage] [i].flipCard (false);	
									}

									_currQuestStage++;

									currStageTxt.GetComponent<UnityEngine.UI.Text> ().text = "Current Stage: " + (_currQuestStage + 1).ToString ();
									if (_currQuestStage >= numStages) {	//Quest is Over
										currStageTxt.GetComponent<UnityEngine.UI.Text> ().text = "";
										statusPrompt ("Quest Was Done successfully");
										for (int i = 0; i < _playersIn.Count; i++) {
											if (bonusQuestPoints == true) {
												_players [_playersIn [i]].AddShields (numStages + 2);

											} else {
												_players [_playersIn [i]].AddShields (numStages);
											}
										}
										bonusQuestPoints = false;
										clearQuestCards ();
										updateHand (_turnId);
										reset ();
										_turnId = nextTurnID;
										nextTurn (false, false);
										_askCounter = 0;
										return;
									}
								}
							} else {
								_rumble = true;
								_askCounter = 1;
							}

						}

						if (_rumble == true) {
							nextTurnQuest ();
							if (didYouSurvive (playArea.GetComponent<CardArea> ().cards)) {		//YOU WON THE STAGE
								statusPrompt ("You Lived!");
								clearWeapons ();
								updateHand (_turnId);
							} else {
								statusPrompt ("You Have Perished R.I.P");					//YOU DIED 
								clearQuestCards ();
								updateHand (_turnId);
								for (int i = 0; i < _playersIn.Count; i++) {
									if (_playersIn [i] == _turnId) {
										_deadPlayers.Add (_playersIn [i]);
									}
								}
							}
						} else {										//SETUP
							updateHand (_turnId);
							nextTurnQuest ();
							statusPrompt ("Setup Your Weapons");
						}
					} else {						//Not Valid
						statusPrompt ("Play Zone is Not Valid");
					}
				}

				// Quest still needs to be setup.
			} else {
				_questReady = checkQuest ();

				// Quest has now been initialized.
				if (_questReady) {
					// Flip the staged cards.
					List<Card> stagedCards = getStagedCards ();
					for (int i = 0; i < stagedCards.Count; i++) {
						stagedCards [i].flipCard (true);
					}

					// Turn off draggagle for all the stages.
					for (int i = 0; i < numStages; i++) {
						TurnOffDraggable (Stages [i]);
					}

					// Draw and add an adventure card.
					for (int x = 0; x < (stagedCards.Count + 1); x++) {
						Hand.GetComponent<CardArea> ().addCard (_adventureDeck.Draw ());
					}

					// Update sponsor hand based off of the UI.
					updateHand (_turnId);

					// Call next turn.
					nextTurn (true, false);

					// Start asking if other players want to play.
					prompt (_turnId, "playQuest");
					statusPrompt ("");

					// Quest is not correctly setup.
				} else {
					// Quest is not yet ready.
					statusPrompt ("Quest is not valid!");
				}
			}

		// Find a sponsor.
		} else if(_sponsorId == -1) {
			prompt (_turnId, "sponsor");
		}
	}

	public void handleTournamentInPlay(){
		// If the player has already been prompted to join the tournament.
		if (_tournamentPrompt) {

			// Atleast one player has joined.
			if (_playersIn.Count > 1) {

				// Everyone has setup their weapons.
				if (_askCounter >= _playersIn.Count) {

					if (!_rumble) {  				
						_rumble = true;
						updateHand (_turnId);
						_tourneeWinners = calculateTournamentWinner ();
						_askCounter = 0;
					}
				}

				if (!_rumble) {
					// If the play area is valid.
					if (playAreaValid (playArea.GetComponent<CardArea> ().cards)) {
						statusPrompt ("Setup Your Weapons");
						updateHand (_turnId);
						_askCounter++;
						nextTurnTournament ();
					} else {
						statusPrompt ("Play Area is not valid");
					}
				} else {
					nextTurnTournament();
					bool win = false;
					for (int i = 0; i < _tourneeWinners.Count; i++) {
						if (_tourneeWinners [i] == _turnId) {
							win = true;
							break;
						}
					}

					if (_askCounter >= _playersIn.Count) {
						statusPrompt ("");
						reset ();
						_askCounter = 0;
						clearQuestCards ();
						updateHand (_turnId);
						_turnId = nextTurnID;
						nextTurn (false, false);
					} else if (win) {
						_askCounter++;
						statusPrompt ("Winner");
						Debug.Log (_turnId);
						clearWeapons ();
						updateHand (_turnId);
						_players [_turnId].AddShields (_tournamentCard.shields + _playersIn.Count);
					} else {
						_askCounter++;
						clearWeapons ();
						updateHand (_turnId);
						statusPrompt ("Lmao you lost");
					}
				}
			}
		} else {
			// Prompt the play to join the tournament.
			prompt(_turnId, "playTournament");
		}
	}

	// Runs when the end turn button is clicked.
	public void EndTurn() {
		// If the discard pile has more than 0 cards.
		if(discardPile.GetComponent<CardArea>().cards.Count > 0){
			discardCard();
			updateHand(_turnId);
		}

		// If the hand has too many cards.
		if(Hand.GetComponent<CardArea>().cards.Count >= 13 ){
			statusPrompt("Too Many Cards, Please Discard or use");
		}
			
		// If you can end the turn.
		if (_canEnd) {

			// Quest currently in play.
			if (_questInPlay) {

				// Handle the quest turn.
				handleQuestInPlay();

				// Tournament currently in play.
			} else if (_tournamentInPlay) {

				// Handle the tournament turn.
				handleTournamentInPlay();
			}
		}
	}

	// NOTE: What does this do?
	private void reclaimCards() {
		List<List<Card>> stages = getStages();

		for (int i = 0; i < stages.Count; i++) {
			for (int j = 0; j < stages [i].Count; j++) {
				_players[_turnId].addCard(stages[i][j]);
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
		
	// 
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

	// 
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
		} else {
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
 
	// TODO: Restructure this as a Prompt class.
	public void prompt(int turnId, string messageType){
		if(messageType == "sponsor"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Sponsor This Quest?";
		} else if (messageType == "playQuest"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Join the Quest?";
		} else if (messageType == "playTournament"){
			Prompt.SetActive(true);
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = "Do You Want To Join This Tournament?";
		}
	}

	//User Status Prompt
	public void statusPrompt(string message){
		gameStatus.GetComponent<UnityEngine.UI.Text> ().text = message;
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
					nextTurn(true, false);
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
					} else {
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
		
	// Reset the game.
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

		// Clean the stages.
		for (int i = 0; i < Stages.Count; i++) {
			Stages[i].SetActive(true);
			Stages[i].GetComponent<CardArea>().cards = new List<Card>();

			// Clears out draw card area.
			foreach (Transform child in Stages[i].transform) {
				GameObject.Destroy(child.gameObject);
			}
		}

	}

	// Create a quest and setup the stages.
	public void createQuest(int sponsor){
		_sponsorId = sponsor;
		_questReady = false;	//Quest is not ready yet
		Debug.Log("numStages " + numStages);

		for (int i = 0; i < (5 - numStages); i++) {
			Stages[4-i].SetActive(false);
		}
	}

	// Initialization.
	void Awake() {
		// Create players.
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		// Set up the decks.
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
		nextTurnID = -1;

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
	
	// Load the players hand onto the UI.
	void loadHand(int playerId){
		
		foreach (Transform child in rankCardArea.transform) {	
			GameObject.Destroy(child.gameObject);
		}

		playArea.GetComponent<CardArea>().cards =  new List<Card>();
		Hand.GetComponent<CardArea>().cards =  new List<Card>();

		// Set Player ID text.
		playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID : "+ (playerId+1).ToString();

		// Get current players shield.
		int currPlayerShield = _players[playerId].shieldCounter;
		shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (currPlayerShield).ToString();

		List<Card> currHand = _players[playerId].hand;
		List<Card> currPlay = _players[playerId].inPlay;
		
		loadCards(currHand, Hand);
		loadCards(currPlay, playArea);

		GameObject cardUI = Instantiate(RankCard);

		// Get the rank asset.
		string rankAsset = getRankAsset(_players [playerId].rank);

		// Set the rank asset.
		Sprite rankCard = Resources.Load<Sprite>(rankAsset);
		cardUI.gameObject.GetComponent<Image>().sprite = rankCard;
		cardUI.transform.SetParent(rankCardArea.transform);
	}

	// Load the cards up.
	void loadCards(List<Card> cards, GameObject area){
		Card currCard;

		// Create Card GameObject's.
		for(int i = 0 ; i < cards.Count; i++){
			currCard = cards[i];

			area.GetComponent<CardArea>().addCard(currCard);
			GameObject CardUI = null; 

			// TODO: Clean this up.
			if (currCard.GetType () == typeof(WeaponCard)) {
				//Is this convention ?
				WeaponCard currWeapon = (WeaponCard)currCard;
				CardUI = Instantiate (WeaponCard);
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
			} else if (currCard.GetType () == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)currCard;
				CardUI = Instantiate (FoeCard);
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().type    = currFoe.type;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			} else if (currCard.GetType () == typeof(AllyCard)) {
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
			} else if(currCard.GetType () == typeof(AmourCard)) {
				AmourCard currAmour = (AmourCard)currCard;
				CardUI = Instantiate (AmourCard);
				CardUI.GetComponent<AmourCard>().name = currAmour.name;
				CardUI.GetComponent<AmourCard>().asset = currAmour.asset;
				CardUI.GetComponent<AmourCard>().power = currAmour.power;
				CardUI.GetComponent<AmourCard>().bid = currAmour.bid;
			} else if(currCard.GetType () == typeof(TestCard)){
				TestCard currTest = (TestCard)currCard;
				CardUI = Instantiate (TestCard);
				CardUI.GetComponent<TestCard>().name = currTest.name;
				CardUI.GetComponent<TestCard>().asset = currTest.asset;
				CardUI.GetComponent<TestCard>().minBids = currTest.minBids;
			}
				
			// Load the card sprite.
			Sprite card = Resources.Load<Sprite>(currCard.asset);
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(area.transform);

			// Set the cards obj to it's UI.
			currCard.obj = CardUI;
		}
	}
		
	// Open show player panel.
	public void OpenShowPlayer(){
		playerPanel.SetActive(true);

		// Load the num card text.
		for(int i = 0 ; i < numCardText.Count ; i++){
			numCardText[i].GetComponent<UnityEngine.UI.Text>().text = "#Card: "+ _players[i].hand.Count.ToString();
		}

		// Load the shield counder list.
		for(int i = 0 ; i < shieldCounterList.Count ; i++){
			shieldCounterList[i].GetComponent<UnityEngine.UI.Text>().text = "#Shield: "+ _players[i].shieldCounter.ToString();
		}

		// Load the rank texts.
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
			 
		// Load the cards.
		for(int i = 0 ; i < playerActive.Count ; i++){
			loadCards(_players[i].inPlay,playerActive[i]);
		}
	}

	// Close show player panel.
	public void CloseShowPlayer(){
		playerPanel.SetActive(false);

		for(int i = 0 ; i < playerActive.Count ; i++){
			foreach (Transform child in playerActive[i].transform) {	
				GameObject.Destroy (child.gameObject);
			}
		}
	}

	// Rank up players.
	public void rankUpPlayers(){
		for(int i = 0 ; i <_players.Count ; i++){
			_players[i].Rankup();
		}
	}

	// Setup non-AI modes.
	public void genericModeSetup(){
		// Hide menu.
		Menu.SetActive(false);

		// Clear hand.
		foreach (Transform child in Hand.transform) {	
			GameObject.Destroy(child.gameObject);
		}

		// Setup players.
		_players = new List<Player>();
		_players.Add(new Player(1));
		_players.Add(new Player(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		// Setup decks.
		_adventureDeck = new Deck("Adventure");
		//_storyDeck = new Deck("QuestOnly");
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
		nextTurnID = -1;

		_sponsorId = -1;
		_questeeTurnId = -1;
		_askCounter = 0;
		_currQuestStage = -1;	
		promptAnswer = -1;

		_tourneeID = -1;

		// Populate the players hands.
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}

		// Load the first players hand.
		loadHand(_turnId);
	}

	// Runs if the user selects Play PVP.
	public void NormalMode(){
		Menu.SetActive(false);
	}

	// Runs if the user selects Play AI.
	public void AIMode(int AIs) {
		Menu.SetActive(false);

		// Add human players.
		_players = new List<Player>();
		for (int i = 0; i < 4 - AIs; i++) {
			_players.Add (new Player (i));
		}

		// Add AI players.
		for (int i = 4 - AIs -1; i < AIs; i++) {
			_players.Add (new AIPlayer (i));
		}
	}

	// Runs if the user selects Quest Only Mode.
	public void QuestOnlyMode(){
		genericModeSetup();
		_storyDeck = new Deck("QuestOnly");
	}

	// Runs if the user selects Tournament Only Mode.
	public void TournamentOnlyMode(){
		genericModeSetup();
		_storyDeck = new Deck("TournamentOnly");
	}

	// Runs if the user selects Event Only Mode.
	public void EventModeOnly(){
		genericModeSetup();
		_storyDeck = new Deck("EventOnly");
	}
}
