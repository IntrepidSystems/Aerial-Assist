using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldProperties : MonoBehaviour {

    public static GameObject BLUE_HUMAN_PLAYER_LEFT_SCORING, BLUE_HUMAN_PLAYER_RIGHT_SCORING, BLUE_HUMAN_PLAYER_LEFT_INBOUND, BLUE_HUMAN_PLAYER_RIGHT_INBOUND, BLUE_PEDESTAL,
        RED_HUMAN_PLAYER_LEFT_SCORING, RED_HUMAN_PLAYER_RIGHT_SCORING, RED_HUMAN_PLAYER_LEFT_INBOUND, RED_HUMAN_PLAYER_RIGHT_INBOUND, RED_PEDESTAL;
    public GameObject blueHumanPlayerLeftScoring, blueHumanPlayerRightScoring, blueHumanPlayerLeftInbound, blueHumanPlayerRightInbound, bluePedestal,
        redHumanPlayerLeftScoring, redHumanPlayerRightScoring, redHumanPlayerLeftInbound, redHumanPlayerRightInbound, redPedestal;

    public static GameObject[] ROBOTS;
    public GameObject[] robots;

    public static List<GameObject> BLUE_BALLS, RED_BALLS;
    public List<GameObject> blueBalls, redBalls;

    public static GameObject BLUE_LEFT_LIGHTS, BLUE_RIGHT_LIGHTS, RED_LEFT_LIGHTS, RED_RIGHT_LIGHTS;
    public GameObject blueLeftLights, blueRightLights, redLeftLights, redRightLights;

    public static GameObject BLUE_LEFT_INNER_LIGHTS, BLUE_LEFT_MIDDLE_LIGHTS, BLUE_LEFT_OUTER_LIGHTS,
        BLUE_RIGHT_INNER_LIGHTS, BLUE_RIGHT_MIDDLE_LIGHTS, BLUE_RIGHT_OUTER_LIGHTS,
        RED_LEFT_INNER_LIGHTS, RED_LEFT_MIDDLE_LIGHTS, RED_LEFT_OUTER_LIGHTS,
        RED_RIGHT_INNER_LIGHTS, RED_RIGHT_MIDDLE_LIGHTS, RED_RIGHT_OUTER_LIGHTS;
    public GameObject blueLeftInnerLights, blueLeftMiddleLights, blueLeftOuterLights,
        blueRightInnerLights, blueRightMiddleLights, blueRightOuterLights,
        redLeftInnerLights, redLeftMiddleLights, redLeftOuterLights,
        redRightInnerLights, redRightMiddleLights, redRightOuterLights;

    public static GameObject FIELD;
    public GameObject field;

    public static GameObject BLUE_BALL_PREFAB, RED_BALL_PREFAB;
    public GameObject blueBallPrefab, redBallPrefab;


    void Start () {
        Application.runInBackground = true;

        FieldProperties.BLUE_HUMAN_PLAYER_LEFT_SCORING = blueHumanPlayerLeftScoring;
        FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_SCORING = blueHumanPlayerRightScoring;
        FieldProperties.BLUE_HUMAN_PLAYER_LEFT_INBOUND = blueHumanPlayerLeftInbound;
        FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_INBOUND = blueHumanPlayerRightInbound;
        FieldProperties.BLUE_PEDESTAL = bluePedestal;
        FieldProperties.RED_HUMAN_PLAYER_LEFT_SCORING = redHumanPlayerLeftScoring;
        FieldProperties.RED_HUMAN_PLAYER_RIGHT_SCORING = redHumanPlayerRightScoring;
        FieldProperties.RED_HUMAN_PLAYER_LEFT_INBOUND = redHumanPlayerLeftInbound;
        FieldProperties.RED_HUMAN_PLAYER_RIGHT_INBOUND = redHumanPlayerRightInbound;
        FieldProperties.RED_PEDESTAL = redPedestal;

        FieldProperties.ROBOTS = robots;
        FieldProperties.BLUE_BALLS = blueBalls;
        FieldProperties.RED_BALLS = redBalls;

        FieldProperties.BLUE_LEFT_LIGHTS = blueLeftLights;
        FieldProperties.BLUE_RIGHT_LIGHTS = blueRightLights;
        FieldProperties.RED_LEFT_LIGHTS = redLeftLights;
        FieldProperties.RED_RIGHT_LIGHTS = redRightLights;

        FieldProperties.BLUE_LEFT_INNER_LIGHTS = blueLeftInnerLights;
        FieldProperties.BLUE_LEFT_MIDDLE_LIGHTS = blueLeftMiddleLights;
        FieldProperties.BLUE_LEFT_OUTER_LIGHTS = blueLeftOuterLights;
        FieldProperties.BLUE_RIGHT_INNER_LIGHTS = blueRightInnerLights;
        FieldProperties.BLUE_RIGHT_MIDDLE_LIGHTS = blueRightMiddleLights;
        FieldProperties.BLUE_RIGHT_OUTER_LIGHTS = blueRightOuterLights;
        FieldProperties.RED_LEFT_INNER_LIGHTS = redLeftInnerLights;
        FieldProperties.RED_LEFT_MIDDLE_LIGHTS = redLeftMiddleLights;
        FieldProperties.RED_LEFT_OUTER_LIGHTS = redLeftOuterLights;
        FieldProperties.RED_RIGHT_INNER_LIGHTS = redRightInnerLights;
        FieldProperties.RED_RIGHT_MIDDLE_LIGHTS = redRightMiddleLights;
        FieldProperties.RED_RIGHT_OUTER_LIGHTS = redRightOuterLights;

        FieldProperties.FIELD = field;
        FieldProperties.BLUE_BALL_PREFAB = blueBallPrefab;
        FieldProperties.RED_BALL_PREFAB = redBallPrefab;
    }

	void Update () {
        if (Scorekeeper.TIME <= 5.0f) {
            FieldProperties.BLUE_LEFT_LIGHTS.active = true;
            FieldProperties.BLUE_RIGHT_LIGHTS.active = false;
            FieldProperties.RED_LEFT_LIGHTS.active = true;
            FieldProperties.RED_RIGHT_LIGHTS.active = false;
        }
        else if (Scorekeeper.TIME <= 10.0f) {
            FieldProperties.BLUE_LEFT_LIGHTS.active = false;
            FieldProperties.BLUE_RIGHT_LIGHTS.active = true;
            FieldProperties.RED_LEFT_LIGHTS.active = false;
            FieldProperties.RED_RIGHT_LIGHTS.active = true;
        }
        else if (Scorekeeper.TIME <= 11.5f) {
            FieldProperties.BLUE_LEFT_LIGHTS.active = true;
            FieldProperties.BLUE_RIGHT_LIGHTS.active = true;

            FieldProperties.BLUE_LEFT_INNER_LIGHTS.active = false;
            FieldProperties.BLUE_LEFT_MIDDLE_LIGHTS.active = false;
            FieldProperties.BLUE_LEFT_OUTER_LIGHTS.active = false;

            FieldProperties.BLUE_RIGHT_INNER_LIGHTS.active = false;
            FieldProperties.BLUE_RIGHT_MIDDLE_LIGHTS.active = false;
            FieldProperties.BLUE_RIGHT_OUTER_LIGHTS.active = false;

            FieldProperties.RED_LEFT_LIGHTS.active = true;
            FieldProperties.RED_RIGHT_LIGHTS.active = true;

            FieldProperties.RED_LEFT_INNER_LIGHTS.active = false;
            FieldProperties.RED_LEFT_MIDDLE_LIGHTS.active = false;
            FieldProperties.RED_LEFT_OUTER_LIGHTS.active = false;

            FieldProperties.RED_RIGHT_INNER_LIGHTS.active = false;
            FieldProperties.RED_RIGHT_MIDDLE_LIGHTS.active = false;
            FieldProperties.RED_RIGHT_OUTER_LIGHTS.active = false;
        }
    }

}