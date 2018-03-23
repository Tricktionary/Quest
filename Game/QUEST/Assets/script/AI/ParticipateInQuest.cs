using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParticipateInQuest: AIBehaviour{

	//Strategy 1
	public bool join1(int aiId,int stages, List<Player> players) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		int allyCards = extractAllies (hand).Count;
		int weaponCards = extractWeapons(hand).Count;
		List<FoeCard> foeCards = extractFoes (hand);
		int weakFoes = 0;
		for (int i = 0; i < foeCards.Count; i++) {
			if (foeCards[i].loPower < 20) {
				weakFoes += 1;
			}
		}

		if ((weaponCards + allyCards) / stages >= 2 && weakFoes >= 2) {
			return true;
		}
		return false;
	}

	public List<Card> play1(int aiId,int testBid, bool isLastStage, List<Player> players) {
		Player ai = players [aiId];

		List<Card> hand = ai.hand;
		List<AllyCard> allyCards = extractAllies (hand);
		List<WeaponCard> weaponCards = extractWeapons(hand);
		List<FoeCard> foeCards = extractFoes (hand);
		List<AmourCard> amourCards = extractAmours (hand);
		List<Card> playFromHand = new List<Card>();

		allyCards.Sort ((x, y) => x.power.CompareTo (y.power));
		weaponCards.Sort ((x, y) => x.power.CompareTo (y.power));
		foeCards.Sort ((x, y) => x.loPower.CompareTo (y.loPower));

		bool amourPlayed = false;
		if (ai.inPlay.Contains(new AmourCard("Amour",0,0,""))) {
			amourPlayed = true;
		}
		int played = 0;
		if (isLastStage) {
			return strongestCombination (hand);
		} else {
			if (!amourPlayed && amourCards.Count > 0) {
				playFromHand.Add (amourCards [0]);
				played++;
			}
			for (int i = 0; i < allyCards.Count; i++) {
				playFromHand.Add (allyCards [i]);
				played++;
				if (played == 2) {
					return playFromHand;
				}
			}
		}
		played = 0;
		for (int i = 0; i < weaponCards.Count; i++) {
			if (!playFromHand.Contains(weaponCards[i])) {
				playFromHand.Add (weaponCards [i]);
				played++;
				if (played == 2) {
					break;
				}
			}
		}
		return playFromHand;
	}



	//Strategy 2
	public bool join2(int aiId,int stages, List<Player> players) {
		//Allies should be played incrementally first then weapons sparingly
		Player ai = players [aiId];
		int discardFoes = 0;
		List<Card> hand = ai.hand;
		List<AllyCard> allyCards = extractAllies (hand);
		List<WeaponCard> weaponCards = extractWeapons(hand);
		List<FoeCard> foeCards = extractFoes (hand);
		List<AmourCard> amourCards = extractAmours (hand);
		if (amourCards.Count > 0) {
			stages--;
		}
		for (int i = 0; i < foeCards.Count; i++) {
			if (foeCards [i].loPower < 20) {
				discardFoes += 1;
			}
		}
		allyCards.Sort((x, y) => x.power.CompareTo(y.power));
		weaponCards.Sort((x, y) => x.power.CompareTo(y.power));
		for (int i=0;i<stages;i++) {
			int tempPower = 0;
			bool done = false;
			for (int j=0;j<allyCards.Count;j++) {
				tempPower += allyCards [0].power;
				allyCards.RemoveAt (0);
				j--;
				if (tempPower > 10) {
					done = true;
					break;
				}
			}
			if (!done) {
				Card previousCard = null;
				for (int j=0;j<weaponCards.Count;j++) {
					if (previousCard.Equals (weaponCards [j])) {
						previousCard = weaponCards [j];
						tempPower += weaponCards [j].power;
						weaponCards.RemoveAt (j);
						j--;
						if (tempPower > 10) {
							done = true;
							break;
						}
					}
				}
			}
			if (!done) {
				return false;
			}
		}
		if (discardFoes >= 2) {
			return true;
		}
		return false;
	}

	public List<Card> play2(int aiId,int testBid, bool isLastStage, List<Player> players) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		List<AllyCard> allies = extractAllies(hand);
		List<WeaponCard> weapons = extractWeapons(hand);
		List<AmourCard> amourCards = extractAmours (hand);
		List<Card> keys = new List<Card>();
		if (!ai.inPlay.Contains (new AmourCard ("Amour", 0, 0, "")) && amourCards.Count > 0) {
			keys.Add (amourCards [0]);
			return keys;
		}

		allies.Sort((x, y) => x.power.CompareTo(y.power));
		weapons.Sort((x, y) => x.power.CompareTo(y.power));

		bool done = false;
		int tempPower = 0;
		for (int j=0;j<allies.Count;j++) {
			tempPower += allies [0].power;
			keys.Add (allies [0]);
			allies.RemoveAt (0);
			j--;
			if (tempPower > 10) {
				done = true;
				break;
			}
		}
		if (!done) {
			Card previousCard = null;
			for (int j=0;j<weapons.Count;j++) {
				if (previousCard.Equals (weapons [j])) {
					previousCard = weapons [j];
					tempPower += weapons [j].power;
					keys.Add (weapons [j]);
					weapons.RemoveAt (j);
					j--;
					if (tempPower > 10) {
						done = true;
						break;
					}
				}
			}
		}

		return keys;
	}

}
