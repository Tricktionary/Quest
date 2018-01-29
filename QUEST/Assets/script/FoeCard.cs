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

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Foe card:\nName: " + _name + "\nLow Power: " + _loPower + "\nHigh Power: " + _hiPower +
			"/nSpecial" + _special;
	}
}