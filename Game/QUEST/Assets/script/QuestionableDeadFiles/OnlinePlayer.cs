using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnlinePlayer : Player {

	public string playerName;
	public int connectionId;
	//public GameObject playerField;

	public OnlinePlayer(string name, int cnnId ){
		//playerField = go;
		connectionId = cnnId;
		_playerDisplay = cnnId;
		_rank = 0;
		_shieldCounter = 0;
		_bp = 5;
		_hand = new List<Card>();
		_allies = new List<Card>();
		_inPlay = new List<Card>();
	}


}
