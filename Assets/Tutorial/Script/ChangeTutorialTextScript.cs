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
    public GameObject characterController;
    public GameObject puzzle1Clipboard;
    public GameObject Large_round_lamp;
    public GameObject doorLock;
    public GameObject screenController;
    public GameObject safeObject;
    public GameObject cubeObject;
    public GameObject glassObject;

    private GameObject tutorialScreen;
    private AudioSource missionClearSound;
    private Material lightMaterial;
    private Text txt;
    private float clipboardPos;
    private int missionNumber;
    private bool safeBool = true, cubeBool = true, glassBool = true;

    private string[] missionTextArray = { 
        //1
        "Mission:\nMove near the lamp.\n- Press button A and throw the ball.\n" +
            "- Press button B to teleport to the ball or button C to cancel.\n\n\n\n"+
            "Press X to show/hide the mission window",
        //2
        "Mission:\n It is too dark to see anything, find a way to light up the main light.\nThere must be a hint somewhere...\n" +
            "- Grab the clipboard on the drawer by pushing the D button",
        //3
        "Mission:\nThis must be a hint to light up the lights.\n" +
            "- Find the right switches to activate. ",
        //4
        "Mission:\nFind the key to open the door",
        //5
        "Mission:\nGo in the next room",
        //6
        "Mission:\nFind a way out"
            };

    // Start is called before the first frame update
    void Start()
    {
        //Gets the differents GameComponents from the scene
        txt = gameObject.transform.GetChild(2).GetComponent<Text>();
        tutorialScreen = gameObject.transform.GetChild(1).gameObject;
        missionClearSound = gameObject.GetComponent<AudioSource>(); 
        clipboardPos = puzzle1Clipboard.transform.position[1];
        lightMaterial = Large_round_lamp.gameObject.GetComponent<Renderer>().material;

        //Set the first mission on the screen
        txt.text = missionTextArray[0];
        missionNumber = 1;
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        UnityEngine.Debug.Log(safeBool);// safeObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (!this.gameObject.transform.GetChild(2).gameObject.activeSelf && !screenController.GetComponent<ScreenTextChangeScript>().GetIntroStatus())
        {

            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        //Go to next mission if the conditions for the current mission is fullfilled
        //2
        if (missionNumber == 1 && characterController.transform.position[2] < -6)
        {
            //disactivates the tutorial video
            tutorialScreen.SetActive(false);
            NextMission();
        }//3
        else if (missionNumber == 2 && puzzle1Clipboard.transform.position[1] > clipboardPos + 0.1)
        {
            NextMission();
        }//4
        else if (missionNumber == 3 && lightMaterial.IsKeywordEnabled("_EMISSION"))
        {
            NextMission();
        }//5
        else if (missionNumber == 4 && !doorLock.GetComponent<Lock>().is_locked)
        {
            NextMission();
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(3);
        }//5
        else if (missionNumber == 5 && characterController.transform.position[2]>0.5)
        {
            NextMission();
        }

        //voices trigger
        if (safeBool && NearObject(safeObject))
        {
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(0);
            safeBool = false;
        }
        if (cubeBool && NearObject(cubeObject))
        {
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(1);
            cubeBool = false;
        }
        if (glassBool && NearObject(glassObject))
        {
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(2);
            glassBool = false;
        }
        
    }

    private void LateUpdate()
    {

        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            if (txt.gameObject.activeSelf)
            {
                txt.gameObject.SetActive(false);
            }
            else
            {
                txt.gameObject.SetActive(true);
            }
        }
    }

    public void NextMission()
    {
        txt.gameObject.SetActive(true);
        //Plays clear sound and changes of the next mission 
        txt.text = missionTextArray[missionNumber];
        missionClearSound.Play();
        missionNumber += 1;
    }

    protected bool NearObject(GameObject obj)
    {
        float a = (obj.transform.position[0] - characterController.transform.position[0]) + (obj.transform.position[2] - characterController.transform.position[2]);
        if (Mathf.Abs(obj.transform.position[0] - characterController.transform.position[0])<1.5 & Mathf.Abs(obj.transform.position[2] - characterController.transform.position[2])<1.5)
        {

            UnityEngine.Debug.Log(safeBool); 
            return true;
        }
        else
        {
            return false;
        }
    }

}
