using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBehaviour : GameBehaviour {

	EventCard _eventCard;

	// Player to do event for.
	int _turnId;

	// Event handled.
	bool _eventHandled = false;

	// End turn method for when a Event card is in play.
	public void endTurn(){
		Game.GameManager.logger.info ("Handling event card: " + _eventCard.name);
		handleEvent(_eventCard.name);
	}

	public int getCurrentTurn(){
		return _turnId;
	}

	public void handleEvent(string name){
		if (!_eventHandled) {
			_eventHandled = true;

			if (name == "Chivalrous Deed") {

				int numPlayers = Game.GameManager.getNumberOfPlayers ();
				int lowest = 1000;
				List<int> winners = new List<int> ();
				Player p;

				// Find the lowest.
				for (int i = 0; i < numPlayers; i++) {
					p = Game.GameManager.getPlayer (i);
					if (p.calcRankShields () < lowest) {
						lowest = p.calcRankShields ();
					}
				}

				Debug.Log ("Lowest is: " + lowest);
		
				// Find the players with the lowest.
				for (int i = 0; i < numPlayers; i++) {
					p = Game.GameManager.getPlayer (i);
					if (p.calcRankShields () == lowest) {
						winners.Add (i);
					}
				}

				string winners_string = "";

				// Award players.
				for (int i = 0; i < winners.Count; i++) {
					winners_string += ((winners[i] + 1) + ", ");
					Game.GameManager.getPlayer(winners[i]).shieldCounter += 3;
				}

				Prompt.PromptManager.statusPrompt("The following players won 3 shields: " + winners_string.Substring(0, winners_string.Length - 2));

			} else if (name == "Pox") {

				int numPlayers = Game.GameManager.getNumberOfPlayers();
				Player p;

				for (int i = 0; i < numPlayers; i++) {
					if (i != _turnId) {
						p = Game.GameManager.getPlayer(i);
						if (p.shieldCounter != 0) {
							p.shieldCounter--;
						}
					}
				}

				Prompt.PromptManager.statusPrompt("Everyone except for player " + (_turnId + 1) + " lost 1 shield.");

			} else if (name == "Plague") {

				Player p;
				p = Game.GameManager.getPlayer(_turnId);
				if(p.shieldCounter >= 2){
					p.shieldCounter = p.shieldCounter-2;
					Prompt.PromptManager.statusPrompt("Player " + (_turnId + 1) + " lost 2 shield.");
				}
				else{
					Prompt.PromptManager.statusPrompt("Player " + (_turnId + 1) + " did not have enough shields.");
				}

				

			} else if (name == "King's Recognition") {
				Game.GameManager.bonusQuestPoints = true;
				Prompt.PromptManager.statusPrompt("Next players to finish a quest will gain two extra shields");

			} else if (name == "Queen's Favor") {
				Player p;
				int lowestVal = 30;
				int lowerCount = 0;
				int currPlayer = _turnId;

				List<int> lowestPlayers = new List<int>();

				for(int i = 0; i < Game.GameManager.getNumberOfPlayers(); i++){
					p = Game.GameManager.getPlayer(i);
					if(p.shieldCounter <= lowestVal){
						lowestVal = p.shieldCounter;
						//Debug.Log("lowest being added: " + lowestVal + "to player");
						lowestPlayers.Add(i);
					}

				}

				for(int i = 0; i < lowestPlayers.Count; i++){
					//Debug.Log("lowestPlayers: " + lowestPlayers[i]);
					p = Game.GameManager.getPlayer(lowestPlayers[i]);
					for(int j = 0; j < 2; j++){
						p.addCard(Game.GameManager.drawAdventureCard());
					}

				}	

				Prompt.PromptManager.statusPrompt("Players with lowest rank got 2 extra Adventure Cards: ");

			} else if (name == "Court Called to Camelot") {
				//Game.GameManager.removeAllAllies();


			} else if (name == "King's Call to Arms") {

			} else if (name == "Prosperity Throughout the Realm") {
				int numPlayers = Game.GameManager.getNumberOfPlayers ();
				Player p;

				for(int i = 0; i < numPlayers; i++){
					p = Game.GameManager.getPlayer(i);
					for(int j = 0; j < 2; j++){
						p.addCard(Game.GameManager.drawAdventureCard());
					}
				}
				Prompt.PromptManager.statusPrompt("All players immediately drew 2 extra Adventure Cards: ");

			} else {
				Debug.Log ("Unknown event card!");
			}
		} else {
			// Reset event handled.
			_eventHandled = false;

			// Proceed to next card and player.
			Game.GameManager.nextCardAndPlayer();
		}
	}

	public void setCurrentTurn(int n){
		_turnId = n;
	}

	public void setCard(Card c){
		_eventCard = (EventCard)c;
	}

}