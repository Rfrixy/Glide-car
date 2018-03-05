using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceAdd : MonoBehaviour {
    public Vector3 force;
    public Vector3 SpawnPoint;
    // Use this for initialization
    private Quaternion qt;
    private void Awake()
    {
        qt = transform.rotation;
    }
    void OnEnable () {
        gameObject.transform.localPosition = SpawnPoint;
        transform.rotation = qt;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}
	
	
}
