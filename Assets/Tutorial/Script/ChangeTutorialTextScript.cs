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
    public GameObject large_round_lamp;
    public GameObject doorLock;
    public GameObject screenController;
    public SafeDoor control_door;
    public GameObject safe;
    public GameObject book;

    private GameObject tutorialScreen;
    private AudioSource missionClearSound;
    private Material lightMaterial;
    private Text txt;
    private float clipboardPos, bookPos;
    private int missionNumber;

    //text for each mission
    private string[] missionTextArray = { 
        //1
        "Mission:\n\nMove near the lamp.\n- Press button A and throw the ball.\n" +
            "- Press button B to teleport to the ball or button C to cancel.\n\n\n\n"+
            "Press X to show/hide the mission window.",
        //2
        "Mission:\n\nIt is too dark to see anything, find a way to light up the main light.\nThere must be a hint somewhere...\n" +
            "- Grab the clipboard on the drawer by pushing the D button.",
        //3
        "Mission:\n\nThis must be a hint to light up the lights.\n" +
            "- Find the right switches to activate. ",
        //4
        "Mission:\n\nExplore the room and find the key to open the door.",
        //5
        "Mission:\n\nThis safe might contain the key, find a way to open it.\n"+
            "- Press LY to open/close the notepad to take a memo"+
            "- Hold B while writing the code on it with the right controller"+
            "- The password must be hidden somewhere in the room.",
        //6
        "Mission:\n\nThis number written on the book has to be the clue you were searching.",
        //7
        "Mission:\n\nUnlock the door and go in the next room.",
        //8
        "Mission:\n\nFind a way out."
            };

    // Start is called before the first frame update
    void Start()
    {
        //Gets the differents GameComponents from the scene
        txt = gameObject.transform.GetChild(2).GetComponent<Text>();
        tutorialScreen = gameObject.transform.GetChild(1).gameObject;
        missionClearSound = gameObject.GetComponent<AudioSource>(); 
        clipboardPos = puzzle1Clipboard.transform.position[1];
        bookPos = book.transform.position[1];
        lightMaterial = large_round_lamp.gameObject.GetComponent<Renderer>().material;

        //Set the first mission on the screen
        txt.text = missionTextArray[0];
        missionNumber = 1;
        //desactivate the display until end of intro
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Activates the mission window if the intro is finished
        if (!this.gameObject.transform.GetChild(2).gameObject.activeSelf && !screenController.GetComponent<ScreenTextChangeScript>().GetIntroStatus())
        {
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }

        //Go to next mission if the conditions for the current mission is fullfilled
        //2
        if (missionNumber == 1 && characterController.transform.position[2] < -6)
        {
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(6);
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
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(7);
            NextMission();
        }//5
        else if (missionNumber == 4 && NearObject(safe))
        {
            NextMission();
        }//6
        else if (missionNumber == 5 && book.transform.position[1] > bookPos + 0.1)
        {
            NextMission();
        }//7
        else if (missionNumber == 6 && !control_door.locked)
        {
            NextMission();
        }//8
        else if (missionNumber == 7 && characterController.transform.position[2] > 0.5)
        {
            screenController.GetComponent<ScreenTextChangeScript>().ChangeAudio(8);
            NextMission();
        }        
    }

    //show/hide the mission window if button X is pushed
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

    //activates the mission window and displays the next mission
    public void NextMission()
    {
        txt.gameObject.SetActive(true);
        //Plays clear sound and changes of the next mission 
        txt.text = missionTextArray[missionNumber];
        missionClearSound.Play();
        missionNumber += 1;
    }

    //calculate the distance between the player and an object
    protected bool NearObject(GameObject obj)
    {
        float a = (obj.transform.position[0] - characterController.transform.position[0]) + (obj.transform.position[2] - characterController.transform.position[2]);
        if (Mathf.Abs(obj.transform.position[0] - characterController.transform.position[0])<2 & Mathf.Abs(obj.transform.position[2] - characterController.transform.position[2])<2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
