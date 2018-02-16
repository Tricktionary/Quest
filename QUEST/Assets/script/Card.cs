using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public abstract class Card : MonoBehaviour {

	
	public Transform oldPosition = null;     //Old Position of the card on the board

	protected string _asset;
	protected string _name;
	protected bool _flipped = false;
	protected GameObject _obj;

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