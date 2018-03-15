using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestCard : AdventureCard
{
	int _minBids;

	public int minBids{
		get{
			return this._minBids;
		}
		set{
			this._minBids = value;
		}
	}

	public TestCard(string name, int minBids, string asset) {
		_name = name;
		_minBids = minBids;
		_asset = asset;
	}
}
