using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviour {

	public GameObject PlayerPanel;
	public GameObject PlayerText;
	public GameObject StartButton;
	public GameObject JoinButton;
	public Logger NetworkLogger = new Logger();
	public string versionNumber = "0.1.0";

	private void Awake(){
		PhotonNetwork.ConnectUsingSettings(versionNumber);
		PhotonNetwork.automaticallySyncScene = true;
		Debug.Log("Connecting to server");
		NetworkLogger.info("Connecting to server");
	}

	private void OnConnectedToMaster(){
		Debug.Log("Conected");
		NetworkLogger.info("Conected");
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	private void OnJoinedLobby(){
		Debug.Log("Conected to lobby");
		NetworkLogger.info("Conected to lobby");
		if(PhotonNetwork.countOfRooms ==0){
			PhotonNetwork.CreateRoom("Game",new RoomOptions(){MaxPlayers = 4},null);
			Debug.Log("Created Game");
			NetworkLogger.info("Created Game");
			//Turn off join button because already joined
			JoinButton.SetActive(false);
		}
	}

	private void OnDisconnectedFromPhoton(){
		Debug.Log("Disconnected");
		NetworkLogger.info("Disconnected");
	}

	public void OnClickJoinRoom(){
		PhotonNetwork.JoinRoom("Game");
	}

	public void OnJoinedRoom(){
		PhotonPlayer[] players = PhotonNetwork.playerList;

		foreach (Transform child in PlayerPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}

		for(int i = 0 ; i < players.Length; i++){
			GameObject currText = Instantiate(PlayerText);
			currText.GetComponent<UnityEngine.UI.Text>().text = "Player ID: " + i ;
			currText.transform.SetParent(PlayerPanel.transform);
		}

		//Turn off join button because already joined
		JoinButton.SetActive(false);

		Debug.Log("Player Connected to Game");
		NetworkLogger.info("Player Connected to Game");

		if (PhotonNetwork.player.ID == 1) {
			StartButton.SetActive (true);
		} else {
			StartButton.SetActive (false);
		}
}

	public void OnPhotonPlayerConnected(){
		PhotonPlayer[] players = PhotonNetwork.playerList;

		foreach (Transform child in PlayerPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}

		for(int i = 0 ; i < players.Length; i++){
			GameObject currText = Instantiate(PlayerText);
			currText.GetComponent<UnityEngine.UI.Text>().text = "Player ID: " + i ;
			currText.transform.SetParent(PlayerPanel.transform);
		}
		if(players.Length > 4){PhotonNetwork.LoadLevel(2);}
	}

  public void StartGame(){
		this.GetComponent<PhotonView>().RPC("loadLevel",PhotonTargets.All);
  }

	[PunRPC] public void loadLevel(){
		PhotonNetwork.LoadLevel(2);
	}
}
