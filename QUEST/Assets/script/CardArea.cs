using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArea :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

	public void OnPointerEnter(PointerEventData eventData){

	}
	public void OnPointerExit(PointerEventData eventData){

	}
	public void OnDrop(PointerEventData eventData){
		Debug.Log(eventData.pointerDrag.name + " Drop to "+ gameObject.name);

		Card d = eventData.pointerDrag.GetComponent<Card>();
		if(d!=null){
			d.oldPosition = this.transform;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
