using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonEventHandler : MonoBehaviour {
  public GameObject yesButton;
  public GameObject noButton;
  public GameObject endTurnButton;
  public GameObject drawCardButton;

  void Awake(){
    yesButton.GetComponent<Button>().onClick.AddListener(PhotonYesClick);
		noButton.GetComponent<Button>().onClick.AddListener(PhotonNoClick);
		endTurnButton.GetComponent<Button>().onClick.AddListener(PhotonEndTurn);
		drawCardButton.GetComponent<Button>().onClick.AddListener(PhotonDraw);
  }

  /* Photon Function */
  public void PhotonDraw(){
  	Debug.Log("Photon Draw Card");
  	this.GetComponent<PhotonView>().RPC("DrawCard",PhotonTargets.All);
  }
  public void PhotonEndTurn(){
  	Debug.Log("Photon End Turn");

  	this.GetComponent<PhotonView>().RPC("EndTurn",PhotonTargets.All);
  }
  public void PhotonYesClick(){
  	Debug.Log("Photon Yes Click");
  	this.GetComponent<PhotonView>().RPC("YesClick",PhotonTargets.All);
  }
  public void PhotonNoClick(){
  	Debug.Log("Photon No Click");
  	this.GetComponent<PhotonView>().RPC("NoClick",PhotonTargets.All);
  }

  public void PhotonDropCardArea(){
    Debug.Log("Photon Card On Drop");
  }


  [PunRPC]
  public void DrawCard(){
    MultiplayerGame.GameManager.DrawCard();
  }

  [PunRPC]
  public void EndTurn(){
    if(MultiplayerGame.GameManager.sync == true){
      MultiplayerGame.GameManager.EndTurn();
    }
  }

  [PunRPC]
  public void YesClick(){
    MultiplayerGame.GameManager.YesClick();
  }

  [PunRPC]
  public void NoClick(){
    MultiplayerGame.GameManager.NoClick();
  }
  }
