using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestCard : StoryCard
{

    private int[] _playerId = new int[3];  //Do we need the players or just their ID?
    private int _stages;
    private string _featuredFoe;


    //getters and setters
    public int[] playerId{ //not sure about get/set for array
		get{
			return this._playerId;
		}
		set{
			this._playerId = value;
		}
	}

	public int stages{
		get{
			return this._stages;
		}
		set{
			this._stages = value;
		}
	}

	public string featuredFoe{
		get{
			return this._featuredFoe;
		}
		set{
			this._featuredFoe = value;
		}
	}



	public QuestCard (string name, int stages, string featuredFoe, string asset) {
		_name = name;
		_stages = stages;
		_featuredFoe = featuredFoe;
		_asset = asset;
		oldPosition = null;
	}

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Quest card:\nName: " + _name + "\nStages: " + _stages + "\nFeatured Foe: " + _featuredFoe;
	}

}
