using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTextChangeScript : MonoBehaviour
{
    public GameObject screen;
    public AudioClip[] audioList;

    protected Text screenText;
    protected AudioSource audioComp;
    protected int dialogueNum;
    protected bool intro;

    protected string[] IntroDialog = {
        "Hmmm, finally awake I see…  ",
        "Rejoice!",
        "You have been personally chosen by I, the brilliant professor Moriatz to become a superior mind of this world!",
        "Resolve the puzzles, get out of the basement and you will be ensured a promising future.",
        "But do not waste my time or you can stay here forever."//,
        //"Hurry up, the clock is already ticking…"
        };

    // Start is called before the first frame update
    void Start()
    {
        dialogueNum = 0;
        screenText = screen.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        screenText.text = IntroDialog[dialogueNum];
        audioComp = screen.GetComponent<AudioSource>();
        intro = true;
        audioComp.PlayDelayed(3.0f);
        Activation();
        Invoke("Activation", 2.9f);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueNum < IntroDialog.Length-1)
        {
            if (intro & !audioComp.isPlaying)
            {
                dialogueNum += 1;
                NextDialogue(dialogueNum);
                ChangeAudio(dialogueNum);
            }
        }
        else
        {

            intro = false;
        }
    }

    public void NextDialogue(int num) 
    {
        screenText.text = IntroDialog[num];
    }

    public void ChangeAudio(int num)
    {
        audioComp.clip = audioList[num];
        audioComp.Play();
    }

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

    public bool GetIntroStatus()
    {
        return intro;
    }
}
