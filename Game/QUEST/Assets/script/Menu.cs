using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	public GameObject MenuPanel;
	public GameObject HotSeatGameMode;

	public void HotSeatMode(){
		MenuPanel.SetActive(false);
		HotSeatGameMode.SetActive(true);
	}

	public void MultiplayerMode(){
		MenuPanel.SetActive(false);
		//SceneManager.LoadScene("MultiplayerLobby",LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {

	}
}
