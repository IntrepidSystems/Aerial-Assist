using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioClip musicClip;
    public AudioSource musicSource;

	void Start () {
        musicSource.clip = musicClip;
        musicSource.Play();
	}
	
	void Update () {
        
	}

}