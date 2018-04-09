using System.Collections.Generic;
public class AIPlayer : Player {

	SponsorQuest sponsorQuestBehaviour;
	ParticipateInQuest participateQuestBehaviour;
	ParticipateInTournament participateTournamentBehaviour;
	TestBid testBidBehaviour;
	int strategyNumber;


	public AIPlayer (int playerId, int strategy) : base(playerId) {
		sponsorQuestBehaviour = new SponsorQuest ();
		participateQuestBehaviour = new ParticipateInQuest ();
		participateTournamentBehaviour = new ParticipateInTournament ();
		testBidBehaviour = new TestBid ();
		strategyNumber = strategy;
	}

	public List<List<Card>> sponsorQuest(QuestCard questCard, List<Player> players) {
		if (sponsorQuestBehaviour.sponsor(_playerId,someoneCouldWin(questCard.stages, players), players, questCard)) {
			return sponsorQuestBehaviour.setup1 (questCard, this);
		}
		return null;
	}

	public List<Card> playTournament(TournamentCard storyCard, List<Player> players) {
		if (strategyNumber == 1) {
			return participateTournamentBehaviour.play1 (_playerId, storyCard, players);
		} else {
			return participateTournamentBehaviour.play2 (_playerId, storyCard, players);
		}
	}

	public bool joinTournament(TournamentCard storyCard, List<Player> players) {
		int value = storyCard.shields;
		bool couldWin = someoneCouldWin (value, players);
		if (strategyNumber == 1) {
			return participateTournamentBehaviour.join1 (couldWin);
		} else {
			return participateTournamentBehaviour.join2 (couldWin);
		}
	}


	public bool joinQuest(QuestCard storyCard, List<Player> players) {
		if (strategyNumber == 1) {
			return participateQuestBehaviour.join1 (_playerId, storyCard.stages, players);
		} else {
			return participateQuestBehaviour.join2 (_playerId, storyCard.stages, players);
		}
	}

	public List<Card> playQuest(List<Player> players, int testBid, bool isLastStage, bool secondRound) {
		if (testBid == -1) {
			if (strategyNumber == 1) {
				return participateQuestBehaviour.play1 (_playerId, testBid, isLastStage, players);
			} else {
				return participateQuestBehaviour.play2 (_playerId, testBid, isLastStage, players);
			}
		} else {
			if (strategyNumber == 1) {
				return testBidBehaviour.discardAfterWinningTest1 (_playerId, players, testBid, secondRound);
			} else {
				return testBidBehaviour.discardAfterWinningTest2 (_playerId, players, testBid, secondRound);
			}
		}
	}

	public int requestBid(List<Player> players, int currBid, bool multipleBids) {
		if (strategyNumber == 1) {
			return testBidBehaviour.nextBid1 (_playerId, players, currBid, multipleBids);
		} else {
			return testBidBehaviour.nextBid2 (_playerId, players, currBid, multipleBids);
		}
	}

	bool someoneCouldWin(int value, List<Player> players) {
		int[] requirements = new int[]{ 5, 7, 10, 0};
		for (int playerId = 0; playerId < players.Count; playerId++) {
			Player currPlayer = players [playerId];
			if (currPlayer.shieldCounter + value >= requirements [currPlayer.rank]) {
				return true;
			}
		}
		return false;
	}

}
