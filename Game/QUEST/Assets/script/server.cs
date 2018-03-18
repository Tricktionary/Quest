using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerClient {
	public int connectionId;
	public string playerName;
}

public class Server : MonoBehaviour{
    private const int MAX_CONNECTION = 4;

    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted;

    private byte error;

	private Deck adventureDeck;
	private Deck storyDeck;

	private List<ServerClient> clients = new List<ServerClient>();

    private void Start()
    {
		Application.runInBackground = true;
      	NetworkTransport.Init();
		ConnectionConfig cc = new ConnectionConfig ();

		reliableChannel = cc.AddChannel (QosType.Reliable);
		unreliableChannel = cc.AddChannel (QosType.Unreliable);

		HostTopology topo = new HostTopology (cc, MAX_CONNECTION);
		hostId = NetworkTransport.AddHost (topo, port, null);
		  
		webHostId = NetworkTransport.AddWebsocketHost (topo, port, null);
		  
		adventureDeck= new Deck("Adventure");
		storyDeck = new Deck("Story");
		isStarted = true;

    }

    private void Update(){

		if (!isStarted) {
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

				case NetworkEventType.ConnectEvent:    //Someone has connected 
					Debug.Log ("Player " + connectionId + "has connected"); 
					OnConnection (connectionId);
					break;
				
				case NetworkEventType.DataEvent:       //GAME LOOP!!
					string msg = Encoding.Unicode.GetString(recBuffer,0,dataSize);
					Debug.Log("Receiving From " + connectionId + ":" + msg);  
					string[] splitData = msg.Split ('|');
					switch(splitData[0])
					{
						case "NAMEIS":
							OnNameIs(connectionId,splitData[1]);
							break;
						default:
							Debug.Log("Invalid Message : " + msg);
							break;
					}
					break;
				
				case NetworkEventType.DisconnectEvent: //Disconnect
					Debug.Log("Player " + connectionId + "has disconnected");  
					break;
				}
		}
    }

	private void OnNameIs(int cnnId, string playerName){
		//Link name to connection ID
		clients.Find(x => x.connectionId == cnnId).playerName = playerName;

		//Tell everyon that a new player has connected
		Send("CNN|" +playerName +'|' + cnnId,reliableChannel,clients);
	}
	private void OnConnection(int cnnId)
	{
		//Add too a list
		ServerClient c = new ServerClient();
		c.connectionId = cnnId;
		c.playerName = "TEMP";

		clients.Add (c);
		//Notify them of their Id
		//Request name and send the name of all the other players
		string msg = "ASKNAME|"+ cnnId +"|";

		foreach (ServerClient sc in clients) {
			msg += sc.playerName + "%" + sc.connectionId + "|";
		}

		msg = msg.Trim ('|');

		 
		//ASKNAME|3|DAVE%1|MICHAEL%2|TEMP%3
		Send(msg,reliableChannel,cnnId);
		initPlayerCards (cnnId);

		 
	}

	private void initPlayerCards(int cnnId){

		List<Card> newPlayerHand = new List<Card> ();

		for (int i = 0; i < 12; i++) {
			newPlayerHand.Add (adventureDeck.Draw());
		}

		string msg = "NEWHAND|";
		for (int i = 0; i < newPlayerHand.Count; i++) {
			msg = msg + JsonUtility.ToJson (newPlayerHand[i]) + "|";
		}

		Debug.Log (msg);
		Send (msg, reliableChannel, cnnId);
	}

	private void Send(string message, int channelId, int cnnId)
	{
		List<ServerClient> c = new List<ServerClient> ();
		c.Add (clients.Find (x => x.connectionId == cnnId));
		Send (message, channelId, c);
	}

	private void Send(string message,int channelId, List<ServerClient> c)
	{
		Debug.Log ("Sending : " + message);
		byte[] msg = Encoding.Unicode.GetBytes (message);
		foreach (ServerClient sc in c) {
			NetworkTransport.Send (hostId,sc.connectionId,channelId,msg,message.Length * sizeof(char), out error);
		}
	}


}
