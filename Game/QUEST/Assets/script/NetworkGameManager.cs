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
	public GameObject Hand;
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

  // Load a player.
  public void loadPlayer(int n){
//    logger.info("Loading player: " + (n + 1));
    foreach (Transform child in Hand.transform) {
      GameObject.Destroy(child.gameObject);
    }

    foreach (Transform child in playArea.transform) {
      GameObject.Destroy(child.gameObject);
    }

    // Set Player ID text.
    playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID: " + (n + 1);

    // Get current players shield.
    shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (_players[n].shieldCounter);

    // Load their hand.
    loadHand(n);
  }

  // Load the players hand onto the UI.
  void loadHand(int playerId){
    foreach (Transform child in rankCardArea.transform) {
      GameObject.Destroy(child.gameObject);
    }

    playArea.GetComponent<CardArea>().cards = new List<Card>();
    Hand.GetComponent<CardArea>().cards = new List<Card>();

    loadCards(_players[playerId].hand, Hand);
    loadCards(_players[playerId].inPlay, playArea);
    GameObject cardUI = Instantiate(RankCard);

    // Get the rank asset.
    string rankAsset = getRankAsset(_players[playerId].rank);

    // Set the rank asset.
    Sprite rankCard = Resources.Load<Sprite>(rankAsset);
    cardUI.gameObject.GetComponent<Image>().sprite = rankCard;
    cardUI.transform.SetParent(rankCardArea.transform);

  }

  // Load Cards
  public List<Card> InitHand(){
    List<Card> currCard = new List<Card>();

    for(int i = 0 ; i < 12 ; i ++){
      currCard.Add( _adventureDeck.Draw() );
    }
    return(currCard);
  }

  void Awake(){
    if(!_instance){
      _instance = this;
    }
    currentNewId = 0;
    _adventureDeck = new Deck("Adventure");
    _storyDeck = new Deck("Story");
  }

  // Load the cards up.
	public void loadCards(List<Card> cards, GameObject area){
		Card currCard;

		// Create Card GameObject's.
		for(int i = 0 ; i < cards.Count; i++){
			currCard = cards[i];

			area.GetComponent<CardArea>().addCard(currCard);
			GameObject CardUI = null;

			// TODO: Clean this up.
			if (currCard.GetType () == typeof(WeaponCard)) {
				WeaponCard currWeapon = (WeaponCard)currCard;
				CardUI = Instantiate (WeaponCard);
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
			} else if (currCard.GetType () == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)currCard;
				CardUI = Instantiate (FoeCard);
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().type    = currFoe.type;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			} else if (currCard.GetType () == typeof(AllyCard)) {
				AllyCard currAlly = (AllyCard)currCard;
				CardUI = Instantiate (AllyCard);
				CardUI.GetComponent<AllyCard>().name    = currAlly.name;
				CardUI.GetComponent<AllyCard>().asset   = currAlly.asset;
				CardUI.GetComponent<AllyCard>().special = currAlly.special;
				CardUI.GetComponent<AllyCard>().power   = currAlly.power;
				CardUI.GetComponent<AllyCard>().bid     = currAlly.bid;
				CardUI.GetComponent<AllyCard>().bonusPower = currAlly.bonusPower;
				CardUI.GetComponent<AllyCard>().bonusBid  =  currAlly.bonusBid;
				CardUI.GetComponent<AllyCard>().questCondition = currAlly.questCondition;
				CardUI.GetComponent<AllyCard>().allyCondition  = currAlly.allyCondition;
			} else if(currCard.GetType () == typeof(AmourCard)) {
				AmourCard currAmour = (AmourCard)currCard;
				CardUI = Instantiate (AmourCard);
				CardUI.GetComponent<AmourCard>().name = currAmour.name;
				CardUI.GetComponent<AmourCard>().asset = currAmour.asset;
				CardUI.GetComponent<AmourCard>().power = currAmour.power;
				CardUI.GetComponent<AmourCard>().bid = currAmour.bid;
			} else if(currCard.GetType () == typeof(TestCard)){
				TestCard currTest = (TestCard)currCard;
				CardUI = Instantiate (TestCard);
				CardUI.GetComponent<TestCard>().name = currTest.name;
				CardUI.GetComponent<TestCard>().asset = currTest.asset;
				CardUI.GetComponent<TestCard>().minBids = currTest.minBids;
			}

			// Load the card sprite.
			Sprite card = Resources.Load<Sprite>(currCard.asset);
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(area.transform);

			// Set the cards obj to it's UI.
			currCard.obj = CardUI;
		}
	}

  // Get the rank asset for a specific rank.
	public string getRankAsset(int rank){
		if(rank == 0){
			return "card_image/rank/rankCard1";
		} else if(rank == 1) {
			return "card_image/rank/rankCard2";
		} else if(rank == 2) {
			return "card_image/rank/rankCard3";
		}
		return null;
	}


  void Start(){

  }

  void Update(){
    if(_players.Count > 1 && loaded == false){
      loadPlayer(turnId);
      loaded = true;
    }
  }


}
