using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour {
    public Rigidbody rb;
    public float exploForce=50f, explosionRadius=0.5f;
    public float velocityMultiplier=40f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collision collision)
    {   
            Vector3 startDir = collision.contacts[0].point;
            Debug.Log(transform.position.y + " || " + startDir.y);
            float vel = collision.impulse.magnitude * velocityMultiplier;
            rb.AddExplosionForce(exploForce + vel, startDir, explosionRadius);
            Debug.Log("colided: " + exploForce + vel);
        
    }
}
