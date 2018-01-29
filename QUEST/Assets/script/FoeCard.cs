using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoeCard : AdventureCard
{
	bool _special;
	int _loPower;
	int _hiPower;

	public FoeCard(string name, int loPower, int hiPower, bool special) {
		_name = name;
		_loPower = loPower;
		_hiPower = hiPower;
		_special = special;
	}
}