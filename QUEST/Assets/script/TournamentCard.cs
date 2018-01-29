using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TournamentCard : Card
{
    int _shields;

	public TournamentCard(string name, int shields) {
		_name = name;
		_shields = shields;
	}
}