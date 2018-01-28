﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArea :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

	//Current Cards in this area
	private List<Card> _cards = new List<Card>();
	private bool _isStoryArea{
		get;
		set;
	} 
	private bool _isAdventureArea{
		get;
		set;
	}
	private bool _isHand{
		get;
		set;
	}

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {	
	}

	//When a card is dropped
	public void OnDrop(PointerEventData eventData){
		Debug.Log(eventData.pointerDrag.name + " Drop to "+ gameObject.name);
		Card d = eventData.pointerDrag.GetComponent<Card>();

		if(d!=null){
			//_area.Add (d);
			//Debug.Log (_area[0].name);
			d.oldPosition = this.transform;
		}

	}
	//Listener for when a mouse enter
	public void OnPointerEnter(PointerEventData eventData){
	}
	//Listener when a mouse exits
	public void OnPointerExit(PointerEventData eventData){
	}

}
