using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerObject : NetworkBehaviour {

  public Player thisPlayer;

  //Card Types
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

  // Text fields.
  public GameObject playerIdTxt;
  public GameObject shieldCounterTxt;


  //PlayArea
  public GameObject PlayArea;

  //Hand
  public GameObject Hand;

  //Rank Card Area
  public GameObject RankCardArea;


  void Awake(){
    //Debug.Log("Player Load");

  }

  void Start(){
    Debug.Log(NetworkGameManager.NetworkManager);
    int id = NetworkGameManager.NetworkManager.GetCurrentNewId();
    //Debug.Log("ID");
    thisPlayer = new Player(id);
    thisPlayer.hand = NetworkGameManager.NetworkManager.InitHand();
    NetworkGameManager.NetworkManager._players.Add(thisPlayer);
    loadHand();
  }


  // Load the players hand onto the UI.
  void loadHand(){
    playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID: " + (thisPlayer.playerId);

    // Get current players shield.
    shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (thisPlayer.shieldCounter);

    foreach (Transform child in RankCardArea.transform) {
      GameObject.Destroy(child.gameObject);
    }

    PlayArea.GetComponent<CardArea>().cards = new List<Card>();
    Hand.GetComponent<CardArea>().cards = new List<Card>();

    loadCards(thisPlayer.hand, Hand);
    loadCards(thisPlayer.inPlay, PlayArea);
    GameObject cardUI = Instantiate(RankCard);

    // Get the rank asset.
    string rankAsset = getRankAsset(thisPlayer.rank);

    // Set the rank asset.
    Sprite rankCard = Resources.Load<Sprite>(rankAsset);
    cardUI.gameObject.GetComponent<Image>().sprite = rankCard;
    cardUI.transform.SetParent(RankCardArea.transform);
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
				CardUI = Instantiate (WeaponCard,new Vector3(), Quaternion.identity);
        //CardUI = Instantiate(WeaponCard ,area.transform);
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
			} else if (currCard.GetType () == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)currCard;
				CardUI = Instantiate (FoeCard,new Vector3(), Quaternion.identity);
        //CardUI = Instantiate(FoeCard,area.transform);
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().type    = currFoe.type;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			} else if (currCard.GetType () == typeof(AllyCard)) {
				AllyCard currAlly = (AllyCard)currCard;
				CardUI = Instantiate (AllyCard,new Vector3(), Quaternion.identity);
        //CardUI = Instantiate(AllyCard,area.transform);
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
				CardUI = Instantiate (AmourCard,new Vector3(), Quaternion.identity);
        CardUI = Instantiate(AmourCard,area.transform);
				CardUI.GetComponent<AmourCard>().name = currAmour.name;
				CardUI.GetComponent<AmourCard>().asset = currAmour.asset;
				CardUI.GetComponent<AmourCard>().power = currAmour.power;
				CardUI.GetComponent<AmourCard>().bid = currAmour.bid;
			} else if(currCard.GetType () == typeof(TestCard)){
				TestCard currTest = (TestCard)currCard;
				CardUI = Instantiate (TestCard,new Vector3(), Quaternion.identity);
        //CardUI = Instantiate(TestCard,area.transform);
				CardUI.GetComponent<TestCard>().name = currTest.name;
				CardUI.GetComponent<TestCard>().asset = currTest.asset;
				CardUI.GetComponent<TestCard>().minBids = currTest.minBids;
			}

			// Load the card sprite.
			Sprite card = Resources.Load<Sprite>(currCard.asset);
			CardUI.gameObject.GetComponent<Image>().sprite = card;

      currCard.obj = CardUI;
      currCard.obj.transform.parent = area.transform;
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





}
