using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTutorialTextScript : MonoBehaviour
{
    //scene gameObjects
    private Text txt;
    private GameObject characterController;
    private int missionNumber;
    private GameObject tutorialScreen;
    private AudioSource missionClearSound;
    private GameObject puzzle1Clipboard;
    private float clipboardPos;
    private Material lightMaterial;
    private GameObject doorLock;
    private bool a;

    private string[] missionTextArray = { 
        //1
        "Mission:\nMove near the lamp.\n- Press button A and throw the ball.\n" +
            "- Press Button B to teleport to the ball or Button C to cancel.",
        //2
        "Mission:\n It is too dark to see anything, find a way to light up the main light.\nThere must be a hint somewhere...\n" +
            "- Grab the clipboard on the drawer by pushing the D button",
        //3
        "Mission:\nThis must be a hint to light up the lights.\n" +
            "- Find the right switches to activate. ",
        //4
        "Mission:\nFind the key to open the door",
        //5
        "Mission:\nGo in the next room"
            };

    // Start is called before the first frame update
    void Start()
    {
        //Gets the differents GameComponents from the scene
        txt = GameObject.Find("MissionText").GetComponent<Text>();
        characterController = GameObject.Find("OVRPlayerController");
        tutorialScreen = GameObject.Find("TutorialUIController/TutorialScreen");
        missionClearSound = gameObject.GetComponent<AudioSource>(); 
        puzzle1Clipboard = GameObject.Find("EscapeRoom/Room1/Clipboard");
        clipboardPos = puzzle1Clipboard.transform.position[1];
        lightMaterial = GameObject.Find("EscapeRoom/Room1/MainLight1/Large_round_lamp").gameObject.GetComponent<Renderer>().material;
        doorLock = GameObject.Find("EscapeRoom/WallBetweenRooms/DoorV6/01_low/prf_lock_01");

        //Set the first mission on the screen
        txt.text = missionTextArray[0];
        missionNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Go to next mission if the conditions for the current mission is fullfilled
        if (missionNumber == 1 & characterController.transform.position[2] < -6)
        {
            //disactivates the tutorial video
            tutorialScreen.SetActive(false);
            NextMission();
        }
        else if (missionNumber == 2 & puzzle1Clipboard.transform.position[1] > clipboardPos + 0.1)
        {
            NextMission();
        }
        else if (missionNumber == 3 & lightMaterial.IsKeywordEnabled("_EMISSION"))
        {
            NextMission();
        }
        else if (missionNumber == 4 & !doorLock.GetComponent<Lock>().is_locked)
        {
            NextMission();
        }
    }


    public void NextMission()
    {
        //Plays clear sound and changes of the next mission 
        txt.text = missionTextArray[missionNumber];
        missionClearSound.Play();
        missionNumber += 1;
    }
}
