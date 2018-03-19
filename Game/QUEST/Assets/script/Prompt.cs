using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour {

	private static Prompt _instance;

	// Prompts.
	public GameObject yesObj;
	public GameObject noObj;
	public GameObject promptObj;
	public GameObject promptTxt;
	public GameObject gameStatus;
	//public GameObject noButton;
	//public GameObject yesButton;

	// Text snippets.
	public static string sponsorText = "Do you want to Sponsor this Quest?";
	public static string questText = "Do you want to join the Quest?";
	public static string tournamentText = "Do you Want To join this Tournament?";
	//public static string testText = "How much would you like to bid?";

	public static Prompt PromptManager { get { return _instance; } }

	private void Awake(){
		if (!_instance) {
			_instance = this;
		}
	}

	// Spawn a prompt.
	public void promptMessage(string messageType){
		promptObj.SetActive(true);
		if(messageType == "sponsor"){
			Game.GameManager.logger.info("Displaying sponsor prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = sponsorText;
		} else if (messageType == "quest"){
			Game.GameManager.logger.info("Displaying join quest prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = questText;
		} else if (messageType == "tournament"){
			Game.GameManager.logger.info("Displaying join tournament prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = tournamentText;
		} 
		/*
		else if (messageType == "test"){ //gets called in endturn if card is a test card
			Game.GameManager.logger.info("Displaying Test prompt!");
			promptTxt.GetComponent<UnityEngine.UI.Text>().text = testText;
			

			//make yes and no into 1-13
		}
		*/

	}

	/*
	public void getInput(string bidNum){
		Debug.Log("bidNum value: " + bidNum);
		if(bidNum == "3"){
			//yesObj.SetActive(false);
			//return 1;
		}
		else{
			//return -1;
		}
		//int x = Int32.Parse(bidNum);

	}
	*/

	// The user responds yes to a prompt.
	public void promptYes(){
		promptObj.SetActive(false);
		if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == sponsorText){
			Game.GameManager._questBehaviour.sponsor(true);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == questText){
			Game.GameManager._questBehaviour.joinQuest(true);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == tournamentText){
			Game.GameManager._tournamentBehaviour.joinTournament(true);
		}
	}

	// The user responds no to a prompt.
	public void promptNo(){
		promptObj.SetActive(false);
		if (promptTxt.GetComponent<UnityEngine.UI.Text>().text == sponsorText) {
			Game.GameManager._questBehaviour.sponsor(false);
		} else if (promptTxt.GetComponent<UnityEngine.UI.Text>().text == questText) {
			Game.GameManager._questBehaviour.joinQuest(false);
		} else if(promptTxt.GetComponent<UnityEngine.UI.Text>().text == tournamentText){
			Game.GameManager._tournamentBehaviour.joinTournament(false);
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
