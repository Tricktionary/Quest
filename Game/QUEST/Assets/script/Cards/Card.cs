using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public abstract class Card : MonoBehaviour, IEquatable<Card> {

	public Transform oldPosition = null;     //Old Position of the card on the board

	public string _asset;
	public string _name;
	public bool _flipped = false;
	public GameObject _obj;
	public bool _draggable = false;

	public bool draggable{
		get {
			return this._draggable;
		}
		set{
			this._draggable = value;
		}
	}
	public GameObject obj {
		get{
			return this._obj;
		}
		set{
			this._obj = value;
		}
	}

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

	public bool flipped{
		get{
			return this._flipped;
		}
		set{
			this._flipped = value;
		}
	}

	// Flip a card over.
	public void flipCard(bool f){
		if(_obj != null){
			_flipped = f;
			Sprite card_image;
			if(_flipped){
				card_image = Resources.Load<Sprite>("card_image/special/backOfCard");
			} else {
				card_image = Resources.Load<Sprite>(this.asset);
			}
			_obj.GetComponent<Image>().sprite = card_image;
		}
	}

	public bool Equals(Card other) {
		if (other.name.Equals (_name)) {
			return true;
		} else {
			return false;
		}
	}

}
