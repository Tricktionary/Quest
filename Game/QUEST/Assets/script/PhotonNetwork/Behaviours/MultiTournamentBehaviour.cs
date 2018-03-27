using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTournamentBehaviour : GameBehaviour {

	// The current tournament card in play.
	TournamentCard _tournamentCard;

	// The current turn id.
	int _turnId;

	// Players participating in the tournament.
	List<int> _playersIn = new List<int>();

	// Players who won the tournament.
	List<int> _winners = new List<int>();

	// Number of players asked to join the tournament.
	int _asked = 0;

	// If a tournament has been initiated or not.
	bool _tournamentInProgress = false;

	// Everyone has been asked to join.
	bool _joinedUp = false;

	// Indicate the tournament has ended.
	bool _tournamentConcluded = false;

	// Index for traversing participating players.
	int participatingPlayerIndex = 0;

	// Amount of players at the start of the cycle.
	int participatingPlayers;

	// Set the current story card.
	public void setCard(Card c){
		_tournamentCard = (TournamentCard)c;
	}

	public void setCurrentTurn(int n){
		_turnId = n;
	}

	// Get current turn.
	public int getCurrentTurn(){
		return _turnId;
	}


	// End turn method for when a Tournament card is in play.
	public void endTurn(){

		if (_tournamentConcluded){
			endTournament();
			return;
		}

		if (_joinedUp){

			// Check if weapon setup is valid.
			if (MultiplayerGame.GameManager.playAreaValid ()) {

				// Set the players cards that they have in play.
				if(MultiplayerGame.GameManager.getPlayer(_playersIn[participatingPlayerIndex]).GetType() != typeof(AIPlayer)){
					Debug.Log ("Current Turn: "+ _playersIn[participatingPlayerIndex]);
					MultiplayerGame.GameManager.setInPlay(_playersIn[participatingPlayerIndex]);
				}


				// Fix prompt message (if they submited an invalid input).
				MultiplayerGame.GameManager.getPromptManager().statusPrompt ("Setup your weapons!");

				// Move to next player in _playersIn.
				participatingPlayerIndex++;

				// If we have finished checking all the participating players.
				if (participatingPlayerIndex > (participatingPlayers - 1)) {

					_winners = calculateTournamentWinner();
					string winners_string = "";

					int shieldPrize = _tournamentCard.shields + _playersIn.Count;

					// Build winners string and reward winners.
					for (int i = 0; i < _winners.Count; i++) {
						MultiplayerGame.GameManager.getPlayer(_winners[i]).AddShields(shieldPrize);
						winners_string += ((_winners[i] + 1) + ", ");
					}

					// Clear all inplay cards for players.
					for (int i = 0; i < _playersIn.Count; i++){
						MultiplayerGame.GameManager.clearInPlay (_playersIn[i]);
					}

					MultiplayerGame.GameManager.logger.info("The following player(s) have won " + shieldPrize + " shields: " + winners_string.Substring(0, winners_string.Length - 2));
						MultiplayerGame.GameManager.getPromptManager().statusPrompt("The following player(s) have won " + shieldPrize + " shields: " + winners_string.Substring(0, winners_string.Length - 2));

					_tournamentConcluded = true;
				} else {
					// Update _turnId.
					_turnId = _playersIn[participatingPlayerIndex];
					//AI Logic
					if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
						Debug.Log("AI Setup Weapon");

						List<Card> aiPlayCard = MultiplayerGame.GameManager.AILogicPlayCards(_turnId);
						MultiplayerGame.GameManager.setInPlayAI(_turnId,aiPlayCard);
						endTurn();
					}
				}

				// Load the new player.
				MultiplayerGame.GameManager.loadPlayer(_turnId);
				MultiplayerGame.GameManager.block(_turnId);

			} else {
					MultiplayerGame.GameManager.getPromptManager().statusPrompt("You can't submit foes to the play area!");
			}

		} else {

			if (!_tournamentInProgress) {
				_tournamentInProgress = true;

				// Add everyone to playersIn.
				for (int i = 0; i < MultiplayerGame.GameManager.getNumberOfPlayers (); i++) {
					_playersIn.Add (i);
				}

				// Prompt to join the tournament.
					MultiplayerGame.GameManager.getPromptManager().promptMessage ("tournament");
			}
		}
	}

	// Moves to the next player.
	public void nextPlayer(){
		_turnId++;
		if (_turnId >= MultiplayerGame.GameManager.getNumberOfPlayers()) {
			_turnId = 0;
		}
		MultiplayerGame.GameManager.block(_turnId);
	}

	// Find the tournament winner.
	public List<int> calculateTournamentWinner(){
		List<Card> cardsInPlay = new List<Card>();
		List<int> powerLevels = new List<int>();
		List<int> winners = new List<int>();
		int power = 0;
		int maxPower = 0;

		// Loop through all participating players.
		for(int i = 0; i < _playersIn.Count; i++){

			// Get the cards in play.
			cardsInPlay = MultiplayerGame.GameManager.getInPlay(_playersIn[i]);

			// Apply player battle points.
			power += MultiplayerGame.GameManager.getPlayer(_playersIn[i]).bp;

			// Loop through cards in play.
			for(int j = 0; j < cardsInPlay.Count; j++){
				power += MultiplayerGame.GameManager.getPowerFromCard(cardsInPlay[j]);
			}

			MultiplayerGame.GameManager.logger.info("Player " + (_playersIn[i] + 1) + " entered a power level of " + power + ".");

			// Add to power levels and reset.
			powerLevels.Add(power);
			power = 0;
		}

		// Find the max power level.
		for(int i = 0; i < powerLevels.Count; i++){
			if(powerLevels[i] > maxPower){
				maxPower = powerLevels[i];
			}
		}

		// Find the player is corresponds too.
		for(int i = 0; i < powerLevels.Count; i++){
			if(powerLevels[i] == maxPower){
				winners.Add(_playersIn[i]);
			}
		}

		return winners;
	}

	// Ends the current tournament.
	public void endTournament(){
		MultiplayerGame.GameManager.logger.info("Ending the tournament!");

		// Reset tournament varaibles.
		_turnId = 0;
		MultiplayerGame.GameManager.block(_turnId);
		_playersIn = new List<int>();
		_winners = new List<int>();
		_asked = 0;
		_tournamentInProgress = false;
		_joinedUp = false;
		_tournamentConcluded = false;
		participatingPlayerIndex = 0;
		participatingPlayers = 0;

		// Proceed to the next player and story card in the game.
		MultiplayerGame.GameManager.nextCardAndPlayer();
	}

	// Fires when someone answers the tournament prompt.
	public void joinTournament(bool answer){
		_asked++;

		// The user wants to join the tournament.
		if (!answer) {
			// Remove the players from the players in the quest.
			_playersIn.Remove (_turnId);
			MultiplayerGame.GameManager.logger.info ("Player " + (_turnId + 1) + " decided to NOT join the tournament.");
		} else {
			MultiplayerGame.GameManager.logger.info ("Player " + (_turnId + 1) + " decided to join the tournament.");
		}

		// If we have asked all the players.
		if (_asked >= (MultiplayerGame.GameManager.getNumberOfPlayers())) {
			if(_playersIn.Count < 2){
				MultiplayerGame.GameManager.logger.info("Not enough players joined the tournament!");

				//Pay the one player that joined the tournament
				for(int i = 0 ; i < _playersIn.Count;i++){
					MultiplayerGame.GameManager.getPlayer(_playersIn[i]).AddShields(1);
				}

				// End the tournament.
				endTournament();
				return;
			} else {
				_turnId = _playersIn[0];
				//Pay everyone that join 1 adventure Card
				for(int i = 0 ; i<_playersIn.Count ; i++){
					MultiplayerGame.GameManager.logger.info("Giving Player " + (_playersIn[i] + 1) + " one card for joining the tournament.");
					MultiplayerGame.GameManager.giveCard(_playersIn[i]);
				}

				// Everyone has had the option to join.
				_joinedUp = true;

				MultiplayerGame.GameManager.logger.info("Starting tournament!");

				// Update the participating players.
				participatingPlayers = _playersIn.Count;

					MultiplayerGame.GameManager.getPromptManager().statusPrompt ("Setup your weapons!");
				if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
					Debug.Log("AI Setup Weapon");

					List<Card> aiPlayCard = MultiplayerGame.GameManager.AILogicPlayCards(_turnId);
					MultiplayerGame.GameManager.setInPlayAI(_turnId,aiPlayCard);
					endTurn();

				}

			}
		}

		else {
				MultiplayerGame.GameManager.getPromptManager().promptMessage("tournament");
			nextPlayer();
			//AI Join?
			if(MultiplayerGame.GameManager.getPlayer(_turnId).GetType() == typeof(AIPlayer)){
					MultiplayerGame.GameManager.getPromptManager().promptYes();
				MultiplayerGame.GameManager.AILogicResponse(_turnId,"");
			}
		}

		// Load the right player.
		MultiplayerGame.GameManager.loadPlayer(_turnId);
		MultiplayerGame.GameManager.block(_turnId);
	}
}
