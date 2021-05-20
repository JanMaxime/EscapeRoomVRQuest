using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterButton : MonoBehaviour
{
    public Text screen_text;
    public SafeDoor control_door;
    public string correct_pw;

    public AudioSource wrong_sound;
    public AudioSource unlock_sound;

    void OnTriggerEnter(Collider other) //if a collider coming, check if the collier is key, if their names correspond to the same door
    {
        HandController hand_controller = other.GetComponent<HandController>();
        //Debug.LogWarning(hand_controller);
        if (hand_controller == null) return;

        bool right = (hand_controller.handType == HandController.HandType.RightHand);
        StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1f, 1f));

        if(screen_text.text != correct_pw)
        {
            //wrong pw!
            wrong_sound.Play();
            screen_text.text = "";
        }
        else
        {
            unlock_sound.Play();

            //controlled door open
            //door open: position x = 3
            control_door.locked = false;
        }
    }
}
