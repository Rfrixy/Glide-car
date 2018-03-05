using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnUI : MonoBehaviour {
	
	public plainHover PH;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		transform.localEulerAngles =  (Vector3.forward*-PH.turnInput * 55);
	}
}
