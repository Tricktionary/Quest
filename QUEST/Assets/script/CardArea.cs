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

	public List<Card> cards{
		get{
			return this._cards;
		}
		set{
			this._cards = value;
		}
	}
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

	// When a card is dropped
	public void OnDrop(PointerEventData eventData){
		Card currCard = eventData.pointerDrag.GetComponent<Card>();
		if(currCard != null){
			// Remove the card from the card area.
			Card c = currCard.oldPosition.GetComponent<CardArea>().removeCard(currCard);
			// Add the removed card to the new card area.
			_cards.Add(c);
			// Move the actual card.
			currCard.oldPosition= this.transform;
		}
	}

	// Listener for when a mouse enter
	public void OnPointerEnter(PointerEventData eventData){
	}

	// Listener when a mouse exits
	public void OnPointerExit(PointerEventData eventData){
	}

}
