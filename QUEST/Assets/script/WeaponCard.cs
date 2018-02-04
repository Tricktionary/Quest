using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class WeaponCard : AdventureCard
{
    protected int _bp;
	protected bool _special;
	protected int _power;

	public int bp{
		get{
			return this._bp;
		}
		set{
			this._bp = value;
		}
	}

	public bool special{
		get{
			return this._special;
		}
		set{
			this._special = value;
		}
	}

	public int power{
		get{
			return this._power;
		}
		set{
			this._power = value;
		}
	}


	public WeaponCard(string name, int power, string asset) {
		_name = name;
		_power = power;
		_asset = asset;
	}


	// ToString Override for nicer printing.
	public override string ToString(){
		return "Weapon card:\nName: " + _name + "\nPower: " + _power + "\nAsset: " + _asset;
	}
}