using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
	public CheckPoint next;
	public CheckPoint prev;
	public Transform SpawnPoint;
	private MessagesManager MM;
	public bool hasBeenReached=false;
	// Use this for initialization
	void Start () {
		hasBeenReached=false;
		if (prev == null)
			hasBeenReached = true;

		MM = MessagesManager.SharedInstance;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col){

		if (hasBeenReached)
			return;
		if (col.isTrigger)
			return;
		if (col.gameObject.tag == "Car") {
			if (prev.hasBeenReached) {
				hasBeenReached = true;

				if (next == null)
					GameManager.SharedInstance.LevelComplete ();
				else				
					MM.DisplayMessage ("CheckPoint");

			}else
				MM.DisplayMessage ("Wrong CheckPoint");


		}

	}
}
