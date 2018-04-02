﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBehaviour : GameBehaviour {

	// The current quest card.
	QuestCard _questCard;

	// The id of the player who sponsored the quest.
	int _sponsorId = -1;

	// The player whose current turn it is.
	int _turnId = 0;

	// The number of players we have asked to join the quest.
	int _asked = 0;

	// The current state the game is on.
	int _currStage = 0;

	// Indicates if the quest is ready (setup).
	bool _questReady = false;

	// Indicates if we are in the setup weapons phase.
	bool _setupWeapons = false;

	// Indicates that a bid has been won, move to next stage/end	
	bool _bidWin = false;

	// Indicates if we are showing the results.
	bool _showResults = false;

	// List of stage powers.
	List<int> _stagePower;

	// List of participating players.
	List<int> _playersIn = new List<int>();

	// List of players who have dies during the stage.
	List<int> _deadPlayers = new List<int>();

	// Index for traversing _playersIn.
	int participatingPlayerIndex = 0;

	// Number of participating players at the start of a cycle.
	int participatingPlayers = 0;

	//The stage that test appears on
	int testStage = -1;

	int numberOfTestStage = 0;

	//highest bid
	int highestBid;
	int highestBidder;

	List<Card> stagedCards;



	// Moves to the next player.
	public void nextPlayer(){
		_turnId++;

		if (_turnId >= Game.GameManager.getNumberOfPlayers()) {
			_turnId = 0;
		}
	}

	// Set current turn.
	public void setCurrentTurn(int n){
		_turnId = n;
	}

	// Get current turn.
	public int getCurrentTurn(){
		return _turnId;
	}

	// End turn method for when a Quest card is in play.
	public void endTurn(){

		/*
			if(Game.GameManager.Stages[_currStage++].GetType() == typeof(TestCard))
			{
				Debug.Log("Test Mode");
				Debug.Log("flip card");
			}
		
		*/
		// Check if the results of the quest are in.
		//Debug.Log("Num in hand2  " + Game.GameManager.getNumInPlay(_turnId));

		if (_showResults) {
			// Move to the next player.
			participatingPlayerIndex++;

			// If we have finished handling all participating players.
			if (participatingPlayerIndex > (participatingPlayers - 1)){

				//Remove AmourCard
				for(int i = 0 ;i <_deadPlayers.Count;i++){
					Game.GameManager.clearInPlayEnd(_deadPlayers[i]);
				}

				// Update the _playersIn list.
				for(int i = 0; i < _deadPlayers.Count; i++){
					_playersIn.Remove(_deadPlayers [i]);
				}
				_deadPlayers = new List<int>();

				// Update participating players count.
				participatingPlayers = _playersIn.Count;

				// Check if there are still players.
				if (participatingPlayers == 0) {
					Game.GameManager.logger.info("Quest is over and everyone died!\t");

					// End the quest.
					endQuest();
					return;

				} else {
					// Move to the next stage (players have been eliminated if they died).
					_currStage++;

					
					
					
					// If there are no more stages, we have winners.
					if (_currStage >= _stagePower.Count){
						Game.GameManager.logger.info("Quest is over and we have winner(s)!");

						// Payout winners.
						for(int i = 0 ; i < _playersIn.Count ; i++){
							Game.GameManager.logger.info("Player " + (_playersIn[i] + 1) + " won the quest!");

							if(Game.GameManager.bonusQuestPoints){
								Game.GameManager.payShield(_playersIn[i], _questCard.stages + 2);
								Game.GameManager.logger.info("Player " + (_playersIn[i] + 1) + " recieved 2 extra shields because of King's Recognition.");
							}
							else{
								Game.GameManager.payShield(_playersIn[i], _questCard.stages);
							}
						}

						// Remove AmourCard.
						for(int i = 0 ;i <_playersIn.Count;i++){
							Game.GameManager.clearInPlayEnd(_playersIn[i]);
						}

						// End the quest.
						Game.GameManager.bonusQuestPoints = false;
						endQuest();
						return;
					}
		

					// Return to setup weapons mode.
					_setupWeapons = true;
					_showResults = false;

					Game.GameManager.logger.info("Swithing back to setup weapons state.");

					// Go back to the first player who is still alive.
					participatingPlayerIndex = 0;
					_turnId = _playersIn[0];

					// Pay everyone 1 adventure card.
					for(int i = 0; i < _playersIn.Count; i++){
						Game.GameManager.logger.info("Giving Player " + (_playersIn[i] + 1) + " an adventure card for passing the stage.");
						Game.GameManager.giveCard(_playersIn[i]);
					}

					// Return to setup weapons prompt.
					Prompt.PromptManager.statusPrompt("Setup your weapons!");
					if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
						Debug.Log("AI Setup Weapon");
						List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
						if(aiPlayCard != null && aiPlayCard.Count > 0){
							Game.GameManager.setInPlayAI(_turnId,aiPlayCard);
						}
						endTurn();
					}

				}
			} else {
				// Update _turnId.

				_turnId = _playersIn[participatingPlayerIndex];

				didYouSurvivePrompt();
				// Clear the players inPlay list.
				Game.GameManager.clearInPlay(_turnId);
			}

			// Load the new player.
			Game.GameManager.loadPlayer(_turnId);
		} 
		else {
			//if we are on a test stage 
			
			

			if(testStage == _currStage){

				if(Game.GameManager.getPlayer(_turnId).GetType() != typeof(AIPlayer)){
					Game.GameManager.setInPlay(_turnId);
				}
				
				//check if newbid is higher than highestBid, lower removed from quest, save new highestBid and highest Bidder if higher
				//save bid cards for all players who are bidding 
				//


		
				Debug.Log("turn ID: " + _turnId);
				Debug.Log("participatingPlayers: " + participatingPlayers);
			
				


				if(Game.GameManager.getInPlay(_turnId).Count > highestBid){ 
					highestBid = Game.GameManager.getInPlay(_turnId).Count;
					highestBidder = _turnId;
					Debug.Log("highest Bid: " + highestBid); 
					Debug.Log("highest Bidder: " + highestBidder); 
				}
				else{
					Debug.Log("Removed player: " + _turnId);
					//return card to their hand 
					Game.GameManager.getInPlay(_turnId);
					_playersIn.Remove(_turnId);
					participatingPlayers--;
				}
				Prompt.PromptManager.statusPrompt ("Bid by adding cards in the play area, current highest bid: " + highestBid); 
			
				
				Debug.Log("participatingPlayers: " + participatingPlayers);
				

				if (participatingPlayers == 1) {	
					Debug.Log("participatingPlayerIndex: " + participatingPlayerIndex);	
					_turnId = _playersIn[participatingPlayerIndex];
					participatingPlayerIndex = 0;
					Game.GameManager.clearInPlay(_turnId);
					_showResults = true;
					_bidWin = true;
					Game.GameManager.logger.info("All playing players have bid for stage, highest bid was player: " + highestBidder);

				}
				else{
					participatingPlayerIndex++;

					if(participatingPlayerIndex == participatingPlayers){
						participatingPlayerIndex = 0;
						_turnId = _playersIn[participatingPlayerIndex];

					}


					_turnId = _playersIn[participatingPlayerIndex];

				}

				//nextPlayer();
				// Load the new player.
				Game.GameManager.loadPlayer(_turnId);
			}
			// If we are on the setup weapons stage.
			else if (_setupWeapons) {
				Debug.Log("Inside _setupWeapons");
				// Weapon setup is valid.
				if (Game.GameManager.playAreaValid ()) {

					// Set the players cards that they have in play.
					if(Game.GameManager.getPlayer(_turnId).GetType() != typeof(AIPlayer)){
						Game.GameManager.setInPlay(_turnId);
					}

					// Fix prompt message (if they submited an invalid input).
					Prompt.PromptManager.statusPrompt ("Setup your weapons!");

					// Move to next player in _playersIn.
					participatingPlayerIndex++;

					// If we have finished checking all the participating players.
					if (participatingPlayerIndex > (participatingPlayers - 1)) {

						_setupWeapons = false;
						_showResults = true;

						Game.GameManager.logger.info("All playing players have setup weapons for stage " + (_currStage + 1));

						participatingPlayerIndex = 0;
						_turnId = _playersIn[0];

						// Unflip the stage cards.
						Game.GameManager.logger.info("Flipping cards in stage " + (_currStage + 1));
						if(_currStage < _questCard.stages){
							List<Card> cardsToReveal = Game.GameManager.Stages[_currStage].GetComponent<CardArea>().cards;
							for(int i = 0; i < cardsToReveal.Count; i++){
								cardsToReveal[i].flipCard(false);
							}
						}
						didYouSurvivePrompt();

						// Clear the players inPlay list.
						Game.GameManager.clearInPlay(_turnId);
					} 

					else {

						// Update _turnId.
						_turnId = _playersIn[participatingPlayerIndex];
						if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
							Debug.Log("AI Setup Weapon");
							List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
							if(aiPlayCard != null){
								Game.GameManager.setInPlayAI(_turnId,aiPlayCard);
							}
							endTurn();
						}
					}

					// Load the new player.
					Game.GameManager.loadPlayer(_turnId);

				// Weapon setup is not valid.
				} else {
					Game.GameManager.logger.info("Play area is invalid!");
					//Prompt.PromptManager.statusPrompt("You can't submit foe/too many amours to the play area!");
				}

			// Otherwise, quest needs to be setup.
			} else {

				// If the quest is unsponsored.
				if (_sponsorId == -1) {
					Prompt.PromptManager.promptMessage ("sponsor");

					// The quest is sponsored.
				} else {

					if(Game.GameManager.getPlayer(_turnId).GetType() != typeof(AIPlayer)){
						_questReady = checkQuest ();
					}

					// Quest is setup correctly.
					if (_questReady) {

						// Flip the staged cards.
						stagedCards = Game.GameManager.getStagedCards(_questCard.stages);
						/*
						for(int x = 0 ; x < 4 ; x++){
							for (int i = 0; i < stagedCards.Count; i++) {
								//if(stagedCards[i].GetType() != typeof(TestCard)){
									stagedCards[i].flipCard (true);
								//}
							}
						}
						*/
						// 2nd time to ensure flipage
						for(int j=0; j < 5; j++){
							for(int x = 0 ; x < 4 ; x++){
								for (int i = 0; i < stagedCards.Count; i++) {
									if(stagedCards[i].GetType() != typeof(TestCard)){
										stagedCards[i].flipCard (true);
									}
								}
							}
						}
						

						//Remove Cards Played in stage
						Game.GameManager.removeCards(_turnId,stagedCards);


						// Get cards to add back to player.
						List<Card> cardsToAdd = new List<Card> ();
						for (int x = 0; x < (stagedCards.Count + 1); x++) {
							cardsToAdd.Add (Game.GameManager.drawAdventureCard ());
						}

						// Add the cards to the players hand.
						Game.GameManager.addCardsToPlayerHand (_turnId, cardsToAdd);

						// Add everyone but the sponsor to playersIn before asking.
						for (int i = 0; i < Game.GameManager.getNumberOfPlayers (); i++) {
							if (i != _turnId) {
								_playersIn.Add (i);
							}
						}

						// Move to the next player.
						nextPlayer();

						// Load new player.
						Game.GameManager.loadPlayer(_turnId);

						// Ask if the next player wants to play.
						Prompt.PromptManager.promptMessage ("quest");

						//AI join quest
						if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
							Game.GameManager.AILogicResponse(_turnId,"quest");
							Debug.Log("Herelol");
						}

						// Clear the status prompt.
						Prompt.PromptManager.statusPrompt ("");
					}
				}
			}
		}
	}

	// Ends the currect quest.
	public void endQuest(){
		Game.GameManager.logger.info("Quest has been fully reset and ended.");

		// Reset the quest behaviour.
		_sponsorId = -1;
		_turnId = 0;
		_asked = 0;
		_currStage = 0;
		_questReady = false;
	  	_setupWeapons = false;
		_showResults = false;
		testStage = -1;
		numberOfTestStage = 0;

		participatingPlayerIndex = 0;
		participatingPlayers = 0;

		// Proceed to the next player and story card in the game.
		Game.GameManager.nextCardAndPlayer();
	}

	// Set the current story card.
	public void setCard(Card c){
		_questCard = (QuestCard)c;
	}

	// When the user responds to the sponsor prompt.
	public void sponsor(bool answer){
		// The user wants to sponsor the quest.
		_asked++;
		if (answer) {
			// Prompt the user to setup the quest.
			Prompt.PromptManager.statusPrompt("Please set up the Quest.");

			Game.GameManager.logger.info("Player " + (_turnId + 1) + " decided to sponsor the quest.");

			// Set the sponsor and setup the stages.
			createQuest(_turnId);

			//Rest asked
			_asked = 0;

			//AI sponsor
			if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				_questReady = true;
				List<List<Card>> AIcards = Game.GameManager.AISponsorCards(_turnId);
				AIStageSetup(AIcards);
				List<GameObject> stages = Game.GameManager.Stages;

				//Display Cards
				for(int i = 0 ; i < _questCard.stages ; i++){
					Game.GameManager.loadCards(AIcards[i],stages[i]);
				}
				//Remove Cards from AI HAND
				for(int i = 0 ; i < AIcards.Count ;i++){
					for(int j = 0 ; j < AIcards[i].Count;j++){
						Game.GameManager.removeCardByName(_turnId,AIcards[i][j].name);
					}
				}
				endTurn();
			}


		// The user doesn't want to sponsor the quest.
		} else {
			Game.GameManager.logger.info("Player " + (_turnId + 1) + " decided NOT to sponsor the quest.");

			if(_asked >= Game.GameManager.getNumberOfPlayers() ){
				Game.GameManager.logger.info("Nobody wanted to sponsor the quest!");
				endQuest();
				return;
			}

			// Move to the next player.
			nextPlayer();

			// They answered no, so their turn is over.
			endTurn();

			// Load the next player.
			Game.GameManager.loadPlayer(_turnId);

			if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				Game.GameManager.AILogicResponse(_turnId,"sponsor");
			}
		}
	}


	// When the user responds to the join quest prompt.
	public void joinQuest(bool answer){
		_asked++;

		// The user doesn't want to join the quest.
		if (!answer) {
			// Remove the players from the players in the quest.
			_playersIn.Remove (_turnId);
			Game.GameManager.logger.info ("Player " + (_turnId + 1) + " decided to NOT join the quest.");
		} else {
			Game.GameManager.logger.info("Player " + (_turnId + 1) + " decided to join the quest.");
		}

		// If we have asked all the players.
		if (_asked >= (Game.GameManager.getNumberOfPlayers() - 1)) {

			if(_playersIn.Count == 0 ){
				Game.GameManager.logger.info("Ending the quest because no players joined.");
				endQuest();
				return;
			}
			else{
				_turnId = _playersIn[0];

				// Pay everyone that join 1 adventure Card.
				for(int i = 0 ; i<_playersIn.Count ; i++){
					Game.GameManager.logger.info("Giving Player " + (_playersIn[i] + 1) + " one adventure card for joining the quest.");
					Game.GameManager.giveCard(_playersIn[i]);
				}

				if(testStage==_currStage){
					Debug.Log("Why am I here????");
					Debug.Log("currently on test stage");
					Prompt.PromptManager.statusPrompt ("Bid by adding cards in the play area");
				
					//Debug.Log("Num in hand +3 " + Game.GameManager.getInPlay(_turnId+3).Count);

					highestBid = Game.GameManager.getNumInPlay(_turnId);
					highestBidder = _turnId;
					Debug.Log("highest bid: " + highestBid);
					participatingPlayers = _playersIn.Count;

					
				}
				else{
				// Move to setup weapon phase.
				_setupWeapons = true;
				Game.GameManager.logger.info("Moving to setup weapons state.");

				participatingPlayers = _playersIn.Count;

				Prompt.PromptManager.statusPrompt("Setup your weapons!");
				}

				if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
					Debug.Log("AI Setup Weapon");
					List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
					if(aiPlayCard != null){
						Game.GameManager.setInPlayAI(_turnId,aiPlayCard);
					}
					endTurn();
				}
			}
		}

		// Not eveyone has been asked yet.
		else {
			Prompt.PromptManager.promptMessage("quest");
			nextPlayer();
			//AI join quest
			if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				Game.GameManager.AILogicResponse(_turnId,"quest");
			}
		}

		// Load the right player.
		Game.GameManager.loadPlayer(_turnId);
	}

	// Create a quest and setup the stages.
	public void createQuest(int sponsor){
		_sponsorId = sponsor;

		// Setup the stages for the user to setup the quest.
		Game.GameManager.setupStages();
	}

	// Check if a player survived the stage.
	public bool didYouSurvive(List<Card> cards){
		int power = 0;
		for(int i = 0 ; i < cards.Count ; i++){
			power += Game.GameManager.getPowerFromCard(cards[i]);
		}

		// Add the rank bonus.
		power += Game.GameManager.getRankPower(Game.GameManager.getPlayer(_turnId).rank);
		Game.GameManager.logger.info("Stage " + (_currStage + 1) + " has a power level of " + _stagePower[_currStage] + ".");
		Game.GameManager.logger.info("Player " + (_turnId + 1) + " has a power level of " + power + ".");
		return power > _stagePower[_currStage];
	}

	public void didYouSurvivePrompt(){
		if (didYouSurvive(Game.GameManager.getInPlay(_turnId))){
			Prompt.PromptManager.statusPrompt ("You passed stage " + (_currStage + 1) + "!");
			Game.GameManager.logger.info("Player " + (_turnId + 1) + " passed stage " + (_currStage + 1) + ".");
		} else {
			// Player died, remove them.
			_deadPlayers.Add(_turnId);
			Game.GameManager.logger.info("Player " + (_turnId + 1) + " died on stage " + (_currStage + 1) + ".");
			Prompt.PromptManager.statusPrompt ("You died on stage " + (_currStage + 1) + "!");
		}
	}

	// Check to make sure a stage is valid.
	public int stageValid(List<Card> currStage,int stageNum, bool doPrompt = true){
		Card currCard = null;
		int power = 0;
		int foeCount = 0;

		List<WeaponCard> weapons = new List<WeaponCard>();
		for(int i = 0; i < currStage.Count; i++) {
			currCard = currStage[i];

			// If the card is a weapon card.
			if(currCard.GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)currCard;

				for(int x = 0 ; x < weapons.Count; x++){
					if(currWeapon.name == weapons[x].name){
						if (doPrompt) {
							Game.GameManager.logger.warn("Quest setup is invalid: duplicate weapons.");
							Prompt.PromptManager.statusPrompt ("Quest Invalid: Duplicate weapons.");
						}
						return -1;
					}
				}

				weapons.Add(currWeapon);
				power += currWeapon.power;

			// Otherwise, it's a foe card.
			}else if(currCard.GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)currCard;

				// Handle feature foe.
				if(_questCard.featuredFoe == currFoe.type){
					power += currFoe.hiPower;
					if (doPrompt) {
						Game.GameManager.logger.info ("Using high power for " + currFoe.name + " because it is featured.");
					}
				} else if(_questCard.featuredFoe == "*") {
					power += currFoe.hiPower;
				} else {
					power += currFoe.loPower;
					if (doPrompt) {
						Game.GameManager.logger.info ("Using low power for " + currFoe.name + " because it is NOT featured.");
					}
				}
				foeCount++;


			} else if(currCard.GetType() == typeof(TestCard)){


				if(currStage.Count > 1){	//If there is more than one card on this stage
					Game.GameManager.logger.info ("To many cards on this test stage");
					return -1;
				}

				testStage = stageNum;		//THE Test stage;
				Debug.Log("testStage "+ testStage);
				numberOfTestStage++;		//Increase global test stage object if there is more than one then break
				return 1;

				// Invalid card.
			}else {
				return -1;
			}
		}

		if(foeCount > 1 || foeCount <= 0){
			if (doPrompt) {
				Game.GameManager.logger.warn("Quest setup is invalid: each stage must have exactly 1 foe.");
				Prompt.PromptManager.statusPrompt ("Quest Invalid: Each stage must exactly have 1 foe.");
			}
			return -1;
		}

		return power;
	}

	// Check if a quest is valid.
	public bool checkQuest() {
		List<int> powerLevels = new List<int>();
		List<List<Card>> stages = Game.GameManager.getStages(_questCard.stages);
		int currPower = 0;;

		if(numberOfTestStage > 1){	//More than one test stage is found in this quest
			Game.GameManager.logger.warn("Quest setup is invalid: Too many test cards.");
			Prompt.PromptManager.statusPrompt("Quest Invalid: Too many test cards.");
			numberOfTestStage = 0;
			testStage = -1;
			return false;
		}

		// Grab the power levels from all the cards within the stages.
		
		for (int i = 0; i < stages.Count; i++) {
			currPower = stageValid(stages[i],i);
			if(currPower == -1){
				return false;
			}else if(currPower == 1){
				powerLevels.Add(10000); //Show that it is a test stage
			}else {
				powerLevels.Add(currPower);
			}
		}

		// Check ascending power level
		for(int i = 0; i < powerLevels.Count - 1; i++){
			

			if(powerLevels[i] >= powerLevels[i + 1] && powerLevels[i] != 10000){
				Game.GameManager.logger.warn("Quest setup is invalid: not ascending power level.");
				Prompt.PromptManager.statusPrompt("Quest Invalid: Not ascending power.");
				return false;
			}
		}

		// Get calculated stage powers if valid.
		_stagePower = powerLevels;


		return true;
	}

/* AI */
	public void AIStageSetup(List<List<Card>> stages){
		List<int> powerLevels = new List<int>();
		int currPower = 0;


		// Grab the power levels from all the cards within the stages.
		for (int i = 0; i < stages.Count; i++) {
			currPower = AIStagePower(stages[i]);
			powerLevels.Add(currPower);
		}

		// Get calculated stage powers if valid.
		_stagePower = powerLevels;
	}

	public int AIStagePower(List<Card> currStage){
		Card currCard = null;
		int power = 0;

		for(int i = 0; i < currStage.Count; i++) {
			currCard = currStage[i];
			if(currCard.GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)currCard;
				power += currWeapon.power;
			}
			else if(currCard.GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)currCard;
				if(_questCard.featuredFoe == currFoe.type){
					power += currFoe.hiPower;
				} else if(_questCard.featuredFoe == "*") {
					power += currFoe.hiPower;
				} else {
					power += currFoe.loPower;
				}
			}
		}
		return power;

	}

}
