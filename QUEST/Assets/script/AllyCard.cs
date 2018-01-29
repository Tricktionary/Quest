using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllyCard : Card
{
	bool _special;
	int _power;
	int _bid;
	int _bonusPower;
	int _bonusBid;
	string _questCondition;
	string _allyCondition;

	public AllyCard(string name, int power, int bid, int bonusPower, int bonusBid, string questCondition, string allyCondition, bool special) {
		_name = name;
		_power = power;
		_bid = bid;
		_bonusPower = bonusPower;
		_bonusBid = bonusBid;
		_questCondition = questCondition;
		_allyCondition = allyCondition;
		_special = special;
	}

}