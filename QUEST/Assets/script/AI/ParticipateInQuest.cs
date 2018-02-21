using System.Collections.Generic;
public class ParticipateInQuest: AIBehaviour{

	//Strategy 1
	public bool join1(int aiId,int stages, List<Player> players) {
		Player ai = players [aiId];
		int allies = ai.inPlay.Count;
		int weapons = 0;
		int discardFoes = 0;
		for (int i = 0; i < ai.hand.Count; i++) {
			if (ai.hand [i] is WeaponCard) {
				weapons += 1;
			} else if (ai.hand [i] is AllyCard) {
				allies += 1;	
			} else if(ai.hand [i] is FoeCard) {
				FoeCard foe = ((FoeCard)ai.hand [i]);
				if (foe.loPower < 20) {
					discardFoes += 1;
				}
			}
		}

		if ((weapons / stages) + allies >= 2 && discardFoes >= 2) {
			return true;
		}
		return false;
	}

	public bool play1(int aiId,bool isTest, bool isLastStage, List<Player> players) {

		if (isTest) {
			//TODO:Test
		} else {
			Player ai = players [aiId];
			for (int i = 0; i < ai.hand.Count; i++) {

			}
			if (isLastStage) {
				
			} else {
				
			}
		}
		return false;
	}
}
