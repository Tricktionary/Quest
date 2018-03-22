using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPrompt : MonoBehaviour {

	//private static MultiplayerPrompt _instance;

	// Prompts.
	public GameObject promptObj;
	public GameObject promptTxt;
	public GameObject gameStatus;

	// Text snippets.
	public static string sponsorText = "Do you want to Sponsor this Quest?";
	public static string questText = "Do you want to join the Quest?";
	public static string tournamentText = "Do you Want To join this Tournament?";

	//public static MultiplayerPrompt PromptManager { get { return _instance; } }

	private void Awake(){

	}

	public MultiplayerPrompt(GameObject promptObj, GameObject promptTxt, GameObject gameStatus){
		this.promptObj = promptObj;
		this.promptTxt = promptTxt;
		this.gameStatus = gameStatus;

	}

	// Spawn a prompt.
	public void promptMessage(string messageType){
		promptObj.SetActive(true);
		if(messageType == "sponsor"){
			MultiplayerGame.GameManager.logger.info("Displaying sponsor prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = sponsorText;
		} else if (messageType == "quest"){
			MultiplayerGame.GameManager.logger.info("Displaying join quest prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = questText;
		} else if (messageType == "tournament"){
			MultiplayerGame.GameManager.logger.info("Displaying join tournament prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = tournamentText;
		}
	}

	// The user responds yes to a prompt.
	public void promptYes(){
		promptObj.SetActive(false);
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == sponsorText){
			MultiplayerGame.GameManager._questBehaviour.sponsor(true);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == questText){
			MultiplayerGame.GameManager._questBehaviour.joinQuest(true);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == tournamentText){
			MultiplayerGame.GameManager._tournamentBehaviour.joinTournament(true);
		}
	}

	// The user responds no to a prompt.
	public void promptNo(){
		promptObj.SetActive(false);
		if (promptTxt.GetComponent<UnityEngine.UI.Text>().text == sponsorText) {
			MultiplayerGame.GameManager._questBehaviour.sponsor(false);
		} else if (promptTxt.GetComponent<UnityEngine.UI.Text>().text == questText) {
			MultiplayerGame.GameManager._questBehaviour.joinQuest(false);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == tournamentText){
			MultiplayerGame.GameManager._tournamentBehaviour.joinTournament(false);
		}
	}

	// User status prompt.
	public void statusPrompt(string message){
		gameStatus.GetComponent<UnityEngine.UI.Text>().text = message;
	}

	/*// Set draggable on an area.
	void setDraggable(GameObject area, bool drag){
		area.GetComponent<CardArea>().acceptObj = drag;
	}*/
}
