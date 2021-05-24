using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string door_name = "Door1"; //all doors are locked initially
    public Lock correspond_lock;
    protected bool can_be_opened = false;
    protected float avaliable_degree_to_open = 100f;

    protected float velocity;
    
    // Update is called once per frame
    void Update()
    {
        if (correspond_lock.is_locked == false)
        { 
            can_be_opened = true; // can be open only if corresponding lock is unlocked
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // the player need to push the door with hands to open it
        HandController hand_controller = other.GetComponent<HandController>();
        if (hand_controller == null) return;

        bool right = (hand_controller.handType == HandController.HandType.RightHand);
        if (can_be_opened)
        {
            StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1f, 1f)); // vibrate to simulate the player push open a heavy old door
            // get the velocity of the controller, the higher the speed is, the larger degree the door will rotate
            if (right)
            {
                velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
            }
            else
            {
                velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude;
            }

            //get direction to decide if the door is pushed open or pushed close
            Vector3 relativePosition = transform.InverseTransformPoint(hand_controller.transform.position);
            float to_update = 0f;

            if (relativePosition.x > 0) //close the door
            {
                to_update = Mathf.Min(100f - avaliable_degree_to_open, 10f * velocity);
                avaliable_degree_to_open = avaliable_degree_to_open + to_update;
                to_update = to_update * -1f;
            }
            else // open the door
            {
                to_update = Mathf.Min(avaliable_degree_to_open, 10f * velocity); // to make sure the door can only be opened at max 100 degree
                avaliable_degree_to_open = avaliable_degree_to_open - to_update;
            }


            transform.Rotate(new Vector3(0f, 0f, to_update));
        }
    }

    public bool isOpen()
    {
        return can_be_opened;
    }
}
