using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class AmourCard : AdventureCard
{ 
	int _power;
	int _bid;

	public int power{
		get{
			return this._power;
		}
		set{
			this._power = value;
		}
	}

	public int bid{
		get{
			return this._bid;
		}
		set{
			this._bid = value;
		}
	}

	public AmourCard(string name, int power, int bid, string asset){
		_name = name;
		_power = power;
		_bid = bid;
		_asset = asset;

	}

	public override string ToString(){
		return "Ally card:\nName: " + _name + "\nPower: " + _power + "\nBid: " + _bid + "\nAsset: " + _asset;
	}


}