using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour {

    public static MatchState MATCH_STATE = MatchState.DISABLED;
    public static float TIME = 0.0f;

    public static System.Random RANDOM_MUSIC_MAKER;

    public GameObject audioPlayerPrefab;
    public AudioClip matchStart, autoEnd, teleopBegin, warning, matchEnd;
    public List<AudioClip> musicClips;

    public int blueScore = 0, redScore = 0;
    public Text blueScoreText, redScoreText, timeText;

    private bool hasPlayedMatchStart, hasPlayedAutoEnd, hasPlayedTeleopBegin, hasPlayedWarning, hasPlayedMatchEnd;
    private bool hasStartedMusic;
    private AudioSource musicPlayer;

    public enum MatchState { DISABLED, AUTONOMOUS, TELEOP }

	void Start () {
        Scorekeeper.TIME = 0.0f;
        blueScore = 0;
        hasPlayedMatchStart = false;
        hasPlayedAutoEnd = false;
        hasPlayedTeleopBegin = false;
        hasPlayedWarning = false;
        hasPlayedMatchEnd = false;
        hasStartedMusic = false;

        Invoke("BeginMatch", 1.0f);
        Scorekeeper.RANDOM_MUSIC_MAKER = new System.Random();
	}

    void Update() {
        if(Scorekeeper.MATCH_STATE != MatchState.DISABLED) {
            Scorekeeper.TIME += Time.deltaTime;
            blueScoreText.text = blueScore.ToString();
            redScoreText.text = redScore.ToString();

            if (Scorekeeper.TIME >= 1.0f && !hasStartedMusic) {
                int musicIndex = (int) Mathf.Round(Random.value * ((float) musicClips.Count)) - 1;
                musicPlayer = PlaySound(musicClips[musicIndex]);
                musicPlayer.volume = 0.07f;
                hasStartedMusic = true;
            }
            else if (Scorekeeper.TIME >= 10.0f && !hasPlayedAutoEnd) {
                PlaySound(autoEnd);
                hasPlayedAutoEnd = true;

                timeText.text = "0:00";
            }
            else if (Scorekeeper.TIME >= 11.5f && !hasPlayedTeleopBegin) {
                PlaySound(teleopBegin);
                Scorekeeper.MATCH_STATE = MatchState.TELEOP;
                hasPlayedTeleopBegin = true;
            }
            else if (Scorekeeper.TIME >= 117.5f && !hasPlayedWarning) {
                PlaySound(warning);
                hasPlayedWarning = true;
            }
            else if (Scorekeeper.TIME >= 146.5f && !hasPlayedMatchEnd) {
                PlaySound(matchEnd);
                musicPlayer.Stop();
                Scorekeeper.MATCH_STATE = MatchState.DISABLED;
                hasPlayedMatchEnd = true;
            }
        }

        if (Scorekeeper.MATCH_STATE == MatchState.AUTONOMOUS) {
            int time = (int)Mathf.Ceil(10.0f - Scorekeeper.TIME);
            if (time >= 0) {
                if (time < 10) {
                    timeText.text = "0:0" + time;
                }
                else {
                    timeText.text = "0:" + time;
                }
            }
            else {
                timeText.text = "0:00";
            }
        } else if (Scorekeeper.MATCH_STATE == MatchState.TELEOP) {
            int time = 135 - (int)Mathf.Floor(Scorekeeper.TIME - 11.5f);
            if (time >= 120) {
                if (time > 129) {
                    timeText.text = "2:" + (time - 120);
                }
                else {
                    timeText.text = "2:0" + (time - 120);
                }
            }
            else if (time >= 60) {
                if (time > 69) {
                    timeText.text = "1:" + (time - 60);
                }
                else {
                    timeText.text = "1:0" + (time - 60);
                }
            }
            else if (time >= 0) {
                if (time > 9) {
                    timeText.text = "0:" + time;
                }
                else {
                    timeText.text = "0:0" + time;
                }
            } else {
                timeText.text = "0:00";
            }
        } else if (Scorekeeper.MATCH_STATE == MatchState.DISABLED) {
            timeText.text = "0:00";
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

    public static void SetMatchState(MatchState state) {
        Scorekeeper.MATCH_STATE = state;
    }

    private void BeginMatch() {
        Scorekeeper.SetMatchState(MatchState.AUTONOMOUS);
        PlaySound(matchStart);
        hasPlayedMatchStart = true;
    }

}