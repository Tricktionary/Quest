using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour {

	private static Blocker _instance;

	//Blocker Panel
	public GameObject Panel;

	//Blocker Text
	public GameObject PlayerID;

	//Current Story Card
	public GameObject DisplayCard;

	//Card Holder
	public GameObject CardHolder;

	public static Blocker BlockManager { get { return _instance; } }

	// Use this for initialization
	void Start () {
		if (!_instance) {
			_instance = this;
		}
		DontDestroyOnLoad(this);
	}

	void SetBlocker(int id, Card card){

		Panel.SetActive(true);

	}
	// Update is called once per frame
	void Update () {

	}
}
