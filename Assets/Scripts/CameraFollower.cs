using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public GameObject follow;

	void Start () {
		
	}
	
	void Update () {
        transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
	}

}