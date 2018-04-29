using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldProperties : MonoBehaviour {

    public static GameObject BLUE_HUMAN_PLAYER_LEFT_SCORING, BLUE_HUMAN_PLAYER_RIGHT_SCORING, BLUE_HUMAN_PLAYER_LEFT_INBOUND, BLUE_HUMAN_PLAYER_RIGHT_INBOUND, BLUE_PEDESTAL;
    public GameObject blueHumanPlayerLeftScoring, blueHumanPlayerRightScoring, blueHumanPlayerLeftInbound, blueHumanPlayerRightInbound, bluePedestal;

    public static GameObject[] ROBOTS;
    public GameObject[] robots;

    public static List<GameObject> BLUE_BALLS;
    public List<GameObject> blueBalls;

    public static GameObject BLUE_LEFT_LIGHTS, BLUE_RIGHT_LIGHTS;
    public GameObject blueLeftLights, blueRightLights;

    public static GameObject BLUE_LEFT_INNER_LIGHTS, BLUE_LEFT_MIDDLE_LIGHTS, BLUE_LEFT_OUTER_LIGHTS,
        BLUE_RIGHT_INNER_LIGHTS, BLUE_RIGHT_MIDDLE_LIGHTS, BLUE_RIGHT_OUTER_LIGHTS;
    public GameObject blueLeftInnerLights, blueLeftMiddleLights, blueLeftOuterLights,
        blueRightInnerLights, blueRightMiddleLights, blueRightOuterLights;


    void Start () {
        FieldProperties.BLUE_HUMAN_PLAYER_LEFT_SCORING = blueHumanPlayerLeftScoring;
        FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_SCORING = blueHumanPlayerRightScoring;
        FieldProperties.BLUE_HUMAN_PLAYER_LEFT_INBOUND = blueHumanPlayerLeftInbound;
        FieldProperties.BLUE_HUMAN_PLAYER_RIGHT_INBOUND = blueHumanPlayerRightInbound;
        FieldProperties.BLUE_PEDESTAL = bluePedestal;

        FieldProperties.ROBOTS = robots;
        FieldProperties.BLUE_BALLS = blueBalls;

        FieldProperties.BLUE_LEFT_LIGHTS = blueLeftLights;
        FieldProperties.BLUE_RIGHT_LIGHTS = blueRightLights;

        FieldProperties.BLUE_LEFT_INNER_LIGHTS = blueLeftInnerLights;
        FieldProperties.BLUE_LEFT_MIDDLE_LIGHTS = blueLeftMiddleLights;
        FieldProperties.BLUE_LEFT_OUTER_LIGHTS = blueLeftOuterLights;
        FieldProperties.BLUE_RIGHT_INNER_LIGHTS = blueRightInnerLights;
        FieldProperties.BLUE_RIGHT_MIDDLE_LIGHTS = blueRightMiddleLights;
        FieldProperties.BLUE_RIGHT_OUTER_LIGHTS = blueRightOuterLights;
	}

	void Update () {
        if (Scorekeeper.TIME <= 5.0f) {
            FieldProperties.BLUE_LEFT_LIGHTS.active = true;
            FieldProperties.BLUE_RIGHT_LIGHTS.active = false;
        }
        else if (Scorekeeper.TIME <= 10.0f) {
            FieldProperties.BLUE_LEFT_LIGHTS.active = false;
            FieldProperties.BLUE_RIGHT_LIGHTS.active = true;
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
        }
    }

}