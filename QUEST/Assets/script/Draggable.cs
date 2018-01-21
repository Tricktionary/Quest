using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler {

	public Transform parentToReturnTo = null; //Null By Default

	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log("OnBeginDrag");
		
		parentToReturnTo = this.transform.parent;
		this.transform.SetParent(this.transform.parent.parent);
		
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData){
		//Debug.Log("Drag");

		this.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData){
		Debug.Log("OnEndDrag");
		this.transform.SetParent(parentToReturnTo);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

}
