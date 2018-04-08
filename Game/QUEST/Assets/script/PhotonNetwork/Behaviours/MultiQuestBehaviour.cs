using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiQuestBehaviour : GameBehaviour {

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

	//The stage that test appears on
	int testStage = -1;

	int numberOfTestStage = 0;

	string blockMessage = "";


	// Moves to the next player.
	public void nextPlayer(){
		_turnId++;
		if (_turnId >= MultiplayerGame.GameManager.getNumberOfPlayers()) {
			_turnId = 0;
		}
		MultiplayerGame.GameManager.block(_turnId,"");
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
		string message = "";
		if(_currStage == testStage){
			Debug.Log("Test Mode");
		}

		// Check if the results of the quest are in.
		else if (_showResults) {
			//Has to be true for showing results
			MultiplayerGame.GameManager.sync = true;
			// Move to the next player.
			participatingPlayerIndex++;

			// If we have finished handling all participating players.
			if (participatingPlayerIndex > (participatingPlayers - 1)){

				//Remove AmourCard
				for(int i = 0 ;i <_deadPlayers.Count;i++){
					MultiplayerGame.GameManager.clearInPlayEnd(_deadPlayers[i]);
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
					MultiplayerGame.GameManager.logger.info("Quest is over and everyone died!");
					MultiplayerGame.GameManager.blockMessage("Quest is over and everyone died!");;
					// End the quest.
					endQuest();
					return;
				} 
				else {
					// Move to the next stage (players have been eliminated if they died).
					_currStage++;


					// If there are no more stages, we have winners.
					if (_currStage >= _stagePower.Count){
						MultiplayerGame.GameManager.logger.info("Quest is over and we have winner(s)!");

						// Payout winners.
						string winnerString = "";
						for(int i = 0 ; i < _playersIn.Count ; i++){
							MultiplayerGame.GameManager.logger.info("Player " + (_playersIn[i] + 1) + " won the quest!");
							winnerString = winnerString + (_playersIn[i] + 1);
							if(MultiplayerGame.GameManager.bonusQuestPoints){
								MultiplayerGame.GameManager.payShield(_playersIn[i], _questCard.stages + 2);
								MultiplayerGame.GameManager.logger.info("Player " + (_playersIn[i] + 1) + " recieved 2 extra shields because of King's Recognition.");
							}
							else{
								MultiplayerGame.GameManager.payShield(_playersIn[i], _questCard.stages);
							}
						}
						message = "Player: "+ winnerString + "are the winner of this Quest";
						MultiplayerGame.GameManager.getPromptManager().statusPrompt(message);

						// Remove AmourCard.
						for(int i = 0 ;i <_playersIn.Count;i++){
							MultiplayerGame.GameManager.clearInPlayEnd(_playersIn[i]);
						}

						// End the quest.
						MultiplayerGame.GameManager.bonusQuestPoints = false;
						endQuest();
						return;
					}

					// Return to setup weapons mode.
					_setupWeapons = true;
					_showResults = false;

					MultiplayerGame.GameManager.logger.info("Swithing back to setup weapons state.");

					// Go back to the first player who is still alive.
					participatingPlayerIndex = 0;
					_turnId = _playersIn[0];
					MultiplayerGame.GameManager.block(_turnId,"");

					// Pay everyone 1 adventure card.
					for(int i = 0; i < _playersIn.Count; i++){
						MultiplayerGame.GameManager.logger.info("Giving Player " + (_playersIn[i] + 1) + " an adventure card for passing the stage.");
						MultiplayerGame.GameManager.giveCard(_playersIn[i]);
					}

					// Return to setup weapons prompt.
					MultiplayerGame.GameManager.getPromptManager().statusPrompt("Setup your weapons!");
					message = "Player: " + (_turnId+1) + " turn to set up weapons";

					if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
						Debug.Log("AI Setup Weapon");
						List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
						if(aiPlayCard != null && aiPlayCard.Count > 0){
							MultiplayerGame.GameManager.setInPlayAI(_turnId,aiPlayCard);
						}
						endTurn();
					}
				}
			} else {
				// Update _turnId.
				_turnId = _playersIn[participatingPlayerIndex];

				message = didYouSurvivePrompt();

				// Clear the players inPlay list.
				MultiplayerGame.GameManager.clearInPlay(_turnId);
				MultiplayerGame.GameManager.loadPlayer(_turnId);
				MultiplayerGame.GameManager.sync = true; 
				MultiplayerGame.GameManager.block(_turnId,"");
				MultiplayerGame.GameManager.blockMessage(message);
				return;
			}

			// Load the new player.
			MultiplayerGame.GameManager.loadPlayer(_turnId);
			MultiplayerGame.GameManager.setSync(_turnId);
			MultiplayerGame.GameManager.block(_turnId,"");
			MultiplayerGame.GameManager.blockMessage(message);

		} else {

			// If we are on the setup weapons stage.
			if (_setupWeapons ) {

				// Weapon setup is valid.
				if (MultiplayerGame.GameManager.playAreaValid ()) {
					
					
					//Only the current client can do this
					if (MultiplayerGame.GameManager.sync == true && MultiplayerGame.GameManager.clientID == _turnId + 1) {
						// Set the players cards that they have in play.
						if (MultiplayerGame.GameManager.getPlayer (_turnId).GetType () != typeof(AIPlayer)) {
							MultiplayerGame.GameManager.setInPlay (_turnId);
							//TODO:Send Photon Call simillar to Tournament Set In Play
							List<Card> currInPlayCards = MultiplayerGame.GameManager.getInPlay (_turnId);
							string[] currInPlayCardStr = new string[currInPlayCards.Count];
							for (int i = 0; i < currInPlayCards.Count; i++) {
								currInPlayCardStr [i] = currInPlayCards [i].name;
							}
							MultiplayerGame.GameManager.photonCall ("PhotonSetInPlay", currInPlayCardStr, _turnId, null, null, null, null, null);
						}
					}


					// Fix prompt message (if they submited an invalid input).
					MultiplayerGame.GameManager.getPromptManager().statusPrompt ("Setup your weapons!");
					message = ("Player: "+(_turnId+1)+" is currently setting up weapon");
					// Move to next player in _playersIn.
					participatingPlayerIndex++;

					// If we have finished checking all the participating players.
					if (participatingPlayerIndex > (participatingPlayers - 1)) {
						_setupWeapons = false;
						_showResults = true;

						MultiplayerGame.GameManager.logger.info("All playing players have setup weapons for stage " + (_currStage + 1));

						participatingPlayerIndex = 0;
						_turnId = _playersIn[0];
						MultiplayerGame.GameManager.block(_turnId,"");

						// Unflip the stage cards.
						MultiplayerGame.GameManager.logger.info("Flipping cards in stage " + (_currStage + 1));

						message = didYouSurvivePrompt();

						// Clear the players inPlay list.
						MultiplayerGame.GameManager.clearInPlay(_turnId);
						MultiplayerGame.GameManager.loadPlayer(_turnId);
						MultiplayerGame.GameManager.sync = true; 
						MultiplayerGame.GameManager.block(_turnId,"");
						MultiplayerGame.GameManager.blockMessage(message);
						return;

					} else {

						// Update _turnId.
						_turnId = _playersIn[participatingPlayerIndex];
						MultiplayerGame.GameManager.block(_turnId,"");
						if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
							Debug.Log("AI Setup Weapon");
							List<Card> aiPlayCard = MultiplayerGame.GameManager.AILogicPlayCards(_turnId);
							if(aiPlayCard != null){
								MultiplayerGame.GameManager.setInPlayAI(_turnId,aiPlayCard);
							}
							endTurn();
						}
					}

					// Load the new player.
					MultiplayerGame.GameManager.loadPlayer(_turnId);
					MultiplayerGame.GameManager.setSync(_turnId);
					MultiplayerGame.GameManager.block(_turnId,message);

				// Weapon setup is not valid.
				} else {
					MultiplayerGame.GameManager.logger.info("Play area is invalid!");
					MultiplayerGame.GameManager.getPromptManager().statusPrompt("You can't submit foe/too many amours to the play area!");
				}

			// Otherwise, quest needs to be setup.
			} else {

				// If the quest is unsponsored.
				if (_sponsorId == -1) {
						MultiplayerGame.GameManager.getPromptManager().promptMessage ("sponsor");

					// The quest is sponsored.
				} else {

					if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() != typeof(AIPlayer)){
						_questReady = checkQuest ();
					}
						
					// Quest is setup correctly.
					if (_questReady) {

						//Only Let this call Once (RECURSIVE METHOD)
						Debug.Log(MultiplayerGame.GameManager.sync);
						Debug.Log(MultiplayerGame.GameManager.clientID);
						Debug.Log(_turnId);
						if (MultiplayerGame.GameManager.sync == true && MultiplayerGame.GameManager.clientID == _turnId+1) {
							//Pull Cards To Push
							Debug.Log("HERE LMAO");
							List<List<Card>> allStageCard = MultiplayerGame.GameManager.getStages(5);
							string[] stage1 = new string[allStageCard[0].Count];
							string[] stage2 = new string[allStageCard[1].Count];
							string[] stage3 = new string[allStageCard[2].Count];
							string[] stage4 = new string[allStageCard[3].Count];
							string[] stage5 = new string[allStageCard[4].Count];

						
							for (int x = 0; x < allStageCard.Count; x++) {
								for (int z = 0; z < allStageCard [x].Count; z++) {
									if (x == 0) {
										//Debug.Log (allStageCard [x] [z].name);
										stage1 [z] = allStageCard [x] [z].name;
									} else if (x == 1) {
										//Debug.Log (allStageCard [x] [z].name);
										stage2 [z] = allStageCard [x] [z].name;
									} else if (x == 2) {
										//Debug.Log (allStageCard [x] [z].name);
										stage3 [z] = allStageCard [x] [z].name;
									} else if (x == 3) {
										//Debug.Log (allStageCard [x] [z].name);
										stage4 [z] = allStageCard [x] [z].name;
									} else if (x == 4) {
										//Debug.Log (allStageCard [x] [z].name);
										stage5 [z] = allStageCard [x] [z].name;
									}
								}
							}

							/* Print Testing */
							for (int i = 0; i < stage1.Length; i++) {
								Debug.Log (stage1 [i]);
							}
							for (int i = 0; i < stage2.Length; i++) {
								Debug.Log (stage2 [i]);
							}
							for (int i = 0; i < stage3.Length; i++) {
								Debug.Log (stage3 [i]);
							}
							for (int i = 0; i < stage4.Length; i++) {
								Debug.Log (stage4 [i]);
							}
							for (int i = 0; i < stage5.Length; i++) {
								Debug.Log (stage5 [i]);
							}
							//Photon Call for sponsoring stage
							MultiplayerGame.GameManager.photonCall ("PhotonQuestStage", null, _turnId, stage1, stage2, stage3, stage4, stage5);
						}


						// Flip the staged cards.
						List<Card> stagedCards = MultiplayerGame.GameManager.getStagedCards(_questCard.stages);
						
						//Remove Cards Played in stage
						MultiplayerGame.GameManager.removeCards(_turnId,stagedCards);

						// Get cards to add back to player.
						List<Card> cardsToAdd = new List<Card> ();
						for (int x = 0; x < (stagedCards.Count + 1); x++) {
							cardsToAdd.Add (MultiplayerGame.GameManager.drawAdventureCard ());
						}

						// Add the cards to the players hand.
						MultiplayerGame.GameManager.addCardsToPlayerHand (_turnId, cardsToAdd);

						// Add everyone but the sponsor to playersIn before asking.
						for (int i = 0; i < MultiplayerGame.GameManager.getNumberOfPlayers (); i++) {
							if (i != _turnId) {
								_playersIn.Add (i);
							}
						}

						// Move to the next player.
						nextPlayer();

						// Load new player.
						MultiplayerGame.GameManager.loadPlayer(_turnId);
						MultiplayerGame.GameManager.setSync(_turnId);
						MultiplayerGame.GameManager.block(_turnId,blockMessage);

						// Ask if the next player wants to play.
							MultiplayerGame.GameManager.getPromptManager().promptMessage ("quest");

						//AI join quest
						if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
							MultiplayerGame.GameManager.AILogicResponse(_turnId,"quest");
							//Debug.Log("Herelol");
						}

						// Clear the status prompt.
							MultiplayerGame.GameManager.getPromptManager().statusPrompt ("");
					}
				}
			}
		}
	}

	// Ends the currect quest.
	public void endQuest(){
		MultiplayerGame.GameManager.logger.info("Quest has been fully reset and ended.");
		for(int i = 0 ; i < MultiplayerGame.GameManager._players.Count; i++){
			MultiplayerGame.GameManager.clearInPlayEnd(i);
		}
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
		_playersIn = new List<int>();
		participatingPlayerIndex = 0;
		participatingPlayers = 0;
		// Proceed to the next player and story card in the game.
		MultiplayerGame.GameManager.nextCardAndPlayer();
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
			MultiplayerGame.GameManager.getPromptManager().statusPrompt("Please set up the Quest.");
			MultiplayerGame.GameManager.blockMessage("Player " + (_turnId + 1) + " decided to sponsor the quest.");
			MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " decided to sponsor the quest.");

			// Set the sponsor and setup the stages.
			createQuest(_turnId);

			//Rest asked
			_asked = 0;
			

			//AI sponsor
			if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				_questReady = true;
				List<List<Card>> AIcards = MultiplayerGame.GameManager.AISponsorCards(_turnId);
				AIStageSetup(AIcards);
				List<GameObject> stages = MultiplayerGame.GameManager.Stages;

				//Display Cards
				for(int i = 0 ; i < _questCard.stages ; i++){
					MultiplayerGame.GameManager.loadCards(AIcards[i],stages[i]);
				}
				//Remove Cards from AI HAND
				for(int i = 0 ; i < AIcards.Count ;i++){
					for(int j = 0 ; j < AIcards[i].Count;j++){
						MultiplayerGame.GameManager.removeCardByName(_turnId,AIcards[i][j].name);
					}
				}
				endTurn();
			}


		// The user doesn't want to sponsor the quest.
		} else {
			MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " decided NOT to sponsor the quest.");
			MultiplayerGame.GameManager.blockMessage("Player " + (_turnId + 1) + " decided NOT to sponsor the quest.");
			if(_asked >= MultiplayerGame.GameManager.getNumberOfPlayers() ){
				MultiplayerGame.GameManager.logger.info("Nobody wanted to sponsor the quest!");
				endQuest();
				return;
			}

			// Move to the next player.
			nextPlayer();

			// They answered no, so their turn is over.
			endTurn();

			// Load the next player.
			MultiplayerGame.GameManager.loadPlayer(_turnId);
			MultiplayerGame.GameManager.setSync(_turnId);
			MultiplayerGame.GameManager.block(_turnId,blockMessage);

			if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				MultiplayerGame.GameManager.AILogicResponse(_turnId,"sponsor");
			}
		}
	}

	// When the user responds to the join quest prompt.
	public void joinQuest(bool answer){
		blockMessage = "";
		_asked++;
		string choice = "";
		// The user doesn't want to join the quest.
		if (!answer) {
			// Remove the players from the players in the quest.
			_playersIn.Remove (_turnId);
			MultiplayerGame.GameManager.logger.info ("Player " + (_turnId + 1) + " decided to NOT join the quest.");
			choice = "Player " + (_turnId + 1) + " decided to NOT join the quest.";
		} else {
			MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " decided to join the quest.");
			choice = "Player " + (_turnId + 1) + " decided to join the quest.";
		}

		// If we have asked all the players.
		if (_asked >= (MultiplayerGame.GameManager.getNumberOfPlayers() - 1)) {

			if(_playersIn.Count == 0 ){
				MultiplayerGame.GameManager.logger.info("Ending the quest because no players joined.");
				endQuest();
				return;
			}
			else{
				_turnId = _playersIn[0];
				MultiplayerGame.GameManager.block(_turnId,"");

				// Pay everyone that join 1 adventure Card.
				for(int i = 0 ; i<_playersIn.Count ; i++){
					MultiplayerGame.GameManager.logger.info("Giving Player " + (_playersIn[i] + 1) + " one adventure card for joining the quest.");
					MultiplayerGame.GameManager.giveCard(_playersIn[i]);
				}

				// Move to setup weapon phase.
				_setupWeapons = true;
				MultiplayerGame.GameManager.logger.info("Moving to setup weapons state.");

				participatingPlayers = _playersIn.Count;

				MultiplayerGame.GameManager.getPromptManager().statusPrompt("Setup your weapons!");
				
				blockMessage = "Player: "+(_turnId+1)+" time to set up weapon.";

				if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
					Debug.Log("AI Setup Weapon");
					List<Card> aiPlayCard = Game.GameManager.AILogicPlayCards(_turnId);
					if(aiPlayCard != null){
						MultiplayerGame.GameManager.setInPlayAI(_turnId,aiPlayCard);
					}
					endTurn();
				}
			}
		}

		// Not eveyone has been asked yet.
		else {
			MultiplayerGame.GameManager.getPromptManager().promptMessage("quest");
			nextPlayer();
			//BLOCK MESSAGE
			//AI join quest
			if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
				MultiplayerGame.GameManager.AILogicResponse(_turnId,"quest");
			}
		}

		// Load the right player.
		MultiplayerGame.GameManager.loadPlayer(_turnId);
		MultiplayerGame.GameManager.setSync(_turnId);
		MultiplayerGame.GameManager.block(_turnId,blockMessage);
		MultiplayerGame.GameManager.blockMessage(choice);
	}

	// Create a quest and setup the stages.
	public void createQuest(int sponsor){
		_sponsorId = sponsor;

		// Setup the stages for the user to setup the quest.
		MultiplayerGame.GameManager.setupStages();
	}

	// Check if a player survived the stage.
	public bool didYouSurvive(List<Card> cards){
		int power = 0;
		for(int i = 0 ; i < cards.Count ; i++){
			power += MultiplayerGame.GameManager.getPowerFromCard(cards[i]);
		}

		// Add the rank bonus.
		power += MultiplayerGame.GameManager.getRankPower(MultiplayerGame.GameManager.getPlayer(_turnId).rank);
		MultiplayerGame.GameManager.logger.info("Stage " + (_currStage + 1) + " has a power level of " + _stagePower[_currStage] + ".");
		MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " has a power level of " + power + ".");
		return power > _stagePower[_currStage];
	}

	public string didYouSurvivePrompt(){
		string message = "";
		if (didYouSurvive(MultiplayerGame.GameManager.getInPlay(_turnId))){
			MultiplayerGame.GameManager.getPromptManager().statusPrompt ("You passed stage " + (_currStage + 1) + "! Stage " + (_currStage + 1) + " has a power level of " + _stagePower[_currStage] + ".");
			MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " passed stage " + (_currStage + 1) + ".");
			message = ("Player " + (_turnId + 1) + " passed stage " + (_currStage + 1) + ".");
		} else {
			// Player died, remove them.
			_deadPlayers.Add(_turnId);
			MultiplayerGame.GameManager.logger.info("Player " + (_turnId + 1) + " died on stage " + (_currStage + 1) + ".");
			MultiplayerGame.GameManager.getPromptManager().statusPrompt ("You died on stage " + (_currStage + 1) + "! Stage " + (_currStage + 1) + " has a power level of " + _stagePower[_currStage] + ".");
			message = ("Player " + (_turnId + 1) + " died on stage " + (_currStage + 1) + ".");
		}
		return message;
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
							MultiplayerGame.GameManager.logger.warn("Quest setup is invalid: duplicate weapons.");
							MultiplayerGame.GameManager.getPromptManager().statusPrompt ("Quest Invalid: Duplicate weapons.");
						}
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
					if (doPrompt) {
						MultiplayerGame.GameManager.logger.info ("Using high power for " + currFoe.name + " because it is featured.");
					}
				} else if(_questCard.featuredFoe == "*") {
					power += currFoe.hiPower;
				} else {
					power += currFoe.loPower;
					if (doPrompt) {
						MultiplayerGame.GameManager.logger.info ("Using low power for " + currFoe.name + " because it is NOT featured.");
					}
				}
				foeCount++;


			} else if(currCard.GetType() == typeof(TestCard)){


				if(currStage.Count > 1){	//If there is more than one card on this stage
					MultiplayerGame.GameManager.logger.info ("To many cards on this test stage");
					return -1;
				}

				testStage = stageNum;		//THE Test stage;
				numberOfTestStage++;		//Increase global test stage object if there is more than one then break
				return 1;

				// Invalid card.
			}else {
				return -1;
			}
		}

		if(foeCount > 1 || foeCount <= 0){
			if (doPrompt) {
				MultiplayerGame.GameManager.logger.warn("Quest setup is invalid: each stage must have exactly 1 foe.");
					MultiplayerGame.GameManager.getPromptManager().statusPrompt ("Quest Invalid: Each stage must exactly have 1 foe.");
			}
			return -1;
		}

		return power;
	}

	// Check if a quest is valid.
	public bool checkQuest() {
		List<int> powerLevels = new List<int>();
		List<List<Card>> stages = MultiplayerGame.GameManager.getStages(_questCard.stages);
		int currPower = 0;;

		if(numberOfTestStage > 1){	//More than one test stage is found in this quest
			MultiplayerGame.GameManager.logger.warn("Quest setup is invalid: Too many test cards.");
				MultiplayerGame.GameManager.getPromptManager().statusPrompt("Quest Invalid: Too many test cards.");
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

		// Check ascending power level.
		for(int i = 0; i < powerLevels.Count - 1; i++){
			if(powerLevels[i] >= powerLevels[i + 1]){
				MultiplayerGame.GameManager.logger.warn("Quest setup is invalid: not ascending power level.");
					MultiplayerGame.GameManager.getPromptManager().statusPrompt("Quest Invalid: Not ascending power.");
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
