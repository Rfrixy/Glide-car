using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEffectMaker : MonoBehaviour {
	public Transform SkidTrailPrefab;
	public static Transform skidTrailsDetachedParent;
	private Transform m_SkidTrail;
	public bool skidding { get; set; }

	// Use this for initialization
	void Start () {

		if (skidTrailsDetachedParent == null)
		{
			skidTrailsDetachedParent = new GameObject("Skid Trails - Detached").transform;
		}
	}
	

	public void EmitTyreSmoke()
	{
//		skidParticles.transform.position = transform.position - transform.up*m_WheelCollider.radius;
//		skidParticles.Emit(1);
		if (!skidding)
		{
			StartCoroutine(StartSkidTrail());
		}
	}



	public IEnumerator StartSkidTrail()
	{
		skidding = true;
		m_SkidTrail = Instantiate(SkidTrailPrefab,transform);
		//while (m_SkidTrail == null)
		//{
		//	yield return null;
		//}
		m_SkidTrail.localPosition = -transform.up*0.15f;
        m_SkidTrail.GetComponent<TrailRenderer>().Clear();// = true;
        m_SkidTrail.gameObject.SetActive(true);
        yield return null;
	}


	public void EndSkidTrail()
	{
		if (!skidding)
		{
			return;
		}
		skidding = false;
		m_SkidTrail.parent = skidTrailsDetachedParent;
		Destroy(m_SkidTrail.gameObject, 10);
	}
}

