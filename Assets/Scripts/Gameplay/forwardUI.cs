using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class forwardUI : MonoBehaviour {
	public plainHover PH;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((PH.grounded)) {
            transform.localPosition = new Vector3(0f, -3000f, 0f);

        }
        else
        {
            transform.localPosition = new Vector3(0f, -30f + PH.tiltInput * -200, 0f);
        }
    }
}
