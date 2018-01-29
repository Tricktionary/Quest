using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponCard : AdventureCard
{
    private int _bp;
	bool _special;
	int _power;
	public WeaponCard(string name, int power, string asset) {
		_name = name;
		_power = power;
		_asset = asset;
	}
}