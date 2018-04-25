using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    private Rigidbody body;
    public GameObject prefab, heldBall, clawBack, claw;
    public float shotPower, shotOffset, driveSpeed, turnSpeed;

    private enum ClawState { VERTICAL, FORWARD_LONG_SHOT, FORWARD_SHORT_SHOT, BACKWARD_LONG_SHOT, BACKWARD_SHORT_SHOT, INTAKE }
    public float INTAKE, VERTICAL, FORWARD_LONG_SHOT, BACKWARD_LONG_SHOT;
    private ClawControl clawControl;
    private ClawState clawState;

    void Start () {
        body = GetComponent<Rigidbody>();

        clawControl = claw.GetComponent<ClawControl>();
        clawState = ClawState.VERTICAL;
	}

    private bool wasSpacedLast = false;

	void Update () {
        body.AddForce(body.transform.forward * Time.deltaTime * driveSpeed * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
        body.AddTorque(new Vector3(0.0f, Time.deltaTime * turnSpeed * Input.GetAxis("Horizontal"), 0.0f));

        if(!wasSpacedLast && Input.GetKey("space") && heldBall.active) {
            GameObject ball = Instantiate(prefab);
            ball.transform.position = heldBall.transform.position + (shotOffset * (heldBall.transform.position - clawBack.transform.position));
            ball.GetComponent<Rigidbody>().AddForce(shotPower * (heldBall.transform.position - clawBack.transform.position) + body.velocity, ForceMode.VelocityChange);
            heldBall.active = false;
        }
        wasSpacedLast = Input.GetKey("space");

        ControlClaw();
	}

    private void ControlClaw() {
        if(Input.GetKey("w")) {
            clawState = ClawState.VERTICAL;
        } else if(Input.GetKey("d")) {
            clawState = ClawState.FORWARD_LONG_SHOT;
        } else if(Input.GetKey("s")) {
            clawState = ClawState.INTAKE;
        } else if(Input.GetKey("a")) {
            clawState = ClawState.BACKWARD_LONG_SHOT;
        }

        switch(clawState) {
            case ClawState.VERTICAL:
                clawControl.MoveArmToEulerAngle(VERTICAL);
                break;

            case ClawState.FORWARD_LONG_SHOT:
                clawControl.MoveArmToEulerAngle(FORWARD_LONG_SHOT);
                break;

            case ClawState.INTAKE:
                clawControl.MoveArmToEulerAngle(INTAKE);
                break;

            case ClawState.BACKWARD_LONG_SHOT:
                clawControl.MoveArmToEulerAngle(BACKWARD_LONG_SHOT);
                break;
        }
    }

}