using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public GameObject follower, associatedBall;
    public float intakeForce;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.SetPositionAndRotation(follower.transform.position, follower.transform.rotation);
	}

}