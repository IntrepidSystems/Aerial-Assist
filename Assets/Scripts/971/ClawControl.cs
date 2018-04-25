using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawControl : MonoBehaviour {

    public float clawP, maxClawSpeed;

    void Start () {
		
	}
	
	void Update () {

	}

    public void MoveArm(float movement) {
        gameObject.transform.Rotate(new Vector3(movement, 0.0f, 0.0f));
    }

    public void MoveArmToEulerAngle(float target) {
        while(target > 180.0f) {
            target -= 360.0f;
        }
        while(target < -180.0f) {
            target += 360.0f;
        }

        float angle = GetClawAngle();
        float error = target - angle;
        float output = clawP * error;
        output = (Mathf.Abs(output) > maxClawSpeed) ? maxClawSpeed * Mathf.Sign(output) : output;

        MoveArm(output * Time.deltaTime);
    }

    public float GetClawAngle() {
        float angle = gameObject.transform.localRotation.eulerAngles.x;
        while(angle > 180.0f) {
            angle -= 360.0f;
        }
        while(angle < -180.0f) {
            angle += 360.0f;
        }
        return angle;
    }

}