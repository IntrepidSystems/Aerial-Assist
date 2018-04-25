using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBall : MonoBehaviour {

    private Rigidbody body;
    private GameObject heldBall;
    private float intakeForce = 0.0f;
    

	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	void Update () {
        RunIntake();
	}

    private bool isBeingIntaken = false;
    private void RunIntake() {
        if (isBeingIntaken && !heldBall.active) {
            body.detectCollisions = false;
            body.AddForce(1000.0f * (heldBall.transform.position - transform.position));
        }
        else {
            body.detectCollisions = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Blue High Goal Sensor" || other.tag == "Red High Goal Sensor"
            || other.tag == "Blue Low Goal Sensor" || other.tag == "Red Low Goal Sensor"
            || other.tag == "Truss Sensor") {
            Invoke("DestroyBall", 1.0f);
        }

        if (other.tag == "Intake Back") {
            ClawBack clawBackScript = other.gameObject.GetComponent<ClawBack>();
            GameObject heldBall = clawBackScript.associatedBall;
            if (!heldBall.active) {
                heldBall.SetActive(true);
                DestroyBall();
            }
        }

        if(other.tag == "Intake Wheel") {
            isBeingIntaken = true;
            FollowObject followScript = other.gameObject.GetComponent<FollowObject>();
            heldBall = followScript.associatedBall;
            intakeForce = followScript.intakeForce;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Intake Wheel") {
            isBeingIntaken = false;
            heldBall = null;
            intakeForce = 0.0f;
        }
    }

    public void DestroyBall() {
        Destroy(gameObject);
    }

}