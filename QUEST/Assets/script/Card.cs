using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class Card : MonoBehaviour{

	public Transform oldPosition = null; 	//Old Position of the card on the board

	public string _name {
		get;
		set;
	}
	public string _asset{
		get;
		set;
	}
		
}

