using System.Collections.Generic;
public class AIPlayer : Player {

	ParticipateInQuest participateQuestBehaviour;
	ParticipateInTournament participateTournamentBehaviour;

	public AIPlayer (int playerId) : base(playerId) {
		participateQuestBehaviour = new ParticipateInQuest ();
		participateTournamentBehaviour = new ParticipateInTournament ();
	}

	bool playTournament(TournamentCard storyCard, List<Player> players) {
		participateTournamentBehaviour.play1 (_playerId, storyCard, players);
		return false;
	}

	bool joinTournament(TournamentCard storyCard, List<Player> players) {
		int value = storyCard.shields;
		bool couldWin = someoneCouldWin (value, players);
		return participateTournamentBehaviour.join1 (couldWin);
	}


	bool playQuest(TournamentCard storyCard, List<Player> players) {
		//participateQuestBehaviour.play (_playerId, storyCard, players);
		return false;
	}

	bool someoneCouldWin(int value, List<Player> players) {
		int[] requirements = new int[]{ 5, 7, 10 };
		for (int playerId = 0; playerId < players.Count; playerId++) {
			Player currPlayer = players [playerId];
			if (currPlayer.shieldCounter + value >= requirements [currPlayer.rank]) {
				return true;
			}
		}
		return false;
	}

}

