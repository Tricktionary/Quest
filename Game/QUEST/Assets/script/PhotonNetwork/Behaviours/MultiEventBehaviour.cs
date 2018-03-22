using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEventBehaviour : GameBehaviour {

	EventCard _eventCard;

	// Player to do event for.
	int _turnId;

	// Event handled.
	bool _eventHandled = false;

	// End turn method for when a Event card is in play.
	public void endTurn(){
		MultiplayerGame.GameManager.logger.info ("Handling event card: " + _eventCard.name);
		handleEvent(_eventCard.name);
	}

	public int getCurrentTurn(){
		return _turnId;
	}

	public void handleEvent(string name){
		if (!_eventHandled) {
			_eventHandled = true;

			if (name == "Chivalrous Deed") {

				int numPlayers = MultiplayerGame.GameManager.getNumberOfPlayers ();
				int lowest = 1000;
				List<int> winners = new List<int> ();
				Player p;

				// Find the lowest.
				for (int i = 0; i < numPlayers; i++) {
					p = MultiplayerGame.GameManager.getPlayer (i);
					if (p.calcRankShields () < lowest) {
						lowest = p.calcRankShields ();
					}
				}

				// Find the players with the lowest.
				for (int i = 0; i < numPlayers; i++) {
					p = MultiplayerGame.GameManager.getPlayer (i);
					if (p.calcRankShields () == lowest) {
						winners.Add (i);
					}
				}

				string winners_string = "";

				// Award players.
				for (int i = 0; i < winners.Count; i++) {
					winners_string += ((winners[i] + 1) + ", ");
					MultiplayerGame.GameManager.getPlayer(winners[i]).shieldCounter += 3;
				}

				MultiplayerPrompt.PromptManager.statusPrompt("The following players won 3 shields: " + winners_string.Substring(0, winners_string.Length - 2));

			} else if (name == "Pox") {

				int numPlayers = Game.GameManager.getNumberOfPlayers();
				Player p;

				for (int i = 0; i < numPlayers; i++) {
					if (i != _turnId) {
						p = MultiplayerGame.GameManager.getPlayer(i);
						if (p.shieldCounter != 0) {
							p.shieldCounter--;
						}
					}
				}

				MultiplayerPrompt.PromptManager.statusPrompt("Everyone except for player " + (_turnId + 1) + " lost 1 shield.");

			} else if (name == "Plague") {

				Player p;
				p = MultiplayerGame.GameManager.getPlayer(_turnId);
				if(p.shieldCounter >= 2){
					p.shieldCounter = p.shieldCounter-2;
					MultiplayerPrompt.PromptManager.statusPrompt("Player " + (_turnId + 1) + " lost 2 shield.");
				}
				else{
					MultiplayerPrompt.PromptManager.statusPrompt("Player " + (_turnId + 1) + " did not have enough shields.");
				}



			} else if (name == "King's Recognition") {
				MultiplayerGame.GameManager.bonusQuestPoints = true;
				MultiplayerPrompt.PromptManager.statusPrompt("Next players to finish a quest will gain two extra shields to lose shields");

			} else if (name == "Queen's Favor") {
				Player p;
				int lowestVal = 30;
				int lowerCount = 0;
				int currPlayer = _turnId;

				List<int> lowestPlayers = new List<int>();

				//find the lowest
				for(int i = 0; i < MultiplayerGame.GameManager.getNumberOfPlayers(); i++){
					p = MultiplayerGame.GameManager.getPlayer(i);
					if(p.shieldCounter <= lowestVal){
						lowestVal = p.shieldCounter;
					}

				}
				//add players with shield value equal to lowest
				for(int i = 0; i < MultiplayerGame.GameManager.getNumberOfPlayers(); i++){
					p = MultiplayerGame.GameManager.getPlayer(i);
					if(p.shieldCounter <= lowestVal){
						lowestPlayers.Add(i);
					}

				}
				//give players two adventure cards
				for(int i = 0; i < lowestPlayers.Count; i++){
					p = MultiplayerGame.GameManager.getPlayer(lowestPlayers[i]);
					for(int j = 0; j < 2; j++){
						p.addCard(MultiplayerGame.GameManager.drawAdventureCard());
					}

				}

				MultiplayerPrompt.PromptManager.statusPrompt("Players with lowest rank got 2 extra Adventure Cards: ");

			} else if (name == "Court Called to Camelot") {
				MultiplayerGame.GameManager.removeAllAllies();


			} else if (name == "King's Call to Arms") {

			} else if (name == "Prosperity Throughout the Realm") {
				int numPlayers = MultiplayerGame.GameManager.getNumberOfPlayers ();
				Player p;

				for(int i = 0; i < numPlayers; i++){
					p = MultiplayerGame.GameManager.getPlayer(i);
					for(int j = 0; j < 2; j++){
						p.addCard(MultiplayerGame.GameManager.drawAdventureCard());
					}
				}
				MultiplayerPrompt.PromptManager.statusPrompt("All players immediately drew 2 extra Adventure Cards: ");

			} else {
				Debug.Log ("Unknown event card!");
			}
		} else {
			// Reset event handled.
			_eventHandled = false;

			// Proceed to next card and player.
			MultiplayerGame.GameManager.nextCardAndPlayer();
		}
	}

	public void setCurrentTurn(int n){
		_turnId = n;
	}

	public void setCard(Card c){
		_eventCard = (EventCard)c;
	}

}
