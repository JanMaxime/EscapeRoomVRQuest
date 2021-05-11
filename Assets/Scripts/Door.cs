using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string door_name = "Door1"; //all doors are locked initially
    public AudioSource sound;
    public Lock correspond_lock;
    protected bool can_be_opened = false;
    protected float avaliable_degree_to_open = 100f;

    private Vector3 last_position_of_hand;
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        //rb.AddForce(0f, 0f, 1f, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        if (correspond_lock.is_locked == false)
        { 
            can_be_opened = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        HandController hand_controller = other.GetComponent<HandController>();
        //Debug.LogWarning(hand_controller);
        if (hand_controller == null) return;

        last_position_of_hand = hand_controller.transform.position;
        bool right = (hand_controller.handType == HandController.HandType.RightHand);
        if (can_be_opened)
        {
            //sound.Play();
            //StartCoroutine(sound.Play());
            StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1f, 1f));
        }
    }

    void OnTriggerStay(Collider other)
    {
        HandController hand_controller = other.GetComponent<HandController>(); // use hand controller to push the door

        if (hand_controller == null) return;

        if (can_be_opened == true)
        {
            //get direction
            Vector3 relativePosition = transform.InverseTransformPoint(hand_controller.transform.position);
            float diff = Vector3.Distance(hand_controller.transform.position, last_position_of_hand) * Mathf.Rad2Deg;// consider the change in distance
            float to_update = 0f;

            if (relativePosition.x > 0)
            {
                //Debug.LogWarning("Close Door!!");
                to_update = Mathf.Min(100f-avaliable_degree_to_open, 1f * diff);
                avaliable_degree_to_open = avaliable_degree_to_open + to_update;
                to_update = to_update * -1f;
            }
            else
            {
                //Debug.LogWarning("Open Door!!");
                to_update = Mathf.Min(avaliable_degree_to_open, 1f * diff);
                avaliable_degree_to_open = avaliable_degree_to_open - to_update;
            }


            transform.Rotate(new Vector3(0f, 0f, to_update));
            //rb.AddForce(0f, 0f, 1f, ForceMode.Impulse);
        }
    }
}
