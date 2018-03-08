using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	public List<Card> play1(int aiId,int testBid, bool isLastStage, List<Player> players) {
		Player ai = players [aiId];

		Dictionary<string,int> cardsWithPower = new Dictionary<string, int>();
		List<Card> cardPowerKeys = new List<Card> ();
		List<Card> sortedList = new List<Card> ();

		for (int i = 0; i < ai.hand.Count; i++) {
			if (!cardsWithPower.ContainsKey (ai.hand [i].name)) {
				if (ai.hand [i] is WeaponCard) {
					cardsWithPower [ai.hand [i].name] = ((WeaponCard)ai.hand [i]).bp;
					cardPowerKeys.Add (ai.hand [i]);
				} else if (ai.hand [i] is AllyCard) {
					cardsWithPower [ai.hand [i].name] = ((AllyCard)ai.hand [i]).power;
					cardPowerKeys.Add (ai.hand [i]);
				} else if (ai.hand [i] is AmourCard) {
					cardsWithPower [ai.hand [i].name] = ((AmourCard)ai.hand [i]).power;
					cardPowerKeys.Add (ai.hand [i]);
				}
			}
		}

		//selectionSort
		/*
		int times = cardsWithPower.Count;
		for (int z=0;z<times;z++) {
			int highest=-1;
			int index=-1;
			for (int i = 0; i < cardPowerKeys.Count; i++) {
				if (highest < cardsWithPower [cardPowerKeys [i]]) {
					index = i;
					highest = cardsWithPower [cardPowerKeys [i]];
				}
			}
			sortedList.Add (cardPowerKeys [index]);
			cardPowerKeys.RemoveAt(index);
		}
		*/

		cardPowerKeys.Sort((x, y) => cardsWithPower[x.name].CompareTo(cardsWithPower[y.name]));
		cardPowerKeys.Reverse ();
		sortedList = cardPowerKeys;

		Dictionary<Card,int> cards = new Dictionary<Card, int>();
		List<Card> keys = new List<Card>();
		bool amour = true;
		int played = 0;
		if (isLastStage) {
			return sortedList;
		} else {
			for (int i = 0; i < sortedList.Count; i++) {
				if (sortedList[i] is AllyCard) {
					keys.Add(sortedList[i]);
					sortedList.RemoveAt (i);
					i--;
					played += 1;
				} else if (sortedList[i] is AmourCard && amour) {
					keys.Add(sortedList[i]);
					sortedList.RemoveAt (i);
					i--;
					played += 1;
					amour = false;
				}
				if (played == 2) {
					return keys;
				}
			}
		}
		played = 0;
		List<Card> weapons = new List<Card>();
		for (int i = sortedList.Count-1; i >= 0; i--) {
			if (sortedList [i] is WeaponCard) {
				weapons.Add (sortedList [i]);
				played += 1;
			}
			if (played == 2) {
				keys.AddRange (weapons);
				return keys;
			}
		}
		return null;
	}
}
