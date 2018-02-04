using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class Card : MonoBehaviour{

	public Transform oldPosition = null; 	//Old Position of the card on the board

	protected string _asset;
	protected string _name;

	public string name {
		get{
			return this._name;
		}
		set{
			this._name = value;
		}
	}

	public string asset{
		get{
			return this._asset;
		}
		set{
			this._asset = value;
		}
	}
		
}

