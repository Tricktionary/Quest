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

	private List<ServerClient> clients = new List<ServerClient>();

    private void Start()
    {
      	  NetworkTransport.Init();
		  ConnectionConfig cc = new ConnectionConfig ();

		  reliableChannel = cc.AddChannel (QosType.Reliable);
		  unreliableChannel = cc.AddChannel (QosType.Unreliable);

		  HostTopology topo = new HostTopology (cc, MAX_CONNECTION);
		  hostId = NetworkTransport.AddHost (topo, port, null);
		  webHostId = NetworkTransport.AddWebsocketHost (topo, port, null);
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
			case NetworkEventType.Nothing:         //1 Nothing is received
		     	break;
			case NetworkEventType.ConnectEvent:    //2 Someone has connected 
				Debug.Log ("Player " + connectionId + "has connected"); 
				OnConnection (connectionId);
				break;
			case NetworkEventType.DataEvent:       //3 GAME LOOP!!
				string msg = Encoding.Unicode.GetString(recBuffer,0,dataSize);
				Debug.Log("Player " + connectionId + "has sent :" + msg);  
				break;
			case NetworkEventType.DisconnectEvent: //4 Disconnect
				Debug.Log("Player " + connectionId + "has disconnected");  
				break;
			}
		}
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

	}

}
