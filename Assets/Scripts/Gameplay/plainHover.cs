using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
[System.Serializable]
public class Wheel{
	public Transform wheelPos;
	public GameObject wheelMesh_GO;
	public float dist;
	//public float upForce;
	public WheelEffectMaker WEM;
	public bool grounded;
}

public class plainHover : MonoBehaviour {

    #region variables 
    public float extraGravityForce = 5f;
    private float egf;
    public bool phoneControls=false;
    public bool joystickControls = true;
    public float arialCamTurn = 0.15f;
	public float groundedCamTurn = 10f;
	public float maxSpeed = 10f;
	public float turnSpeedThreshold=2f;
	public float sideSkidThreshold = 2f;
    public float stopSkiddingThreshold = 2f;

    [Range(0,1)]
	public float driftingSkidMultiplier = 0.125f;
	public float speed = 90f;
	public float turnSpeed = 5f;
    public float turnOffset = 1;
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;
	public float tiltForce = 200f;
	public float sideForceRecoveryRte = 1f;
	private Rigidbody carRigidbody;
	public float maxUpForce = 100f;
    public float maxDampForce = -200f;

    public float sideForce=40f;
	private float startingSideForce;
	public float dampForce=2000f;
	public float boostForce = 500f;
	public float aerialBoostReductionMultiplier = 3f;
	public float handBrakeStopForce = 500f;
	public float handBrakeTurnFactor=2f;
	public float handBrakeSideForceFactor=0.5f;
	[Tooltip("Keep this less than skidthreshold")]
	public float handBrakeSkidThresholdReduction = 2f;
    [Range(0, 1)]
    public float inputSmoothing = 0.6f;
    public float turnInput;
	public float tiltInput;
	public float inputSensitivity=1.5f;
	public float powerInput;
	public float boostInput;
	public float hBrakeInput;
    private float tiltCalibrated = 0.5f;
    public float WheelMeshOffset;
	public Wheel[] FrontWheels;
	public Wheel[] BackWheels;
	private List<Wheel> AllWheels;
	public ParticleSystem[] BoostTrails;
	public bool grounded;
	public Vector3 localVelocity;
	private int AcceleratorInput=0,BrakeInput=0;
    private bool isSkidding;
    public newCam NC;
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    public GameObject AssistiveWheels;

    private float grounded_y_Angle;
    #endregion



    void Awake () 
	{
		carRigidbody = GetComponent <Rigidbody>();
		carRigidbody.centerOfMass =m_CentreOfMassOffset;
		startingSideForce = sideForce;
        egf = extraGravityForce;
#if UNITY_EDITOR
        phoneControls = false;
#endif
    }

    void Start(){
		BoostTrails [0].Stop ();
		BoostTrails [1].Stop ();
		AllWheels=new List<Wheel>();
		AllWheels.Add (FrontWheels [0]);
		AllWheels.Add (FrontWheels [1]);
		AllWheels.Add (BackWheels [0]);
		AllWheels.Add (BackWheels [1]);

	}

	void Update () 
	{

        #region Controls

        //		COMP Controls begin
        if ((!phoneControls)&&(!joystickControls)) {
			powerInput = 0;
			turnInput = 0;
			boostInput = 0;
			hBrakeInput = 0;
			if (Input.GetKey ("w"))
				powerInput = 1;

			if (Input.GetKey ("s"))
				powerInput = -1;
		
			if (Input.GetKey ("d"))
				turnInput = 1;

			if (Input.GetKey ("a"))
				turnInput = -1;

			if (Input.GetKey ("."))
				boostInput = 1;

			if (Input.GetKey (","))
				hBrakeInput = 1;

			tiltInput = powerInput/2;
		} else if(phoneControls){

			powerInput = AcceleratorInput - BrakeInput;


            //tiltInput = Mathf.SmoothStep(tiltInput, Mathf.Clamp((Input.acceleration.y + tiltCalibrated) * inputSensitivity, -1, 1), inputSmoothing); // phone wale
            tiltInput = Mathf.SmoothStep(tiltInput, 
                Mathf.Clamp(( Input.acceleration.y - tiltCalibrated  ), -0.35f, 0.35f), inputSmoothing)/0.7f; // phone wale


            turnInput = Mathf.SmoothStep(turnInput, Mathf.Clamp(Input.acceleration.x * inputSensitivity, -1, 1), inputSmoothing);

//#if UNITY_EDITOR
//            boostInput = 0;
//            hBrakeInput = 0;
//            if (Input.GetKey("."))
//                boostInput = 1;

//            if (Input.GetKey(","))
//                hBrakeInput = 1;
//#endif

        }
        else if (joystickControls)
        {
            Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
            powerInput = moveVec.y;
            tiltInput = moveVec.y;
            turnInput = moveVec.x;

#if UNITY_EDITOR
            boostInput = 0;
            hBrakeInput = 0;
            if (Input.GetKey("."))
                boostInput = 1;

            if (Input.GetKey(","))
                hBrakeInput = 1;
#endif
        }

        #endregion


        //turnInput *= Mathf.Abs(turnInput);
        //turnInput = Mathf.Clamp(turnInput, -0.7f, 0.7f) / 0.7f;
    }





	void FixedUpdate()
	{

		localVelocity = transform.InverseTransformDirection (carRigidbody.velocity);

		if (localVelocity.z > maxSpeed)
			powerInput = (powerInput < 0) ? powerInput : 0;

        if (localVelocity.z < -maxSpeed/4)
            powerInput = (powerInput > 0) ? powerInput : 0;



        //backWheels
        for (int i=0;i<2;i++){
			Ray ray = new Ray (BackWheels[i].wheelPos.transform.position, -transform.up);
			RaycastHit hit;
            //Debug.DrawRay(BackWheels[i].wheelPos.transform.position, -transform.up * hoverHeight);

            if (Physics.Raycast (ray, out hit, hoverHeight)) { // hoverhiehgt is the max height of the raycast
				BackWheels [i].grounded = true;
				WheelJob (BackWheels [i], hit);
			} else {
				BackWheels [i].grounded = false;
				BackWheels [i].WEM.EndSkidTrail ();
			}
		}

        if ((BackWheels[0].grounded) && (BackWheels[1].grounded))
        {
            //if ((FrontWheels[0].grounded) && (FrontWheels[1].grounded))
                SetGrounded(true);
            //else
                //SetGrounded(false);
        }
        else
            SetGrounded(false);

		// FrontWheels
		for(int i=0;i<2;i++){
			Ray ray = new Ray (FrontWheels[i].wheelPos.transform.position, -transform.up);
			Debug.DrawRay (FrontWheels[i].wheelPos.transform.position, -transform.up*hoverHeight);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, hoverHeight)) // hoverhiehgt is the max height of the raycast
			{

				WheelJob (FrontWheels [i], hit,true);
                FrontWheels[i].grounded = true;

                //Debug.Log("hover " + hit.distance);

            }
            else {
				FrontWheels [i].grounded = false;
				FrontWheels [i].WEM.EndSkidTrail ();
			}

		}






		if (grounded)
        {
            GroundedCarControl();

            Vector3 localAngularVelocity = transform.InverseTransformDirection(carRigidbody.angularVelocity);
            localAngularVelocity.y *= 0.5f;
            carRigidbody.angularVelocity = transform.TransformDirection(localAngularVelocity);
        }
        else
			AerialCarControl ();


	}

    #region Aerial & Grounded Car Control

    void GroundedCarControl(){
		float turnMultiplier=1;
		if (localVelocity.z < 0)    
			turnMultiplier *= -1;

		if (Mathf.Abs (localVelocity.z) < turnSpeedThreshold)
			turnMultiplier = localVelocity.z/turnSpeedThreshold;


		carRigidbody.AddForce (transform.forward * powerInput * speed);




        //hBraking
        if (hBrakeInput > 0) {
			turnInput *= handBrakeTurnFactor;
			Vector3 hBrakeForce =  (transform.forward * handBrakeStopForce * -Mathf.Clamp (localVelocity.z, -1, 1));
			carRigidbody.AddForce (hBrakeForce);
		}

        //turning

        //carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed * turnMultiplier, 0f); // Physics Rotation

        transform.RotateAround(transform.position-Vector3.forward*turnOffset,transform.up, Time.deltaTime * turnSpeed * turnInput* turnMultiplier); // direct rotation

        //boosting
        if (boostInput > 0) {
			carRigidbody.AddForce (transform.forward * boostInput * boostForce);
			EmitBoost ();
		} else
			StopBoost ();

	}



    void AerialCarControl(){


		if (hBrakeInput > 0) {
			Vector3 hBrakeForce =  (transform.forward * handBrakeStopForce/8 * -Mathf.Clamp (localVelocity.z, -1, 1));
			carRigidbody.AddForce (hBrakeForce);
		}

        carRigidbody.AddForce(-transform.up * extraGravityForce,ForceMode.Acceleration);


        carRigidbody.angularVelocity *= 0.9f;


        if (boostInput > 0) {
			carRigidbody.AddForce (transform.forward * boostInput * boostForce/aerialBoostReductionMultiplier);
			EmitBoost ();
		} else
			StopBoost ();

		//if ((boostInput > 0) || (hBrakeInput > 0) || joystickControls) {


            // ------------ TORQUE ROTATION ---------------------------- //
            //transform.RotateAround (transform.position,transform.forward,-turnInput); // indefinite -- side tilt

   //         float currentDip = transform.localEulerAngles.x;
   //         currentDip = (currentDip > 180) ? currentDip - 360 : currentDip;

   //         if(currentDip*tiltInput<35)
			//carRigidbody.AddRelativeTorque (tiltInput*tiltForce, 0f, 0f); //forward - backward dip

   //         float current_yRotation = transform.rotation.eulerAngles.y;
   //         current_yRotation = (current_yRotation > 180) ? current_yRotation - 360 : current_yRotation;


            //if((current_yRotation < grounded_y_Angle+25)&&(turnInput>0) || (current_yRotation > grounded_y_Angle - 25)&&(turnInput<0))

            //    carRigidbody.AddTorque(0f, turnInput * tiltForce, 0f); // turning in air


            // -------- DIRECT ROTATION --------------------------- // 
            transform.Rotate(Vector3.up * Time.deltaTime * tiltForce * turnInput*1.5f, Space.World); // direct rotation
            transform.Rotate(Vector3.right * Time.deltaTime * tiltForce * tiltInput, Space.Self); // direct rotation

        //}

    }

    #endregion


    #region boostParticles control
    void EmitBoost(){
		if (BoostTrails [0].isStopped) {
			BoostTrails [0].Play ();
			BoostTrails [1].Play ();
		}
	}

	void StopBoost(){
		if (BoostTrails [0].isPlaying) {
			BoostTrails [0].Stop ();
			BoostTrails [1].Stop ();
		}

	}
    #endregion

    void WheelJob(Wheel wheel, RaycastHit hit,bool turnWheelMesh=false){


        float proportionalHeight = Mathf.Clamp((hoverHeight/2 - hit.distance),0,hoverHeight)/hoverHeight;

        //if (proportionalHeight * hoverForce > maxUpForce)
        //    hoverForce = 100f;


        //if (hit.distance - wheel.dist * dampForce > maxDampForce)
        //    dampForce = 0f;

        float _upForce = Mathf.Min(proportionalHeight * hoverForce, maxUpForce);
        Vector3 appliedHoverForce = transform.up * Mathf.Min(proportionalHeight * hoverForce, maxUpForce)
                                      - transform.up * Mathf.Min(dampForce * (hit.distance - wheel.dist),maxDampForce);

        //Debug.Log("force " + appliedHoverForce.y);


        if (!grounded)
        {

            //return;
        }

        Vector3 SideForce = Vector3.Project (carRigidbody.velocity, wheel.wheelPos.transform.right) * sideForce; // old

		if (hBrakeInput > 0)
			SideForce *= handBrakeSideForceFactor;
		
		float skidAmount = SideForce.magnitude/sideForce;

		if(sideForce>0)
			carRigidbody.AddForce	(-SideForce); 

		if(isSkidding)
        {
            if ((skidAmount > stopSkiddingThreshold - hBrakeInput) && (grounded))
            {
                carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed * driftingSkidMultiplier, 0f);

                wheel.WEM.EmitTyreSmoke();

            }
            else
            {
                isSkidding = false;
                wheel.WEM.EndSkidTrail();

            }


        }
        else if(!isSkidding)
		    if ((skidAmount > sideSkidThreshold-hBrakeInput*handBrakeSkidThresholdReduction)&&(grounded))
            {
			carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed * driftingSkidMultiplier, 0f);
                isSkidding = true;
			wheel.WEM.EmitTyreSmoke ();
		    } else {
			    wheel.WEM.EndSkidTrail ();
		    }


		carRigidbody.AddForceAtPosition(appliedHoverForce,wheel.wheelPos.transform.position);

		wheel.dist = hit.distance;

		if (turnWheelMesh) {
			wheel.wheelMesh_GO.transform.localRotation = Quaternion.AngleAxis (turnInput * 25, Vector3.up); 
		}


	}





    #region lift off and Land
    // LIFT OFF AND LAND -----------------------------------------//
    void SetGrounded(bool grounded_bool){
		if (grounded_bool != grounded) {
			if (grounded) {
				grounded = false;
				OnLiftOff ();
			} else {

				grounded = true;
				OnLand ();
			}
		}
	}

	void OnLiftOff(){
        grounded_y_Angle = transform.rotation.eulerAngles.y;

        grounded_y_Angle = (grounded_y_Angle > 180) ? grounded_y_Angle - 360 : grounded_y_Angle;

        NC.m_TurnSpeed = arialCamTurn;
        NC.grounded = false;
        //extraGravityForce = egf;
        //AssistiveWheels.SetActive(true);
        //AC.m_FollowTilt = false;

        if (phoneControls)
        {
            tiltCalibrated = Mathf.Clamp(Input.acceleration.y, -0.65f, -0.35f);
            Debug.Log("New calibrated value :" + tiltCalibrated);
        }
        StartCoroutine(DelayedOnLift ());
    }
	private IEnumerator DelayedOnLift(){
		yield return new WaitForSeconds (1f);
		if (!grounded) {
			NC.grounded=false;
//			AC.m_FollowTilt = true;

		}

	}
	void OnLand(){
        StartCoroutine(RestoreGroundedCam());
        //AC.m_TurnSpeed = groundedCamTurn;
        //AC.m_FollowTilt = true;
        NC.grounded = true; 
        //AssistiveWheels.SetActive(false);

        //extraGravityForce = egf*2;
        StartCoroutine(RestoreSideForce());
    }
    


	private IEnumerator RestoreSideForce(){
        sideForce = Mathf.Clamp(sideForce - Mathf.Abs(localVelocity.x+localVelocity.z),sideForce/4,sideForce);

		while ((grounded) && (sideForce < startingSideForce)) {
			sideForce += sideForceRecoveryRte * Time.deltaTime;
			yield return null;  

		}

	}

    private IEnumerator RestoreGroundedCam()
    {

        while ((grounded) && (NC.m_TurnSpeed < groundedCamTurn))
        {
            NC.m_TurnSpeed += 10 * Time.deltaTime;
            yield return null;

        }

    }

    #endregion


    #region inputCode
    // INPUT CODE ------------------------------------------------- //
    public void AcceleratorPressed(){
		AcceleratorInput = 1;
	}
	public void AcceleratorReleased(){
		AcceleratorInput = 0;
	}

	public void BrakePressed(){
		BrakeInput = 1;
	}
	public void BrakeReleased(){
		BrakeInput = 0;
	}


	public void BoostPressed(){
		boostInput = 1;
	}
	public void BoostReleased(){
		boostInput = 0;
	}

	public void HBrakePressed(){
		hBrakeInput = 1;
	}
	public void HBrakeReleased(){
		hBrakeInput = 0;
	}
#endregion
}

