using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBid : AIBehaviour {

	public void extractFoes(List<Card> hand, List<Card> foeCards, int minPower) {
	}

	//Strategy 1
	public int nextBid1(int aiId, List<Player> players, int currBid, bool multipleBids) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		int weakFoes = 0;
		//first bid
		if (!multipleBids) {
			List<FoeCard> foeCards = extractFoes (hand);
			for (int i = 0; i < foeCards.Count; i++) {
				if (foeCards[i].loPower < 20) {
					weakFoes++;
				}
			}
		}
		if (weakFoes > currBid) {
			return weakFoes;
		}
		return 0;
	}

	//Strategy 1
	public List<Card> discardAfterWinningTest1(int aiId, List<Player> players, int amt, bool multipleBids) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		List<Card> discards = new List<Card> ();
		List<FoeCard> foeCards = extractFoes (hand);
		for (int i = 0; i < foeCards.Count; i++) {
			if (foeCards [i].loPower < 20) {
				foeCards.Add (foeCards[i]);
			}
		}
		foeCards.Sort ((x, y) => x.loPower.CompareTo (y.loPower));
		for (int i = 0; i < amt; i++) {
			discards.Add(foeCards [i]);
		}
		return discards;
	}

	//Strategy 2
	public int nextBid2(int aiId, List<Player> players, int currBid, bool multipleBids) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		int discards = 0;
		List<FoeCard> foeCards = extractFoes (hand);
		for (int i = 0; i < foeCards.Count; i++) {
			if (foeCards[i].loPower < 25) {
				discards++;
			}
		}
		//second bid
		if (multipleBids) {
			discards = 0;
			HashSet<Card> uniqueCards = new HashSet<Card> ();
			for (int i = 0; i < hand.Count; i++) {
				if (uniqueCards.Contains(hand[i])) {
					discards++;
				} else if (hand [i] is FoeCard) {
					if (((FoeCard)hand [i]).loPower < 25) {
						discards++;
					}
				} else {
					uniqueCards.Add (hand [i]);
				}
			}
		}
		if (discards > currBid) {
			return discards;
		}
		return 0;
	}

	//Strategy 2
	public List<Card> discardAfterWinningTest2(int aiId, List<Player> players, int amt, bool multipleBids) {
		Player ai = players [aiId];
		List<Card> hand = ai.hand;
		List<Card> discards = new List<Card> ();
		List<FoeCard> foeCards = extractFoes (hand);
		if (!multipleBids) {
			for (int i = 0; i < foeCards.Count; i++) {
				if (foeCards [i].loPower < 25) {
					foeCards.Add (foeCards [i]);
				}
			}
			foeCards.Sort ((x, y) => x.loPower.CompareTo (y.loPower));
			for (int i = 0; i < amt; i++) {
				discards.Add (foeCards [i]);
			}
		} else {
			HashSet<Card> uniqueCards = new HashSet<Card> ();
			for (int i = 0; i < hand.Count; i++) {
				if (uniqueCards.Contains(hand[i])) {
					discards.Add (hand [i]);
				} else if (hand [i] is FoeCard) {
					if (((FoeCard)hand [i]).loPower < 25) {
						discards.Add (hand [i]);
					}
				} else {
					uniqueCards.Add (hand [i]);
				}
			}
		}
		return discards;
	}

}

