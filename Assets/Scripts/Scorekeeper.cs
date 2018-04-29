using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour {

    public static MatchState MATCH_STATE = MatchState.DISABLED;
    public static float TIME = 0.0f;

    public GameObject audioPlayerPrefab;
    public AudioClip matchStart, autoEnd, teleopBegin, warning, matchEnd, music;

    public int score = 0;
    public Text scoreText;

    private bool hasPlayedMatchStart, hasPlayedAutoEnd, hasPlayedTeleopBegin, hasPlayedWarning, hasPlayedMatchEnd;
    private bool hasStartedMusic;
    private AudioSource musicPlayer;

    public enum MatchState { DISABLED, AUTONOMOUS, TELEOP }

	void Start () {
        Scorekeeper.TIME = 0.0f;
        score = 0;
        hasPlayedMatchStart = false;
        hasPlayedAutoEnd = false;
        hasPlayedTeleopBegin = false;
        hasPlayedWarning = false;
        hasPlayedMatchEnd = false;
        hasStartedMusic = false;

        PlaySound(matchStart);
        Scorekeeper.MATCH_STATE = MatchState.AUTONOMOUS;
        hasPlayedMatchStart = true;
	}

    void Update() {
        Scorekeeper.TIME += Time.deltaTime;
        scoreText.text = "Score: " + score;

        if(Scorekeeper.TIME >= 1.0f && !hasStartedMusic) {
            musicPlayer = PlaySound(music);
            hasStartedMusic = true;
        } else if (Scorekeeper.TIME >= 10.0f && !hasPlayedAutoEnd) {
            PlaySound(autoEnd);
            hasPlayedAutoEnd = true;
        } else if(Scorekeeper.TIME >= 11.5f && !hasPlayedTeleopBegin) {
            PlaySound(teleopBegin);
            Scorekeeper.MATCH_STATE = MatchState.TELEOP;
            hasPlayedTeleopBegin = true;
        } else if(Scorekeeper.TIME >= 117.0f && !hasPlayedWarning) {
            PlaySound(warning);
            hasPlayedWarning = true;
        } else if(Scorekeeper.TIME >= 147.0f && !hasPlayedMatchEnd) {
            PlaySound(matchEnd);
            musicPlayer.Stop();
            Scorekeeper.MATCH_STATE = MatchState.DISABLED;
            hasPlayedMatchEnd = true;
        }
	}

    AudioSource PlaySound(AudioClip audio) {
        GameObject audioPlayer = Instantiate(audioPlayerPrefab);
        AudioScript audioScript = audioPlayer.GetComponent<AudioScript>();
        audioScript.musicClip = audio;
        audioScript.musicSource = audioPlayer.GetComponent<AudioSource>();
        audioScript.musicSource.Play();
        return audioScript.musicSource;
    }

}