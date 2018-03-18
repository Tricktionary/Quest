using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//https://www.youtube.com/watch?v=qGkkaNkq8co 19:52
public class Client : MonoBehaviour {
  	private const int MAX_CONNECTION = 4;

  	private int port = 5701;

  	private int hostId;
  	private int webHostId;

  	private int reliableChannel;
  	private int unreliableChannel;

 	private byte error;
	private float connectionTime;

	private int connectionId;

	private bool isStarted = false;
	private bool isConnected = false;

	public void Connect()
	{
		NetworkTransport.Init();
		ConnectionConfig cc = new ConnectionConfig ();

		reliableChannel = cc.AddChannel (QosType.Reliable);
		unreliableChannel = cc.AddChannel (QosType.Unreliable);

		HostTopology topo = new HostTopology (cc, MAX_CONNECTION);

		hostId = NetworkTransport.AddHost (topo,0);

		connectionId = NetworkTransport.Connect( hostId,"127.0.0.1",port,0, out error);

		connectionTime = Time.time;

		isConnected  = true;

	}

	private void Update(){
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
			case NetworkEventType.Nothing:         //1 Nothing is received 
				break;
			case NetworkEventType.ConnectEvent:    //2 Someone has connected 
				break;
			case NetworkEventType.DataEvent:       //3 GAME LOOP
				break;
			case NetworkEventType.DisconnectEvent: //4 Disconnect
				break;
			}
		}
	}
}
