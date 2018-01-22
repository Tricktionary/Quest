using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler{

	private int _type;
	private int _bp;

	public int Type {get {return _type;}}
	public int BP {get {return _bp;}}
	public Transform oldPosition = null; //Null By Default

	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log("OnBeginDrag");

		oldPosition = this.transform.parent;
		this.transform.SetParent(this.transform.parent.parent);

		GetComponent<CanvasGroup>().blocksRaycasts = false;

		CardArea[] zone = GameObject.FindObjectsOfType<CardArea>(); //Find Drop Zones

	}

	public void OnDrag(PointerEventData eventData){
		//Debug.Log("Drag");
		this.transform.position = eventData.position;

	}

	public void OnEndDrag(PointerEventData eventData){
		Debug.Log("OnEndDrag");
		this.transform.SetParent(oldPosition);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
		
}
