using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArea :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

	//Current Cards in this area
	private List<Card> _cards = new List<Card>();
	private bool _isStoryArea;
	private bool _isAdventureArea;
	private bool _isHand;
	private bool _inPlay;


	public bool isStoryArea{
		get;
		set;
	} 
	public bool isAdventureArea{
		get;
		set;
	}
	public bool isHand{
		get;
		set;
	}
	public bool inPlay{
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
		Card currCard = eventData.pointerDrag.GetComponent<Card>();
		
		if(currCard!=null){
			_cards.Add(currCard);
			for(int i = 0 ; i < _cards.Count; i++){
				Debug.Log(_cards[i].name);
			}
			//_area.Add (d);
			//Debug.Log (_area[0].name);
			currCard.oldPosition = this.transform;
		}

	}
	//Listener for when a mouse enter
	public void OnPointerEnter(PointerEventData eventData){
	}
	//Listener when a mouse exits
	public void OnPointerExit(PointerEventData eventData){
	}

}
