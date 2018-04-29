using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Control : MonoBehaviour {

    private Rigidbody body;

    public bool isBlue;
    public GameObject ballPrefab, heldBall, clawBack, claw;
    public float shotPower, shotOffset, driveSpeed, turnSpeed;
    public GameObject field;
    public GameObject intakeTarget;

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
        if (Scorekeeper.MATCH_STATE == Scorekeeper.MatchState.TELEOP) {
            body.AddForce(new Vector3(body.transform.forward.x, 0.0f, body.transform.forward.z) * Time.deltaTime * driveSpeed * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
            body.AddTorque(new Vector3(0.0f, Time.deltaTime * turnSpeed * Input.GetAxis("Horizontal"), 0.0f));

            if (!wasSpacedLast && Input.GetKey("space") && heldBall.active) {
                FireBall();
            }
            wasSpacedLast = Input.GetKey("space");

            if(heldBall.active) {
                FieldProperties.BLUE_LEFT_MIDDLE_LIGHTS.active = false;
                FieldProperties.BLUE_LEFT_OUTER_LIGHTS.active = false;
                FieldProperties.BLUE_RIGHT_MIDDLE_LIGHTS.active = false;
                FieldProperties.BLUE_RIGHT_OUTER_LIGHTS.active = false;

                FieldProperties.BLUE_LEFT_INNER_LIGHTS.active = true;
                FieldProperties.BLUE_RIGHT_INNER_LIGHTS.active = true;
            }

            ControlClaw();
        }
        else if (Scorekeeper.MATCH_STATE == Scorekeeper.MatchState.AUTONOMOUS) {
            if (Scorekeeper.TIME <= 1.0f) {
                clawControl.MoveArmToEulerAngle(BACKWARD_LONG_SHOT);
                DriveToAndPointAtPosition(new Vector3(-74.0f, 0.0f, 245.0f), true);
            } else if(Scorekeeper.TIME <= 1.6f) {
                DriveToPosition(new Vector3(-74.0f, 0.0f, 245.0f), true);
            } else if(Scorekeeper.TIME <= 2.5f) {
                PointAtLocation(new Vector3(-74.0f, 0.0f, 325.0f), true);
            } else if(Scorekeeper.TIME <= 2.75f) {
                FireBall();
            } else if(Scorekeeper.TIME <= 5.0f) {
                clawControl.MoveArmToEulerAngle(INTAKE);

                if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 45.0f)) > 80.0f) {
                    DriveToAndPointAtPosition(new Vector3(0.0f, 0.0f, 45.0f), false);
                } else {
                    DriveToPosition(new Vector3(0.0f, 0.0f, 45.0f), false);
                }
            } else if(Scorekeeper.TIME <= 8.0f) {
                clawControl.MoveArmToEulerAngle(BACKWARD_LONG_SHOT);

                if (Vector3.Distance(transform.position, new Vector3(74.0f, 0.0f, 245.0f)) > 80.0f) {
                    DriveToAndPointAtPosition(new Vector3(74.0f, 0.0f, 245.0f), true);
                }
                else {
                    DriveToPosition(new Vector3(74.0f, 0.0f, 245.0f), true);
                }
            } else if(Scorekeeper.TIME <= 9.0f) {
                PointAtLocation(new Vector3(74.0f, 0.0f, 325.0f), true);
            } else {
                FireBall();
            }
        }
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

    public void FireBall() {
        if (heldBall.active) {
            GameObject ball = Instantiate(ballPrefab);
            BallController intakeScript = ball.GetComponent<BallController>();
            intakeScript.field = field;
            intakeScript.ballPrefab = ballPrefab;
            intakeScript.isBlue = isBlue;

            ball.transform.position = heldBall.transform.position + (shotOffset * (heldBall.transform.position - clawBack.transform.position));
            ball.GetComponent<Rigidbody>().AddForce(shotPower * 1.5f * (heldBall.transform.position - clawBack.transform.position) + body.velocity, ForceMode.VelocityChange);
            heldBall.active = false;
        }
    }

    public void DriveToAndPointAtPosition(Vector3 target, bool reverse) {
        DriveToPosition(target, reverse);
        PointAtLocation(target, reverse);
    }

    public void DriveToPosition(Vector3 target, bool reverse) {
        float output = ((reverse) ? -1.0f : 1.0f) * Vector3.Distance(transform.position,target);
        output = (Mathf.Abs(output) > 1.0) ? Mathf.Sign(output) * 1.0f : output;

        if (Vector3.Distance(transform.position, target) > 10.0f) {
            body.AddForce(new Vector3(body.transform.forward.x, 0.0f, body.transform.forward.z) * Time.deltaTime * driveSpeed * output, ForceMode.VelocityChange);
        } else {
            body.AddForce(Vector3.zero);
        }
    }

    public void PointAtLocation(Vector3 target, bool reverse) {
        float adjAngleError = Vector3.SignedAngle(transform.forward, target - transform.position, new Vector3(0.0f, 1.0f, 0.0f));
        if(reverse) {
            adjAngleError += 180.0f;
        }
        while(adjAngleError > 180.0f) {
            adjAngleError -= 360.0f;
        }
        while(adjAngleError < -180.0f) {
            adjAngleError += 360.0f;
        }

        float output = (Mathf.Abs(adjAngleError) > 1.0f) ? Mathf.Sign(adjAngleError) * 1.0f : adjAngleError;
        body.AddTorque(new Vector3(0.0f, Time.deltaTime * turnSpeed * output * 0.75f, 0.0f));
    }

}