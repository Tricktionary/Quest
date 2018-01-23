using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame(){ //Loads The "PlayField"
		SceneManager.LoadScene("playField");

	}
}
