using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class Card : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler,IDropHandler{

	public string _name {
		get;
		set;
	}
	public string _asset{
		get;
		set;
	}

	public Transform oldPosition = null; 	//Old Position of the card on the board
	
	//Action that occurs when you begin to drag
	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log("OnBeginDrag");
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
		Debug.Log("OnEndDrag");
		this.transform.SetParent(oldPosition);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
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
}
