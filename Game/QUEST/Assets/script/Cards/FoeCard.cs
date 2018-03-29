using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class FoeCard : AdventureCard
{
	private bool _special;
	private int _loPower;
	private int _hiPower;
	private string _type;

	public string type{
		get{
			return this._type;
		}
		set{
			this._type = value;
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

	public int loPower{
		get{
			return this._loPower;
		}
		set{
			this._loPower = value;
		}
	}

	public int hiPower{
		get{
			return this._hiPower;
		}
		set{
			this._hiPower = value;
		}
	}

	public int power(QuestCard quest) {
		if (!quest.featuredFoe.Equals ("") && _name.Contains(quest.featuredFoe) || quest.featuredFoe.Equals ("*")) {
			return _hiPower;
		}
		return _loPower;
	}



	public FoeCard(string name, string type,  int loPower, int hiPower, bool special, string asset) {
		_name = name;
		_type = type;
		_loPower = loPower;
		_hiPower = hiPower;
		_special = special;
		_asset = asset;
		_draggable = true;
	}

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Foe card:\nName: " + _name + "\n _type" + _type + "\nLow Power: " + _loPower + "\nHigh Power: " + _hiPower +
			"/nSpecial" + _special;
	}
}