using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntakeBall : MonoBehaviour {

    public GameObject humanPlayerStation1, humanPlayerStation2, humanPlayerStation1Prep, humanPlayerStation2Prep;
    public GameObject field;

    private Rigidbody body;
    private GameObject heldBall;
    private float intakeForce = 1000.0f;

    private Scorekeeper scorekeeper;

	void Start () {
        body = GetComponent<Rigidbody>();
        scorekeeper = field.GetComponent<Scorekeeper>();
	}
	
	void Update () {
        RunIntake();
        SendToNearestHumanPlayer();
	}

    private bool isBeingIntaken = false;
    private void RunIntake() {
        if (isBeingIntaken && !heldBall.active) {
            body.detectCollisions = false;
            body.AddForce(intakeForce * (heldBall.transform.position - transform.position));
        }
        else {
            body.detectCollisions = true;
        }
    }

    private bool isBeingSentToNearestHumanPlayer = false, isBehindBlueGoal = false;
    private void SendToNearestHumanPlayer() {
        if (isBeingSentToNearestHumanPlayer) {
            if (!isBehindBlueGoal) {
                GameObject targetStation = (Vector3.Distance(gameObject.transform.position, humanPlayerStation1.transform.position) <
                    Vector3.Distance(gameObject.transform.position, humanPlayerStation2.transform.position)) ? humanPlayerStation1 : humanPlayerStation2;
                Vector3 output = Vector3.ClampMagnitude(100000.0f * (targetStation.transform.position - gameObject.transform.position), 1000.0f);
                body.AddForce(output);
            } else {
                GameObject targetStation = (Vector3.Distance(gameObject.transform.position, humanPlayerStation1Prep.transform.position) <
                    Vector3.Distance(gameObject.transform.position, humanPlayerStation2Prep.transform.position)) ? humanPlayerStation1Prep : humanPlayerStation2Prep;
                Vector3 output = Vector3.ClampMagnitude(100000.0f * (targetStation.transform.position - gameObject.transform.position), 1000.0f);
                body.AddForce(output);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
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

        if (other.tag == "Field Box") {
            isBeingSentToNearestHumanPlayer = false;
        }
        if(other.tag == "Behind Blue Goal Box") {
            isBehindBlueGoal = true;
        }

        if(other.tag == "Truss Sensor") {
            scorekeeper.score = scorekeeper.score + 10;
        }
        if(other.tag == "Blue High Goal Sensor") {
            scorekeeper.score = scorekeeper.score + 10;
        }
        if(other.tag == "Blue Low Goal Sensor") {
            scorekeeper.score = scorekeeper.score + 1;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Intake Wheel") {
            isBeingIntaken = false;
            heldBall = null;
            intakeForce = 0.0f;
        }

        if(other.tag == "Field Box") {
            isBeingSentToNearestHumanPlayer = true;
        }
        if(other.tag == "Behind Blue Goal Box") {
            isBehindBlueGoal = false;
        }
    }

    public void DestroyBall() {
        Destroy(gameObject);
    }

}