using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carFloater : MonoBehaviour {
    public float turnForce, forwardForce, upForce;
    public float hoverHeight;
    private float powerInput, turnInput, hBrakeInput, boostInput, tiltInput;
    public float maxUpForce = 10000f;
    public Transform[] wheels;
    private Rigidbody carRigidbody;
    public float dampForce;
    private float[] lastDist;
	// Use this for initialization
	void Start () {
        carRigidbody = GetComponent<Rigidbody>();
        lastDist = new float[4];
	}
	
	// Update is called once per frame
	void Update () {
        powerInput = 0;
        turnInput = 0;
        boostInput = 0;
        hBrakeInput = 0;
        if (Input.GetKey("w"))
            powerInput = 1;

        if (Input.GetKey("s"))
            powerInput = -1;

        if (Input.GetKey("d"))
            turnInput = 1;

        if (Input.GetKey("a"))
            turnInput = -1;

        if (Input.GetKey("."))
            boostInput = 1;

        if (Input.GetKey(","))
            hBrakeInput = 1;

        tiltInput = powerInput;
    }

    private void FixedUpdate()
    {


        for (int i = 0; i < wheels.Length; i++)
        {
            Ray ray = new Ray(wheels[i].transform.position, -transform.up);
            Debug.DrawRay(wheels[i].transform.position, -transform.up * hoverHeight);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, hoverHeight*2)) // hoverhiehgt is the max height of the raycast
            {

                float proportionalHeight = Mathf.Clamp((hoverHeight - hit.distance), -hoverHeight, hoverHeight) / hoverHeight;


                float _upForce = Mathf.Min(proportionalHeight * upForce, maxUpForce);

                Vector3 appliedHoverForce = transform.up * Mathf.Min(proportionalHeight * upForce, maxUpForce)
                -transform.up * dampForce * (hit.distance - lastDist[i])/Time.fixedDeltaTime;

                lastDist[i] = hit.distance;

                carRigidbody.AddForceAtPosition(appliedHoverForce, wheels[i].transform.position);
            }
        }
        


        //Vector3 SideForce = Vector3.Project(carRigidbody.velocity, wheel.wheelPos.transform.right) * sideForce; // old

        //if (hBrakeInput > 0)
        //    SideForce *= handBrakeSideForceFactor;

        //float skidAmount = SideForce.magnitude / sideForce;
        //if (sideForce > 0)
        //    carRigidbody.AddForce(-SideForce);





        carRigidbody.AddForce(transform.forward * powerInput * forwardForce);
        carRigidbody.AddRelativeTorque(0f, turnInput * turnForce, 0f);

    }
}
