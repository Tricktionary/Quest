using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class TournamentCard : StoryCard
{
    protected int _shields;

    public int shields{
		get{
			return this._shields;
		}
		set{
			this._shields = value;
		}
	}

	public TournamentCard(string name, int shields, string asset) {
		_name = name;
		_shields = shields;
		_asset = asset;
	}

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Tournament card:\nName: " + _name + "\nShields: " + _shields;
	}
}