using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCam : MonoBehaviour {
    public Transform carTransform;
    public Transform camPoint;
    public Rigidbody m_CarRigidBody;
    public float m_MoveSpeed=50f;
    public float m_TurnSpeed = 10f;
    public bool grounded;
    public float yVelocity;
    public float verticalVelocity_pitch_multiplier=0.01f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //translating 
        transform.position = Vector3.Lerp(transform.position, carTransform.position, Time.fixedDeltaTime * m_MoveSpeed);





        //rotating

        if (grounded)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, 

                Quaternion.LookRotation(carTransform.forward,carTransform.transform.up),
                m_TurnSpeed * Time.fixedDeltaTime);


        }
        else
        {
            Vector3 aerialForward = camPoint.position - carTransform.position;

            Vector3 forwardVector= Vector3.ProjectOnPlane(aerialForward, Vector3.up).normalized;
            //Vector3 upDirection = Quaternion.AngleAxis(Vector3.right)

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(forwardVector - Vector3.up*m_CarRigidBody.velocity.y*verticalVelocity_pitch_multiplier, Vector3.up),
                m_TurnSpeed * Time.fixedDeltaTime);


        }


        //transform.LookAt(target);
		
	}
}
