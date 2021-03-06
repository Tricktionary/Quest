﻿using System.Collections;
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

	public void Host(){
		MenuPanel.SetActive(false);
		SceneManager.LoadScene("Server",LoadSceneMode.Single);
	}

	public void PhotonMultiplayer(){
		MenuPanel.SetActive(false);
		SceneManager.LoadScene("PhotonMultiplayer",LoadSceneMode.Single);
	}

	public void Join(){
		MenuPanel.SetActive(false);
		SceneManager.LoadScene("Client",LoadSceneMode.Single);
	}

	public void MultiplayerLobby(){
		MenuPanel.SetActive(false);
		SceneManager.LoadScene("MultiplayerLobby",LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {

	}
}
