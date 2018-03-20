using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkGameManager : NetworkBehaviour
{

  public GameObject Card;
	public GameObject WeaponCard;
	public GameObject FoeCard;
	public GameObject AllyCard;
	public GameObject QuestCard;
	public GameObject AmourCard;
	public GameObject TestCard;
	public GameObject EventCard;

	public GameObject RankCard;
	public GameObject TournamentCard;

	// Panels.
	public GameObject playerPanel;
	public GameObject playArea;

	// Stages.
	public List<GameObject> Stages;

	// Show player stuff.
	public List<GameObject> playerActive;
	public List<GameObject> numCardText;
	public List<GameObject> shieldCounterList;
	public List<GameObject> rankTextList;


	// Misc GameObject's.
	public GameObject currStageTxt;
	public GameObject discardPile;
	public GameObject rankCardArea;
	public GameObject drawCardArea;
	//public GameObject Hand;
	public GameObject winScreen;
	public GameObject winScreenTxt;

	// Text fields.
	public GameObject playerIdTxt;
	public GameObject shieldCounterTxt;

	// Decks.
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private Deck _discardPileAdventure;
	private Deck _discardPileStory;

	// The current story card in play.
	Card _storyCard;
	bool activeStoryCard = false;

  public int turnId = 0;

 	public bool bonusQuestPoints = false;

  public bool loaded = false;

  //Player ID that needs to be instantiated
  public int currentNewId;

  // List of players.
  public List<Player> _players = new List<Player>();

  private static NetworkGameManager _instance;

  public static NetworkGameManager  NetworkManager { get { return _instance; } }


  public int GetCurrentNewId(){
    currentNewId++;
    return currentNewId;
  }

  //Wake
  void Awake(){
    if(!_instance){
      _instance = this;
    }
    currentNewId = 0;
    _adventureDeck = new Deck("Adventure");
    _storyDeck = new Deck("Story");
  }

  //Give Players Their Card Hands
  public List<Card> InitHand(){
    List<Card> newHand = new List<Card>();

    for(int i = 0 ; i < 12 ; i ++){
      newHand.Add(_adventureDeck.Draw());
    }
    return newHand;
  }


  //Start Runs After PLayers Join
  void Start(){

  }

  void Update(){

  }


}
