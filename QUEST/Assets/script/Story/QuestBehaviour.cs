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

	// End turn method for when a Quest card is in play.
	public void endTurn(){

		// Check if the results of the quest are in.
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
					Prompt.PromptManager.statusPrompt("Oh no! Everyone died!");
					// End the quest.
					endQuest();
					return;
				}
				else {
					// Move to the next stage (players have been eliminated if they died).
					_currStage++;

					// If there are no more stages, we have winners.
					if (_currStage >= _stagePower.Count){
						Debug.Log ("We have winner(s), ending quest!");
						// Payout winners.
						// TODO: do something for the winners?
						for(int i = 0 ; i < _playersIn.Count ; i++){
							Game.GameManager.payShield(_playersIn[i],_questCard.stages);
						}
						// End the quest.
						//Remove AmourCard
						for(int i = 0 ;i <_playersIn.Count;i++){
							Game.GameManager.clearInPlayEnd(_playersIn[i]);
						}
						endQuest();
					}

					// Return to setup weapons mode.
					_setupWeapons = true;
					_showResults = false;

					// Go back to the first player who is still alive.
					participatingPlayerIndex = 0;
					_turnId = _playersIn[0];

					// Return to setup weapons prompt.
					Prompt.PromptManager.statusPrompt("Setup your weapons!");
					if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
						Debug.Log("AI Setup Weapon");
						List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
						if(aiPlayCard != null || aiPlayCard.Count > 0){
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
		} else {

			// If we are on the setup weapons stage.
			if (_setupWeapons) {

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
						participatingPlayerIndex = 0;
						_turnId = _playersIn[0];

						//Unflip Cards
						//TODO : Unflip Cards

						didYouSurvivePrompt();

						// Clear the players inPlay list.
						Game.GameManager.clearInPlay(_turnId);
					} else {

						// Update _turnId.
						_turnId = _playersIn[participatingPlayerIndex];
						if(Game.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
							Debug.Log("AI Setup Weapon");
							List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
							if(aiPlayCard != null){
								Game.GameManager.setInPlayAI(_turnId,aiPlayCard);
							}
							//Debug.Log(aiPlayCard.Count);
							endTurn();
						}
					}

					// Load the new player.
					Game.GameManager.loadPlayer(_turnId);

				// Weapon setup is not valid.
				} else {
					Prompt.PromptManager.statusPrompt("You can't submit foe/too many amours to the play area!");
				}

			// Otherwise, quest needs to be setup.
			} else {

				// If the quest is unsponsored.
				if (_sponsorId == -1) {
					Prompt.PromptManager.promptMessage ("sponsor");

					// The quest is sponsored.
				} else {
					_questReady = checkQuest ();
					// Quest is setup correctly.
					if (_questReady) {

						// Flip the staged cards.
						List<Card> stagedCards = Game.GameManager.getStagedCards (_questCard.stages);
						for(int x = 0 ; x < 2 ; x++){
							for (int i = 0; i < stagedCards.Count; i++) {
								stagedCards[i].flipCard (true);
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
						}

						// Clear the status prompt.
						Prompt.PromptManager.statusPrompt ("");

					// Incorrect setup.
					} else {
						Prompt.PromptManager.statusPrompt ("Quest is invalid, setup correctly.");
					}
				}
			}
		}
	}

	// Ends the currect quest.
	public void endQuest(){
		// Reset the quest behaviour.
		_sponsorId = -1;
		_turnId = 0;
		_asked = 0;
		_currStage = 0;
		_questReady = false;
		 _setupWeapons = false;
		 _showResults = false;
		participatingPlayerIndex = 0;
		participatingPlayers = 0;

		//Clear text
		Prompt.PromptManager.statusPrompt("Please draw a Story Card");

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

			// Set the sponsor and setup the stages.
			createQuest(_turnId);

			//Rest asked
			_asked = 0;

		// The user doesn't want to sponsor the quest.
		} else {
			if(_asked >= Game.GameManager.getNumberOfPlayers() ){
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
			_playersIn.Remove(_turnId);
		}

		// If we have asked all the players.
		if (_asked >= (Game.GameManager.getNumberOfPlayers() - 1)) {

			if(_playersIn.Count == 0 ){
				endQuest();
				return;
			}
			else{
				_turnId = _playersIn[0];

				// Move to setup weapon phase.
				_setupWeapons = true;

				participatingPlayers = _playersIn.Count;

				Prompt.PromptManager.statusPrompt("Setup your weapons!");

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

		return power > _stagePower[_currStage];
	}

	public void didYouSurvivePrompt(){
		if (didYouSurvive(Game.GameManager.getInPlay(_turnId))){
			Prompt.PromptManager.statusPrompt ("You passed stage " + (_currStage + 1) + "!");
		} else {
			// Player died, remove them.
			_deadPlayers.Add(_turnId);

			Prompt.PromptManager.statusPrompt ("You died on stage " + (_currStage + 1) + "!");
		}
	}

	// Check to make sure a stage is valid.
	public int stageValid(List<Card> currStage){
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
						return -1;
					}
				}

				weapons.Add(currWeapon);
				power += currWeapon.power;

			// Otherwise, it's a foe card.
			} else if(currCard.GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)currCard;

				// Handle feature foe.
				if(_questCard.featuredFoe == currFoe.type){
					power += currFoe.hiPower;
				} else if(_questCard.featuredFoe == "*") {
					power += currFoe.hiPower;
				} else {
					power += currFoe.loPower;
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

	// Check if a quest is valid.
	public bool checkQuest() {
		List<int> powerLevels = new List<int>();
		List<List<Card>> stages = Game.GameManager.getStages(_questCard.stages);
		int currPower = 0;;

		// Grab the power levels from all the cards within the stages.
		for (int i = 0; i < stages.Count; i++) {
			currPower = stageValid(stages[i]);
			if(currPower == -1){
				return false;
			} else {
				powerLevels.Add(currPower);
			}
		}

		// Check ascending power level.
		for(int i = 0; i < powerLevels.Count - 1; i++){
			if(powerLevels[i] >= powerLevels[i + 1]){
				return false;
			}
		}

		// Get calculated stage powers if valid.
		_stagePower = powerLevels;

		return true;
	}
}
