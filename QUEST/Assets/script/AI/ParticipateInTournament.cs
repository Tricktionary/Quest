using System.Collections.Generic;
public class ParticipateInTournament: AIBehaviour{

	//Strategy 1
	public bool join1(bool couldWin){
		if (couldWin) {
			return true;
		}
		return false;
	}

	//Strategy 1
	public List<Card> play1(int aiId,TournamentCard storyCard, List<Player> players){
		//Balls to the wall
		List<Card> playCard = players[aiId].hand;
		List<Card> weaponCards = new List<Card>();
		
		for(int i = 0 ; i < playCard.Count ; i++){
			if(playCard[i] is WeaponCard){
				weaponCards.Add(playCard[i]);
			}
		}


		return weaponCards;


		/*
		Player ai = players [aiId];

		int value = storyCard.shields;
		int[] requirements = new int[]{ 5, 7, 10 };
		bool strongest = false;

		for (int playerId = 0; playerId < players.Count; playerId++) {
			Player currPlayer = players [playerId];
			if (currPlayer.shieldCounter + value >= requirements [currPlayer.rank] && playerId != aiId) {
				strongest = true;
				break;
			}
		}

		Dictionary<Card,int> cards = new Dictionary<Card, int>();
		List<Card> keys = new List<Card>();

		//filter weapon cards
		for (int i = 0; i < ai.hand.Count; i++) {
			if (ai.hand [i] is WeaponCard) {
				if (cards.ContainsKey(ai.hand[i])) {
					int amount = cards [ai.hand [i]] + 1;
					cards.Add (ai.hand[i], amount);
				} else {
					cards.Add (ai.hand[i], 1);
					keys.Add (ai.hand [i]);
				}
			} else if (ai.hand [i] is AllyCard) {
				keys.Add(ai.hand[i]);
			}
		}
		if (strongest) {
			//play strongest
			return keys;
		} else {
			//play only duplicates
			List<Card> playCards = new List<Card>();

			for (int i = 0; i < keys.Count; i++) {
				if (cards [keys [i]] > 1) {
					playCards.Add (keys [i]);
				}
			}
			return playCards;
		}*/

	}

	public bool join2(bool couldWin){
		return true;
	}

	//Strategy 2: Just the strongest possible hand
	public List<Card> play2(int aiId,TournamentCard storyCard, List<Player> players){
		Player ai = players [aiId];
		Dictionary<Card,int> cards = new Dictionary<Card, int>();
		List<Card> keys = new List<Card>();

		//filter weapon cards
		for (int i = 0; i < ai.hand.Count; i++) {
			if (ai.hand [i] is WeaponCard) {
				if (cards.ContainsKey (ai.hand [i])) {
					int amount = cards [ai.hand [i]] + 1;
					cards.Add (ai.hand [i], amount);
				} else {
					cards.Add (ai.hand [i], 1);
					keys.Add (ai.hand [i]);
				}
			} else if (ai.hand [i] is AllyCard) {
				keys.Add(ai.hand[i]);
			}
		}
		return keys;
	}
}
