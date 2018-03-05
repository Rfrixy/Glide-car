using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour {

	void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Car")
        {
			GameManager.SharedInstance.Retry ();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Car")
        {
            GameManager.SharedInstance.Retry();
        }
    }
}
