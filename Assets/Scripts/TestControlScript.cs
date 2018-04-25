using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControlScript : MonoBehaviour {

    private Rigidbody body;
    public GameObject prefab;

	void Start () {
        body = GetComponent<Rigidbody>();
	}

    bool wasSpacedLast = false;

	void Update () {
        body.AddForce(body.transform.forward * Time.deltaTime * 1000.0f * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
        body.AddTorque(new Vector3(0.0f, Time.deltaTime * 100000000.0f * Input.GetAxis("Horizontal"), 0.0f));

        if(!wasSpacedLast && Input.GetKey("space")) {
            GameObject ball = Instantiate(prefab);
            ball.transform.position = body.transform.position + new Vector3(0.0f, 24.0f, 0.0f);
            ball.GetComponent<Rigidbody>().AddForce((800.0f * (body.transform.forward + new Vector3(0.0f, 0.5f, 0.0f))) + body.velocity, ForceMode.VelocityChange);
        }
        wasSpacedLast = Input.GetKey("space");
	}

}