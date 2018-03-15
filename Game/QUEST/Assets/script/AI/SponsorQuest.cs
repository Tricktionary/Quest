using System.Collections.Generic;
using UnityEngine;
public class SponsorQuest: AIBehaviour {

	//Strategy 1 and Strategy 2 check
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
		int differentPower = 0;
		for (int y = 0; y < foeCards.Count; y++) {
			if (y + 1 < foeCards.Count) {
				if (foeCards [y].power (questCard) != foeCards [y + 1].power (questCard)) {
					differentPower += 1;
				}
			} else {
				differentPower += 1;
			}
			if (differentPower == questCard.stages) {
				return true;
			}
		}
		return false;
	}

	private List<WeaponCard> extractWeapons(List<Card> hand) {
		List<WeaponCard> weaponCards = new List<WeaponCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i].GetType () == typeof(WeaponCard)) {
				WeaponCard currWeapon = (WeaponCard)hand [i];
				weaponCards.Add (currWeapon);
			}
		}
		return weaponCards;
	}
	private List<FoeCard> extractFoes(List<Card> hand) {
		List<FoeCard> foeCards = new List<FoeCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i].GetType () == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)hand [i];
				foeCards.Add (currFoe);
			}
		}
		return foeCards;
	}

	private void findSuitableFoe(ref List<FoeCard> foeCards, QuestCard questCard, int stagePower, bool isOne) {
		bool stageSet = false;
		while (!stageSet) {
			while(true) {
				if (foeCards.Count == 0) {
					Debug.LogError ("Error on sponsorQuest Check");
					return;
				}
				if (isOne && foeCards [0].power (questCard) < stagePower) {
					stageSet = true;
					break;
				} else if (!isOne && foeCards [0].power (questCard) > stagePower) {
					stageSet = true;
					break;
				} else {
					foeCards.RemoveAt (0);
				}
			}
		}
	}

	private int setupBoss(ref List<List<Card>> returnedStages, ref List<WeaponCard> weaponCards, QuestCard questCard, int minPower) {
		int bossPower = ((FoeCard)returnedStages [returnedStages.Count-1] [0]).power (questCard);
		for (int i = 0; i < weaponCards.Count; i++) {
			if (!returnedStages [returnedStages.Count-1].Contains (weaponCards [i])) {
				returnedStages [returnedStages.Count-1].Add (weaponCards [i]);
				bossPower += weaponCards [i].bp;
				weaponCards.RemoveAt (i);
				i--;
			}
			if (bossPower >= minPower) {
				break;
			}
		}
		return bossPower;
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
		List<WeaponCard> weaponCards = extractWeapons (ai.hand);
		List<FoeCard> foeCards = extractFoes (ai.hand);

		//Sort and just POP the last one into each stage  Foe
		weaponCards.Sort((x, y) => x.bp.CompareTo(y.bp));
		weaponCards.Reverse ();
		foeCards.Sort ((x, y) => x.power (questCard).CompareTo (y.power (questCard)));
		foeCards.Reverse ();
		//Add the stages
		for (int i = questCard.stages - 1; i >= 0; i--) {
			returnedStages.Add (new List<Card> ());
			if (returnedStages.Count > 1) {
				//Find previous stage power
				int stagePower = ((FoeCard)returnedStages[returnedStages.Count-2][0]).power(questCard);
				//Does not beat previous stage
				if (foeCards[0].power(questCard) >= stagePower) {
					findSuitableFoe (ref foeCards, questCard, stagePower, true);
				}
			}
			returnedStages [returnedStages.Count-1].Add (foeCards[0]);
			foeCards.RemoveAt(0);
		}

		int bossPower = setupBoss (ref returnedStages, ref weaponCards, questCard, 50);
		/*
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
		*/
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

		List<List<Card>> returnedStages = new List<List<Card>> ();
		List<WeaponCard> weaponCards = extractWeapons (ai.hand);
		List<FoeCard> foeCards = extractFoes (ai.hand);

		//Sort and just POP the last one into each stage  Foe
		weaponCards.Sort((x, y) => x.bp.CompareTo(y.bp));
		foeCards.Sort ((x, y) => x.power (questCard).CompareTo (y.power (questCard)));
		//Add the stages
		for (int i = 0; i < questCard.stages-1; i++) {
			returnedStages.Add (new List<Card> ());
			if (returnedStages.Count > 1) {
				//Find previous stage power
				int stagePower = ((FoeCard)returnedStages[returnedStages.Count-2][0]).power(questCard);
				//Does not beat previous stage
				if (foeCards[0].power(questCard) < stagePower) {
					findSuitableFoe (ref foeCards, questCard, stagePower, true);
				}
			}
			returnedStages [returnedStages.Count-1].Add (foeCards[0]);
			foeCards.RemoveAt(0);
		}
		returnedStages.Add(new List<Card> ());
		returnedStages [returnedStages.Count - 1].Add (foeCards [foeCards.Count - 1]);
		int bossPower = ((FoeCard)returnedStages [returnedStages.Count-1] [0]).power (questCard);
		setupBoss (ref returnedStages, ref weaponCards, questCard, 40);
		/*
		for (int i = 0; i < weaponCards.Count; i++) {
			if (!returnedStages [returnedStages.Count-1].Contains (weaponCards [i])) {
				returnedStages [returnedStages.Count-1].Add (weaponCards [i]);
				bossPower += weaponCards [i].bp;
				weaponCards.RemoveAt (i);
				i--;
			}
			if (bossPower >= 40) {
				break;
			}
		}
		*/
		return returnedStages;
	}
}
