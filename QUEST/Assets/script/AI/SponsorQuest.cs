using System.Collections.Generic;
using UnityEngine;
public class SponsorQuest: AIBehaviour {

	//Strategy 1
	public bool sponsor(int aiId, bool couldwin, List<Player> players, QuestCard questCard){
		if (couldwin) {
			return false;
		} else if (canSponsor(players[aiId], questCard)){
			return true;
		} else {
			return false;
		}
	}

	private bool canSponsor(Player ai, QuestCard questCard) {
		List<WeaponCard> weaponCards = new List<WeaponCard>();
		List<FoeCard> foeCards = new List<FoeCard>();
		List<Card> hand = ai.hand;

		for(int i = 0 ; i < hand.Count ; i++){
			if(hand[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)hand[i];
				weaponCards.Add(currWeapon);
			}
			if(hand[i].GetType() == typeof(FoeCard)){
				FoeCard currFoe = (FoeCard)hand[i];
				foeCards.Add(currFoe);
			}
		}
		foeCards.Sort ((x, y) => x.power (questCard).CompareTo (y.power (questCard)));
		foeCards.Reverse ();
		int differentPower = 1;
		for (int y = foeCards.Count -1; y >= 0; y--) {
			if (y > 0) {
				if (foeCards [y].power (questCard) != foeCards [y - 1].power (questCard)) {
					differentPower += 1;
					if (differentPower == questCard.stages) {
						return true;
					}
				}
			}
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

		List<List<Card>> returnedStages = new List<List<Card>> ();
		List<WeaponCard> weaponCards = new List<WeaponCard>();
		List<FoeCard> foeCards = new List<FoeCard>();
		List<Card> hand = ai.hand;

		for(int i = 0 ; i < hand.Count ; i++){
			if(hand[i].GetType() == typeof(WeaponCard)){
				WeaponCard currWeapon = (WeaponCard)hand[i];
				weaponCards.Add(currWeapon);
			}
			if(hand[i].GetType() == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)hand[i];
				foeCards.Add(currFoe);
			}
		}


		//Sort and just POP the last one into each stage  Foe
		weaponCards.Sort((x, y) => x.bp.CompareTo(y.bp));
		weaponCards.Reverse ();
		foeCards.Sort ((x, y) => x.power (questCard).CompareTo (y.power (questCard)));
		foeCards.Reverse ();
		//Add the stages
		for (int i = questCard.stages - 1; i >= 0; i--) {
			returnedStages.Add (new List<Card> ());
			FoeCard currFoe = foeCards[0];
			int index = 0;
			if (returnedStages.Count > 1) {
				//Find previous stage power
				int stagePower = ((FoeCard)returnedStages[returnedStages.Count-2][0]).power(questCard);
				//Does not beat previous stage
				if (currFoe.power(questCard) >= stagePower) {
					bool stageSet = false;
					while (!stageSet) {
						while(true) {
							if (foeCards [0].power (questCard) < stagePower) {
								currFoe = foeCards [0];
								index = 0;
								stageSet = true;
								break;
							} else {
								foeCards.RemoveAt (0);
							}
						}
					}
				}
			}
			returnedStages [returnedStages.Count-1].Add (currFoe);
			foeCards.RemoveAt(index);
		}
		int bossPower = ((FoeCard)returnedStages [0] [0]).power (questCard);
		for (int i = 0; i < weaponCards.Count; i++) {
			if (!returnedStages [0].Contains (weaponCards [i])) {
				returnedStages [0].Add (weaponCards [i]);
				bossPower += weaponCards [i].bp;
				weaponCards.RemoveAt (i);
				i--;
			}
			if (bossPower >= 50) {
				break;
			}
		}
		int previousFoePower = bossPower;
		int currFoePower = 0;
		for (int i = 1; i < returnedStages.Count; i++) {
			currFoePower = ((FoeCard)returnedStages [i] [0]).power (questCard);
			for (int j = 0; j < weaponCards.Count; j++) {
				if (weaponCards [j].bp + currFoePower < previousFoePower) {
					currFoePower += weaponCards [j].bp;
					returnedStages [i].Add (weaponCards [j]);
					weaponCards.RemoveAt (j);
					break;
				}
			}
			previousFoePower = currFoePower;
		}
		returnedStages.Reverse ();
		return returnedStages;
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

		for(int i = 0 ; i < hand.Count ; i++){
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
		weaponCards.Sort((x, y) => x.bp.CompareTo(y.bp));
		weaponCards.Reverse ();
		foeCards.Sort ((x, y) => x.power (questCard).CompareTo (y.power (questCard)));
		foeCards.Reverse ();



		return null;
	}
}