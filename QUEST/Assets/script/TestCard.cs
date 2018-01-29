using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestCard : Card
{
	string _name;
	int _minBids;

	public TestCard(string name, int minBids) {
		_name = name;
		_minBids = minBids;
	}
}