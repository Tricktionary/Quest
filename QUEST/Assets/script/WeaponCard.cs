using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponCard : Card
{
    private int _bp;
	bool _special;
	int _power;
	int _bid;
	public WeaponCard(string name, int power, int bid, bool special) {
		_name = name;
		_power = power;
		_bid = bid;
		_special = special;
	}
}