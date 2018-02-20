using System.Collections.Generic;
public class ParticipateInTournament: AIBehaviour{


	//Strategy 1
	bool join(int id,StoryCard storyCard, List<Player> players){
		int value = ((TournamentCard)storyCard).shields;
		int[] requirements = new int[]{ 5, 7, 10 };
		for (int playerId = 0; playerId < players.Count; playerId++) {
			Player currPlayer = players [playerId];
			if (currPlayer.shieldCounter + value >= requirements [currPlayer.rank]) {
				return true;
			}
		}
		return false;
	}

	//Strategy 1
	List<Card> play(int aiId,TournamentCard storyCard, List<Player> players){
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

		//filter weapon cards
		for (int i = 0; i < ai.hand.Count; i++) {
			if (ai.hand [i] is WeaponCard) {
				if (cards.ContainsKey(ai.hand[i])) {
					int amount = cards [ai.hand [i]] + 1;
					cards.Add (ai.hand[i], amount);
				} else {
					cards.Add (ai.hand[i], 1);
				}
			}
		}
		if (strongest) {
			//play strongest
			List<Card> returnPlayCards = new List<Card>();
			if (cards.Keys.Count > 0) {
				returnPlayCards.AddRange (cards.Keys);
			}
			return returnPlayCards;
		} else {
			//play only duplicates
			List<Card> playCards = new List<Card>();

			for (int i = 0; i < cards.Keys.Count; i++) {
				if (cards [cards.Keys [i]] > 1) {
					playCards.Add (cards.Keys [i]);
				}
			}
			return playCards;
		}
	}
}
