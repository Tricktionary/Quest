using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AdventureCard : Card,IBeginDragHandler,IDragHandler, IEndDragHandler
{
	//Action that occurs when you begin to drag
	public void OnBeginDrag(PointerEventData eventData){
		//Debug.Log("OnBeginDrag");
		 
		oldPosition = this.transform.parent;
		this.transform.SetParent(this.transform.parent.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		CardArea[] zone = GameObject.FindObjectsOfType<CardArea>(); //Find Drop Zones
	}

	//Action beginnning onDrag
	public void OnDrag(PointerEventData eventData){
		//Debug.Log("Drag");
		this.transform.position = eventData.position;

	}

	//Action that occurs at the end of the drag
	public void OnEndDrag(PointerEventData eventData){
		//Debug.Log("OnEndDrag");
		this.transform.SetParent(oldPosition);
		//this.transform.parent.GetComponent<CardArea>().removeCard(this);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	//Change Card Sprite to back of card
	public void flip(){

	}

	//Change Card Sprite to front of card
	public void unFlip(){

	}

}