using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTextChangeScript : MonoBehaviour
{
    //get scene GameObjects
    public GameObject screen;
    public AudioClip[] audioList;

    public GameObject characterController;
    public GameObject safeObject;
    public GameObject cubeObject;
    public GameObject glassObject;
    public GameObject lightsaber;

    protected Text screenText;
    protected AudioSource audioComp;
    protected int dialogueNum;
    protected bool intro = true, safeBool = true, cubeBool = true, glassBool = true, saberBool = true;

    //dialogues for the intro
    protected string[] dialogueArray = {
        "Hmmm, finally awake I see�  ",
        "Rejoice!",
        "You have been personally chosen by I, the brilliant professor Moriatz to become a superior mind of this world!",
        "Resolve the puzzles, get out of the basement and you will be ensured a promising future.",
        "But do not waste my time or you can stay here forever.",
        "Hurry up, the clock is already ticking�",
        //6
        "Humpf, attracted by light like a mosquito. You can�t even see in the dark, what a disappointment.",
        //7
        "Maybe you have some potential in the end. You just turn on the light, but the door is still closed. You will never find the key� the way out I mean.",
        //8
        "Not too shabby� But this was too easy, let�s see how you do in the next room.",
        //9
        "In this safe is my best invention, something that ignores even the laws of the universe! But it�s locked with a password too bad for you.",
        //10
        "This is a nice cube isn�t it, made in Norissium. Nothing special, just the most solid material that I created.",
        //11
        "Aren�t you intrigued ?`These metallic cubes are see heavy that no human can possibly lift them. It�s not like you could touch them anyway.",
        //12
        "So you finally find my lovely lightsaber! One swing and it disintegrates anything. Very useful to do some cleaning."
        };

    // Start is called before the first frame update
    void Start()
    {
        dialogueNum = 0;
        screenText = screen.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        screenText.text = dialogueArray[dialogueNum];
        audioComp = screen.GetComponent<AudioSource>();

        //play the audio and screen 3 sec after awake
        audioComp.PlayDelayed(3.0f);
        Activation();
        Invoke("Activation", 2.9f);
    }

    // Update is called once per frame
    void Update()
    {
        //plays the next dialogue of the introduction
        if (dialogueNum < 5)
        {
            if (intro & !audioComp.isPlaying)
            {
                dialogueNum += 1;
                ChangeAudio(dialogueNum);
            }
        }
        else
        {
            //when introduction is finished
            intro = false;
        }

        //voiceslines trigger
        //near the safe
        if (safeBool && NearObject(safeObject))
        {
            ChangeAudio(9);
            safeBool = false;
        }
        //near the cube to break
        if (cubeBool && NearObject(cubeObject))
        {
            ChangeAudio(10);
            cubeBool = false;
        }
        //near the glass panel
        if (glassBool && NearObject(glassObject))
        {
            ChangeAudio(11);
            glassBool = false;
        }
        if (saberBool && (lightsaber.gameObject.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.None))
        {
            ChangeAudio(12);
            saberBool = false;
        }



    }

    //change text of dialogue
    public void NextDialogue(int num) 
    {
        screenText.text = dialogueArray[num];
    }

    //change audio of dialogue
    public void ChangeAudio(int num)
    {
        NextDialogue(num);
        audioComp.clip = audioList[num];
        audioComp.Play();
    }

    // on/off for the tv screen
    public void Activation()
    {
        if (screen.GetComponent<Renderer>().material.IsKeywordEnabled("_EMISSION"))
        {
            screen.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            screenText.gameObject.SetActive(false);
        }
        else
        {
            screen.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            screenText.gameObject.SetActive(true);
        }
    }

    //get bool if intro is finished
    public bool GetIntroStatus()
    {
        return intro;
    }

    //calculate the distance between the player and an object
    protected bool NearObject(GameObject obj)
    {
        float a = (obj.transform.position[0] - characterController.transform.position[0]) + (obj.transform.position[2] - characterController.transform.position[2]);
        if (Mathf.Abs(obj.transform.position[0] - characterController.transform.position[0]) < 1.5 & Mathf.Abs(obj.transform.position[2] - characterController.transform.position[2]) < 1.5)
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
