using System.Collections.Generic;
public class AIPlayer : Player {

	SponsorQuest sponsorQuestBehaviour;
	ParticipateInQuest participateQuestBehaviour;
	ParticipateInTournament participateTournamentBehaviour;

	public AIPlayer (int playerId) : base(playerId) {
		sponsorQuestBehaviour = new SponsorQuest ();
		participateQuestBehaviour = new ParticipateInQuest ();
		participateTournamentBehaviour = new ParticipateInTournament ();
	}

	List<List<Card>> sponsorQuest(QuestCard questCard, List<Player> players) {
		if (sponsorQuestBehaviour.sponsor(_playerId,someoneCouldWin(questCard.stages, players), players, questCard.stages)) {
			return sponsorQuestBehaviour.setup1 (questCard, this);
		}
		return null;
	}

	List<Card> playTournament(TournamentCard storyCard, List<Player> players) {
		return participateTournamentBehaviour.play1 (_playerId, storyCard, players);

	}

	bool joinTournament(TournamentCard storyCard, List<Player> players) {
		int value = storyCard.shields;
		bool couldWin = someoneCouldWin (value, players);
		return participateTournamentBehaviour.join1 (couldWin);
	}
		

	bool joinQuest(QuestCard storyCard, List<Player> players) {
		return participateQuestBehaviour.join1 (_playerId, storyCard.stages, players);
	}

		List<Card> playQuest(List<Player> players, int testBid, bool isLastStage) {
		return participateQuestBehaviour.play1 (_playerId, testBid, isLastStage, players);
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

