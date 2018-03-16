using System.Collections.Generic;
using UnityEngine;
public class AIBehaviour {
	protected List<WeaponCard> extractWeapons(List<Card> hand) {
		List<WeaponCard> weaponCards = new List<WeaponCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i] is WeaponCard) {
				WeaponCard currWeapon = (WeaponCard)hand [i];
				weaponCards.Add (currWeapon);
			}
		}
		return weaponCards;
	}
	protected List<AllyCard> extractAllies(List<Card> hand) {
		List<AllyCard> allyCards = new List<AllyCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i] is AllyCard) {
				AllyCard currAlly = (AllyCard)hand [i];
				allyCards.Add (currAlly);
			}
		}
		return allyCards;
	}
	protected List<FoeCard> extractFoes(List<Card> hand) {
		List<FoeCard> foeCards = new List<FoeCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i] is FoeCard) {
				FoeCard currFoe = (FoeCard)hand [i];
				foeCards.Add (currFoe);
			}
		}
		return foeCards;
	}

	protected List<AmourCard> extractAmours(List<Card> hand) {
		List<AmourCard> amourCards = new List<AmourCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i] is AmourCard) {
				AmourCard currAmour = (AmourCard)hand [i];
				amourCards.Add (currAmour);
			}
		}
		return amourCards;
	}

	protected List<TestCard> extractTests(List<Card> hand) {
		List<TestCard> testCards = new List<TestCard>();
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i] is TestCard) {
				TestCard currTest = (TestCard)hand [i];
				testCards.Add (currTest);
			}
		}
		return testCards;
	}

	protected List<Card> strongestCombination(List<Card> hand) {
		List<Card> strongest = new List<Card> ();
		for (int i = 0; i < hand.Count; i++) {
			if (!strongest.Contains (hand [i])) {
				if (hand [i] is AllyCard || hand [i] is WeaponCard || hand [i] is AmourCard) {
					strongest.Add (hand [i]);
				}
			} else {
			}
		}
		return strongest;
	}

}

