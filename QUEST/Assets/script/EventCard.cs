using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventCard : StoryCard
{
    private string _conditions;

    public string conditions{
		get{
			return this._conditions;
		}
		set{
			this._conditions = value;
		}
	}
    
    public EventCard(string name, string conditions, string asset){
       _name = name;
       _conditions = conditions;
       _asset = asset;
    }

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Event card:\nName: " + _name + "\nConditions: " + _conditions;
	}
}