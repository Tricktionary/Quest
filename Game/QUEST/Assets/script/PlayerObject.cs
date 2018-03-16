using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour {

  // Prefabs.
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

  // Show player stuff.
  public GameObject playerPanel;
  public List<GameObject> playerActive;
  public List<GameObject> numCardText;
  public List<GameObject> shieldCounterList;
  public List<GameObject> rankTextList;

  //Stages
  public List<GameObject> Stages;

  // Misc GameObject's.
  public GameObject currStageTxt;
  public GameObject discardPile;
  public GameObject rankCardArea;
  public GameObject drawCardArea;
  public GameObject Hand;

  public GameObject drawCardButton;
  public GameObject endTurnButton;
  public GameObject openPlayerPanel;
  public GameObject closePlayerPanel;
  public GameObject showHandButton;

  // Text fields.
  public GameObject playerIdTxt;
  public GameObject shieldCounterTxt;

  public Player thisPlayer;
  public GameObject playArea;


  NetworkClient myClient;

  void Update(){

  }

  //
  void Awake(){
    Debug.Log("Player Load");
  }

}
