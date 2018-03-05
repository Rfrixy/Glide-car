using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTilt : MonoBehaviour {
    public float tiltMultiplier = 20f;
    public float turnSpeed = 8f;
    [Range(-1f,1f)]
    public float DummyValue = 0f;
	// Update is called once per frame
	void FixedUpdate () {
        float requiredRotation = Input.acceleration.x * tiltMultiplier + DummyValue * tiltMultiplier;
        float lerpedRotation = Mathf.Lerp(transform.rotation.z, requiredRotation, 0.5f);
        //Debug.Log("lerpa " + lerpedRotation);
        transform.localRotation = Quaternion.Euler(6f, 0f, lerpedRotation);
            
	}

    public void DisableThis()
    {
        this.enabled = false;
    }

}
