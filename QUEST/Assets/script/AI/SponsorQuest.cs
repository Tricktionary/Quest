using System.Collections.Generic;
public class SponsorQuest: AIBehaviour {

	//Strategy 1
	public bool sponsor(int aiId, bool couldwin, List<Player> players, int stages){
		if (couldwin) {
			return false;
		} else if (canSponsor(stages, players[aiId])){
			return true;
		} else {
			return false;
		}
	}

	private bool canSponsor(int stages, Player ai) {
		List<WeaponCard> weaponCards = new List<WeaponCard>();
		List<FoeCard> foeCards = new List<FoeCard>();
		List<Card> hand = ai.hand;

		for(i = 0 ; i < hand.Count ; i++){
			if(hand[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)hand[i];
				weaponCards.Add(currWeapon);
			}
			if(hand[i].GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)hand[i];
				foeCards.Add(currFoe);
			}
		}

		if(FoeCard.Count > stages ){
			true;
		}
		return false;
	}

	//TODO: when test is implemented put second last as test
	//SETUP 1
	public List<List<Card>> setup1(QuestCard questCard, Player ai) {
		//TODO: Set up the quest
		//An algorithm to satisfy the constraints and rules of SETUP 1
		//create a minimum quest setup, smallest number of cards (foes + weapons)
		//note: weapons should only be used to separate same power in this case
		//Stack up last monster to be highest possible
		//going backwards to the first stage, equip each foe with the highest duplicate weapon if possible
		//quest setup is finished, return List<List<Card>>

		List<WeaponCard> weaponCards = new List<WeaponCard>();
		List<FoeCard> foeCards = new List<FoeCard>();
		List<Card> hand = ai.hand;

		for(i = 0 ; i < hand.Count ; i++){
			if(hand[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)hand[i];
				weaponCards.Add(currWeapon);
			}
			if(hand[i].GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)hand[i];
				foeCards.Add(currFoe);
			}
		}


		//Sort and just POP the last one into each stage  Foe




		
		return null;
	}

	//TODO: when test is implemented put second last as test
	//SETUP 2
	public List<List<Card>> setup2(QuestCard questCard, Player ai) {
		//TODO: Set up the quest
		//An algorithm to satisfy the constraints and rules of SETUP 2
		//create a minimum quest setup, smallest and weakest number of cards (foes + weapons)
		//note: weapons should only be used to separate same power in this case
		//Stack up last monster to be highest possible and replace last foe with strongest one if applicable
		//since you already selected the weakest monsters with minimal weapons, this is already implemented
		//quest setup is finished, return List<List<Card>>

		List<WeaponCard> weaponCards = new List<WeaponCard>();
		List<FoeCard> foeCards = new List<FoeCard>();
		List<Card> hand = ai.hand;

		for(i = 0 ; i < hand.Count ; i++){
			if(hand[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)hand[i];
				weaponCards.Add(currWeapon);
			}
			if(hand[i].GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)hand[i];
				foeCards.Add(currFoe);
			}
		}


		//Sort



		return null;
	}
}