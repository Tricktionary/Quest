using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
//https://www.youtube.com/watch?v=qGkkaNkq8co 1:06:55

public class MClient : MonoBehaviour {
	private const int MAX_CONNECTION = 4;

	private int port = 5701;

	private int hostId;
	private int webHostId;

	private int reliableChannel;
	private int unreliableChannel;

	private byte error;
	private float connectionTime;

	private int connectionId;
	private int clientId; //OUR CLIENT ID
	private bool isStarted = false;
	private bool isConnected = false;


	public GameObject playField;

	public List<OnlinePlayer> players = new List<OnlinePlayer> ();

	private string playerName;



	public void Connect()
	{
		string pName = GameObject.Find ("NameField").GetComponent<InputField> ().text;

		if (pName == "") {
			Debug.Log ("Enter A name");
			return;
		}

		playerName = pName;

		NetworkTransport.Init();
		ConnectionConfig cc = new ConnectionConfig ();

		reliableChannel = cc.AddChannel (QosType.Reliable);
		unreliableChannel = cc.AddChannel (QosType.Unreliable);

		HostTopology topo = new HostTopology (cc, MAX_CONNECTION);

		hostId = NetworkTransport.AddHost (topo,0);

		connectionId = NetworkTransport.Connect (hostId, "127.0.0.1", port, 0, out error);
		connectionTime = Time.time;


		isConnected  = true;

	}


	private void Update(){
		Application.runInBackground = true;
		if (!isConnected) {
			return;
		} else {
			int recHostId; 
			int connectionId; 
			int channelId; 
			byte[] recBuffer = new byte[1024]; 
			int bufferSize = 1024;
			int dataSize;
			byte error;
			NetworkEventType recData = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

			switch (recData) {

			case NetworkEventType.DataEvent:       //GAME LOOP
				string msg = Encoding.Unicode.GetString (recBuffer, 0, dataSize);
				Debug.Log ("Receiving-MSG :" + msg);
				string[] splitData = msg.Split ('|');

				switch(splitData[0])
				{
				case "ASKNAME":
					OnAskName(splitData);
					break;
				case "RECEIVEADVENTURECARD":	
					ReceivingAdventureCard (splitData);
					break;
				case "CNN":
					SpawnPlayer (splitData [1],int.Parse(splitData[2]));
					break;
				case "DC":
					break;
				default:
					Debug.Log("Invalid Message : " + msg);
					break;
				}
				break;
			}
		}
	}

	private void ReceivingAdventureCard(string[] data){
		//Debug.Log ("HERE");
		//Debug.Log(data[1]);
		string currCardJson = data[1];
		Card currCard;
		currCard = JsonUtility.FromJson<Card>(currCardJson);
		Debug.Log (currCard.ToString ());
	}
	private void OnAskName(string[] data){

		//Set out client ID
		clientId = int.Parse(data [1]);

		//Send our name to the server 
		Send("NAMEIS|" + playerName,reliableChannel);

		//create all the other players
		for (int i = 2; i < data.Length - 1; i++) {
			string[] d = data [i].Split ('%');
			SpawnPlayer (d [0], int.Parse (d [1]));
		}

		//Send("MAKEHAND|",reliableChannel);
		for (int i = 0; i < 12; i++) {
			Send ("GETADVENTURECARD", reliableChannel);
		}
	}

	private void SpawnPlayer(string playerName, int cnnId)
	{
		//GameObject go = Instantiate (playerObj) as GameObject;

		if (cnnId == clientId) {
			//remove canvas
			GameObject.Find("Canvas").SetActive(false);
			playField.SetActive (true);
			isStarted = true;
		}

		OnlinePlayer p = new OnlinePlayer (playerName, cnnId);
		players.Add (p);
	}

	private void Send(string message,int channelId)
	{
		Debug.Log ("Sending : " + message);
		byte[] msg = Encoding.Unicode.GetBytes (message); 
		NetworkTransport.Send (hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);

	}

}
