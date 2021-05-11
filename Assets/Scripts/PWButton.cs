using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PWButton : MonoBehaviour
{
    public Text screen_text;

    protected string button_text;
    // Use this for initialization
    void Start()
    {
        button_text = gameObject.name; //name of the button is set to the number!!
    }

    void OnTriggerEnter(Collider other) //if a collider coming, check if the collier is key, if their names correspond to the same door
    {
        HandController hand_controller = other.GetComponent<HandController>();
        //Debug.LogWarning(hand_controller);
        if (hand_controller == null) return;

        bool right = (hand_controller.handType == HandController.HandType.RightHand);
        StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1f, 1f));

        //Debug.LogWarning(button_text);

        //add the string to the text in the screen
        screen_text.text += button_text;
    }
}
