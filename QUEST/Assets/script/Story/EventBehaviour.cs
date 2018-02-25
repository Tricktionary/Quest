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
		handleEvent(_eventCard.name);
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

			} else if (name == "King's Recognition") {

			} else if (name == "Queen's Favor") {

			} else if (name == "Court Called To Camelot") {

			} else if (name == "King's Call to Arms") {

			} else if (name == "Prosperity Throughout the Realm") {

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