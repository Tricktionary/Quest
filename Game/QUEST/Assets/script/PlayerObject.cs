using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour {
  Player thisPlayer;
  void Update(){

  }
  void Awake(){
    //Debug.Log("Player Load");
    int id = NetworkGameManager.NetworkManager.GetCurrentNewId();
    //Debug.Log(id);
    thisPlayer = new Player(id);
    thisPlayer.hand = NetworkGameManager.NetworkManager.InitHand();
    NetworkGameManager.NetworkManager._players.Add(thisPlayer);
  }
}
