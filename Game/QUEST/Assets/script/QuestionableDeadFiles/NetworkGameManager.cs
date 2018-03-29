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
	//public GameObject playArea;

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
  public int currentNewId = 0;

  // List of players.
  public List<Player> _players = new List<Player>();

  private static NetworkGameManager _instance;

  public static NetworkGameManager  NetworkManager { get { return _instance; } }

  //Wake
  void Awake(){
    if(!_instance){
      _instance = this;
    }
    _discardPileAdventure = new Deck("");
    _discardPileStory = new Deck("");
    _adventureDeck = new Deck("Adventure");
    _storyDeck = new Deck("Story");
  }

  public void DrawStoryCard(){
      if(activeStoryCard){
        Debug.Log("A story Card has already been drawn.");
      }else{
        Debug.Log("Drawing Story Card");
        _storyCard = _storyDeck.Draw();
        GameObject storyCardObj = null;

        // Discard.
        _discardPileStory.Discard(_storyCard);

        //Re-shuffle
        if(_storyDeck.GetSize() == 0){
          //logger.info("The check has ran out, reshuffling the deck!");
          _storyDeck = _discardPileStory;
          _discardPileStory = new Deck ("");
        }

        // Load the card sprite.
  			Sprite storySprite = Resources.Load<Sprite>(_storyCard.asset);

        // A quest card has been drawn.
  			if (_storyCard.GetType() == typeof(QuestCard)) {
  				//logger.info("A quest card was drawn: " + _storyCard.name);
  				storyCardObj = Instantiate(QuestCard);
  				//_questBehaviour.setCurrentTurn(_currentPlayer);
  				//_questBehaviour.setCard(_storyCard);
  				// A tournament card been drawn.
  			} else if (_storyCard.GetType() == typeof(TournamentCard)) {
  				//logger.info("A tournament card was drawn: " + _storyCard.name);
  				storyCardObj = Instantiate(TournamentCard);
  				//_tournamentBehaviour.setCurrentTurn(_currentPlayer);
  				//_tournamentBehaviour.setCard(_storyCard);
  				// A event card has been drawn.
  			} else if (_storyCard.GetType() == typeof(EventCard)) {
  				//logger.info("A event card was drawn: " + _storyCard.name);
  				storyCardObj = Instantiate(EventCard);
  				//_eventBehaviour.setCurrentTurn(_currentPlayer);
  				//_eventBehaviour.setCard(_storyCard);
  			}

  			// Update the card.
  			storyCardObj.gameObject.GetComponent<Image>().sprite = storySprite;
  			storyCardObj.transform.SetParent(drawCardArea.transform);

  			// Indicate a story card is in play.
  			activeStoryCard = true;

  			// Clear the prompt.
  			//Prompt.PromptManager.statusPrompt("");

  			// Auto end the turn to prompt.
  			//EndTurn();

      }
  }

  //Get new ID to instantiate to player
  public int GetCurrentNewId(){
    currentNewId++;
    return currentNewId;
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

  //Update every frame
  void Update(){

  }


}
