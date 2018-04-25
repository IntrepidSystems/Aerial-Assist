using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBall : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Blue High Goal Sensor" || other.tag == "Red High Goal Sensor" || other.tag == "Blue Low Goal Sensor" || other.tag == "Red Low Goal Sensor") {
            Invoke("DestroyBall", 1.0f);
        }
    }

    public void DestroyBall() {
        Destroy(gameObject);
    }

}