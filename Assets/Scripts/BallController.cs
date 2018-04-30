using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

    public bool isBlue;

    private Rigidbody body;
    private Scorekeeper scorekeeper;

    enum BallState { INTAKING, RETURNING, WAITING_FOR_ROBOT, SCORED, NONE };
    private BallState currentBallState;

	void Start() {
        body = GetComponent<Rigidbody>();
        scorekeeper = FieldProperties.FIELD.GetComponent<Scorekeeper>();
	}
	
	void Update() {
        ChangeBallState();
        RunBallActions();
	}

    private bool isIntaking = false, isReturning = false, isWaitingForRobot = false, isScored = false;
    void ChangeBallState() {
        if(isScored) {
            currentBallState = BallState.SCORED;
        } else if (isIntaking) {
            currentBallState = BallState.INTAKING;
        } else if (isReturning) {
            currentBallState = BallState.RETURNING;
        } else if (isWaitingForRobot) {
            currentBallState = BallState.WAITING_FOR_ROBOT;
        } else {
            currentBallState = BallState.NONE;
        }
    }

    void RunBallActions() {
        switch(currentBallState) {
            case BallState.INTAKING:
                if (!intakingRobotHeldBall.active && isIntakingRobotBlue == isBlue) {
                    body.AddForce(800.0f * (intakingRobotHeldBall.transform.position - transform.position));
                    body.detectCollisions = false;
                } else {
                    body.detectCollisions = true;
                }
                break;

            case BallState.RETURNING:
                if (Mathf.Abs(transform.position.z) < 300.0f || Mathf.Abs(transform.position.x) > 180.0f) {
                    Vector3 output = Vector3.ClampMagnitude(100000.0f * (ballReturnTargetLocation - transform.position), 1000.0f) - (2.0f * body.velocity);
                    body.AddForce(output);
                } else {
                    Vector3 prepLocation = new Vector3(ballReturnTargetLocation.x, ballReturnTargetLocation.y, 450.0f * Mathf.Sign(ballReturnTargetLocation.z));
                    Vector3 output = Vector3.ClampMagnitude(100000.0f * (prepLocation - transform.position), 1000.0f) - (2.0f * body.velocity);
                    body.AddForce(output);
                }

                if(Vector3.Distance(transform.position, ballReturnTargetLocation) < 12.0f) {
                    UpdateWaitingForRobotInfo(true, ballReturnTargetLocation);
                    UpdateBallReturnInfo(false, Vector3.zero);
                }

                body.detectCollisions = true;
                break;

            case BallState.WAITING_FOR_ROBOT:
                float closestDistance = float.MaxValue;
                GameObject closestRobotOfSameColor = null;
                foreach(GameObject robot in FieldProperties.ROBOTS) {
                    Control controlScript = robot.GetComponent<Control>();
                    if(controlScript.isBlue == isBlue && Vector3.Distance(controlScript.transform.position, transform.position) < closestDistance) {
                        closestDistance = Vector3.Distance(controlScript.transform.position, transform.position);
                        closestRobotOfSameColor = robot;
                    }
                }

                if (closestDistance > 150.0f) {
                    Vector3 output = Vector3.ClampMagnitude(100000.0f * (waitingForRobotLocation - transform.position), 1000.0f);
                    body.AddForce(output);
                } else {
                    UpdateWaitingForRobotInfo(false, Vector3.zero);
                    UpdateBallReturnInfo(false, Vector3.zero);
                    Vector3 output = Vector3.ClampMagnitude(100000.0f * (closestRobotOfSameColor.GetComponent<Control>().clawBack.transform.position - gameObject.transform.position), 550.0f) + new Vector3(0.0f, 60.0f, 0.0f);
                    body.velocity = output;
                }

                body.detectCollisions = true;
                break;

            case BallState.SCORED:
                body.detectCollisions = true;

                if (!hasSpawnedNewPedestalBall) {
                    Invoke("SpawnNewPedestalBall", 1.0f);
                    Invoke("DestroyBall", 2.0f);
                    UpdateRespawnBallInfo(true, true);
                }
                break;

            case BallState.NONE:
                body.detectCollisions = true;
                break;
        }
    }

    private bool isIntakingRobotBlue;
    private GameObject intakingRobotHeldBall, intakingRobotWheel;
    private FollowObject intakingRobotFollowerScript;
    void UpdateIntakingInfo(bool isIntaking, bool isIntakingRobotBlue, GameObject intakingRobotHeldBall, GameObject intakingRobotWheel, FollowObject intakingRobotFollowerScript) {
        this.isIntakingRobotBlue = isIntakingRobotBlue;
        this.intakingRobotHeldBall = intakingRobotHeldBall;
        this.intakingRobotWheel = intakingRobotWheel;
        this.intakingRobotFollowerScript = intakingRobotFollowerScript;
        this.isIntaking = isIntaking;
    }

    private Vector3 ballReturnTargetLocation;
    void UpdateBallReturnInfo(bool isBallReturning, Vector3 target) {
        ballReturnTargetLocation = target;
        this.isReturning = isBallReturning;
    }

    private Vector3 waitingForRobotLocation;
    void UpdateWaitingForRobotInfo(bool isWaitingForRobot, Vector3 target) {
        waitingForRobotLocation = target;
        this.isWaitingForRobot = isWaitingForRobot;
    }

    private bool hasSpawnedNewPedestalBall = false;
    void UpdateRespawnBallInfo(bool isScored, bool hasSpawnedNewPedestalBall) {
        this.isScored = isScored;
        this.hasSpawnedNewPedestalBall = hasSpawnedNewPedestalBall;
    }

    Vector3 GetClosestLocationToPosition(Vector3 currentPosition, Vector3[] locations) {
        float minDistance = float.MaxValue;
        Vector3 closestLocation = Vector3.zero;
        foreach(Vector3 vector in locations) {
            if(Vector3.Distance(vector, currentPosition) < minDistance) {
                minDistance = Vector3.Distance(vector, currentPosition);
                closestLocation = vector;
            }
        }
        return closestLocation;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Intake Wheel") {
            UpdateIntakingInfo(true, other.gameObject.GetComponent<FollowObject>().isBlue, other.gameObject.GetComponent<FollowObject>().associatedBall,
                other.gameObject, other.gameObject.GetComponent<FollowObject>());
        }
        if(other.tag == "Intake Back") {
            GameObject intakeBack = other.gameObject;
            ClawBack intakeBackScript = intakeBack.GetComponent<ClawBack>();
            if (intakeBackScript.isBlue == isBlue) {
                if (!intakeBackScript.associatedBall.active) {
                    intakeBackScript.associatedBall.active = true;
                    DestroyBall();
                }
            } else {
                DestroyBallAndSpawnNew();
            }
        }

        if(other.tag == "Field Box") {
            UpdateBallReturnInfo(false, Vector3.zero);
        }

        if (isBlue) {
            if (other.tag == "Blue High Goal Sensor" || other.tag == "Blue Low Goal Sensor") {
                scorekeeper.blueScore += (((other.tag == "Blue High Goal Sensor") ? 10 : 1) + ((Scorekeeper.MATCH_STATE == Scorekeeper.MatchState.AUTONOMOUS) ? 10 : 0));
                UpdateRespawnBallInfo(true, false);
            }
            else if (other.tag == "Truss Sensor") {
                scorekeeper.blueScore += 10;
            }
        } else {
            if (other.tag == "Red High Goal Sensor" || other.tag == "Red Low Goal Sensor") {
                scorekeeper.redScore += (((other.tag == "Red High Goal Sensor") ? 10 : 1) + ((Scorekeeper.MATCH_STATE == Scorekeeper.MatchState.AUTONOMOUS) ? 10 : 0));
                UpdateRespawnBallInfo(true, false);
            }
            else if (other.tag == "Truss Sensor") {
                scorekeeper.redScore += 10;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Intake Wheel") {
            UpdateIntakingInfo(false, false, null, null, null);
        }

        if(other.tag == "Field Box" && currentBallState != BallState.INTAKING) {
            if (isBlue) {
                UpdateBallReturnInfo(true, GetClosestLocationToPosition(transform.position,
                    new Vector3[] { FieldProperties.BLUE_HUMAN_PLAYER_LEFT_SCORING.transform.position, FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_SCORING.transform.position,
                FieldProperties.BLUE_HUMAN_PLAYER_LEFT_INBOUND.transform.position, FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_INBOUND.transform.position }
                    ));
            } else {
                UpdateBallReturnInfo(true, GetClosestLocationToPosition(transform.position,
                    new Vector3[] { FieldProperties.RED_HUMAN_PLAYER_LEFT_SCORING.transform.position, FieldProperties.RED_HUMAN_PLAYER_RIGHT_SCORING.transform.position,
                FieldProperties.RED_HUMAN_PLAYER_LEFT_INBOUND.transform.position, FieldProperties.RED_HUMAN_PLAYER_RIGHT_INBOUND.transform.position }
                    ));
            }
        }
    }

    public void DestroyBall() {
        if (isBlue) {
            FieldProperties.BLUE_BALLS.Remove(gameObject);
        } else {
            FieldProperties.RED_BALLS.Remove(gameObject);
        }
        Destroy(gameObject);
    }

    public void DestroyBallAndSpawnNew() {
        SpawnNewPedestalBall();
        DestroyBall();
    }

    public void SpawnNewPedestalBall() {
        if (isBlue) {
            foreach (GameObject robot in FieldProperties.ROBOTS) {
                if (robot.GetComponent<Control>().heldBall.active) {
                    return;
                }
            }
            if (FieldProperties.BLUE_BALLS.Count > 0) {
                return;
            }

            FieldProperties.BLUE_LEFT_INNER_LIGHTS.active = false;
            FieldProperties.BLUE_RIGHT_INNER_LIGHTS.active = false;

            GameObject newBall = Instantiate(FieldProperties.BLUE_BALL_PREFAB);
            newBall.transform.position = FieldProperties.BLUE_PEDESTAL.transform.position;

            BallController newBallController = newBall.GetComponent<BallController>();
            newBallController.isBlue = isBlue;
            newBallController.currentBallState = BallState.RETURNING;
            newBallController.UpdateBallReturnInfo(true, GetClosestLocationToPosition(newBall.transform.position,
                    new Vector3[] { FieldProperties.BLUE_HUMAN_PLAYER_LEFT_SCORING.transform.position, FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_SCORING.transform.position,
                FieldProperties.BLUE_HUMAN_PLAYER_LEFT_INBOUND.transform.position, FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_INBOUND.transform.position }
                    ));

            FieldProperties.BLUE_BALLS.Add(newBall);
        } else {
            foreach (GameObject robot in FieldProperties.ROBOTS) {
                if (robot.GetComponent<Control>().heldBall.active) {
                    return;
                }
            }
            if (FieldProperties.RED_BALLS.Count > 0) {
                return;
            }

            FieldProperties.RED_LEFT_INNER_LIGHTS.active = false;
            FieldProperties.RED_RIGHT_INNER_LIGHTS.active = false;

            GameObject newBall = Instantiate(FieldProperties.RED_BALL_PREFAB);
            newBall.transform.position = FieldProperties.RED_PEDESTAL.transform.position;

            BallController newBallController = newBall.GetComponent<BallController>();
            newBallController.isBlue = isBlue;
            newBallController.currentBallState = BallState.RETURNING;
            newBallController.UpdateBallReturnInfo(true, GetClosestLocationToPosition(newBall.transform.position,
                    new Vector3[] { FieldProperties.RED_HUMAN_PLAYER_LEFT_SCORING.transform.position, FieldProperties.RED_HUMAN_PLAYER_RIGHT_SCORING.transform.position,
                FieldProperties.RED_HUMAN_PLAYER_LEFT_INBOUND.transform.position, FieldProperties.RED_HUMAN_PLAYER_RIGHT_INBOUND.transform.position }
                    ));

            FieldProperties.RED_BALLS.Add(newBall);
        }
    }

}