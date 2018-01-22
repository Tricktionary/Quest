using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler{

	private int _type
	{ 
		get; 
		set;
	}

	private int _bp 
	{ 
		get; 
		set;
	}

	public int Type {get {return _type;}}
	public int BP {get {return _bp;}}
	public Button btn;

	public Transform oldPosition = null; //Null By Default

	//void Start(){
	//	btn = gameObject.GetComponent<Button> ();
	//	btn.onClick.AddListener(OnBeginDrag);
	//}

	public void OnBeginDrag(PointerEventData eventData){
		
		Debug.Log("OnBeginDrag");
		Debug.Log (_bp);
		_bp = 5;
		Debug.Log (_bp);

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
