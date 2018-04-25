using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControlScript : MonoBehaviour {

    private Rigidbody body;

	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	void Update () {
        body.AddForce(body.transform.forward * Time.deltaTime * 1000.0f * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
        body.AddTorque(new Vector3(0.0f, Time.deltaTime * 100000000.0f * Input.GetAxis("Horizontal"), 0.0f));
	}

}