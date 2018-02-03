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
    
    public EventCard(string conditions){
        conditions = _conditions;
    }

	// ToString Override for nicer printing.
	public override string ToString(){
		return "Event card:\nName: " + _name + "\nConditions: " + _conditions;
	}
}