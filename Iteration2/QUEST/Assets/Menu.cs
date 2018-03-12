using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	public GameObject MenuPanel;
	public GameObject Network;
	public GameObject HotSeatGameMode;

	public void HotSeatMode(){
		MenuPanel.SetActive(false);
		HotSeatGameMode.SetActive(true);
	}

	public void MultiplayerMode(){
		MenuPanel.SetActive(false);
		Network.SetActive(true);
	}

	// Use this for initialization
	void Start () {

	}
}
