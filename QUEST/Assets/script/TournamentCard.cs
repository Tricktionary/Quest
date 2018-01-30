using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TournamentCard : StoryCard
{
    int _shields;

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