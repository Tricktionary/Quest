using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestCard : Card
{
	int _minBids;

	public TestCard(string name, int minBids, string asset) {
		_name = name;
		_minBids = minBids;
		_asset = asset;
	}
}