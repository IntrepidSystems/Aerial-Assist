using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioClip musicClip;
    public AudioSource musicSource;

	void Start () {
        musicSource.clip = musicClip;
        musicSource.Play();
        InvokeRepeating("ChangePitch", 8.5f, 0.25f);
	}

    private bool isFlipped = false;
    void ChangePitch() {
        if(isFlipped) {
            musicSource.pitch = 1.0f;
            isFlipped = false;
        } else {
            musicSource.pitch = 1.1f;
            isFlipped = true;
        }
    }
	
	void Update () {
        
	}

}