using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArea :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

	//Current Cards in this area
	private List<Card> _cards = new List<Card>();
	private bool _acceptObj = true;

	public List<Card> cards{
		get{
			return this._cards;
		}
		set{
			this._cards = value;
		}
	}

	public bool acceptObj {
		get{
			return this._acceptObj;
		}
		set{
			this._acceptObj = value;
		}
	}
		
	// Remove and return from card area.
	public Card removeCard(Card card){
		for (int i = 0; i < _cards.Count; i++){
			if (_cards[i].name == card.name){
				Card c = _cards[i];
				_cards.RemoveAt(i);
				return c;
			}
		}
		return null;
	}

	public void addCard(Card card){
		_cards.Add(card);
	}

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {	
	}

	//When a card is dropped
	public void OnDrop(PointerEventData eventData){
		//Debug.Log(eventData.pointerDrag.name + " Drop to "+ gameObject.name);
		if(acceptObj == true){
			Card currCard = eventData.pointerDrag.GetComponent<Card>();
			if(currCard!=null){
				// Remove the card from the card area.
				Card c = currCard.oldPosition.GetComponent<CardArea>().removeCard(currCard);
				// Add the removed card to the new card area.
				_cards.Add(c);
				// Move the actual card.
				currCard.oldPosition= this.transform;
			}
		}
	}

	//Listener for when a mouse enter
	public void OnPointerEnter(PointerEventData eventData){
	}
	//Listener when a mouse exits
	public void OnPointerExit(PointerEventData eventData){
	}

}
