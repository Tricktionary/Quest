using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestCard : StoryCard
{
    
    int[] _playerId = new int[3];  //Do we need the players or just their ID?
    int _stages;
    string _featuredFoe;

	public QuestCard (string name, int stages, string featuredFoe) {
		_name = name;
		_stages = stages;
		_featuredFoe = featuredFoe;
	}

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Quest card:\nName: " + _name + "\nStages: " + _stages + "\nFeatured Foe: " + _featuredFoe;
	}

}