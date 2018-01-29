using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllyCard : AdventureCard
{
	bool _special;
	int _power;
	int _bid;
	int _bonusPower;
	int _bonusBid;
	string _questCondition;
	string _allyCondition;

	public AllyCard(string name, int power, int bid, int bonusPower, int bonusBid, string questCondition, string allyCondition, bool special, string asset) {
		_name = name;
		_power = power;
		_bid = bid;
		_bonusPower = bonusPower;
		_bonusBid = bonusBid;
		_questCondition = questCondition;
		_allyCondition = allyCondition;
		_special = special;
		_asset = asset;
	}

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Ally card:\nName: " + _name + "\nPower: " + _power + "\nBid: " + _bid + "\nBonus Power: " + _bonusPower + 
			"\nBonus Bid: " + _bonusBid + "\nQuest Condition: " + _questCondition + "\nAlly Condition: " + _allyCondition + 
			"\nSpecial: " + _special + "\nAsset: " + _asset;
	}
}