using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour {

    public int score = 0;
    public Text scoreText;

	void Start () {
        score = 0;
	}
	
	void Update () {
        scoreText.text = "Score: " + score.ToString();
	}

}