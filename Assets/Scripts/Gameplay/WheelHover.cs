using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelHover : MonoBehaviour {
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;
	public float maxUpForce = 100f;
	public Rigidbody carRigidbody;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, hoverHeight)) // hoverhiehgt is the max height of the raycast
		{
			Debug.Log ("Dist: " + hit.distance);
			//			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			float proportionalHeight = Mathf.Clamp (hoverHeight / hit.distance+0.01f,0f,maxUpForce);
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
			//			carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
			carRigidbody.AddForce(appliedHoverForce);

		}

	}


}
