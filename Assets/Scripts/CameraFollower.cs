using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public GameObject follow;

	void Start () {
		
	}
	
	void Update () {
        transform.LookAt(follow.transform);
	}

}