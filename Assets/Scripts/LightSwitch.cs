using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : ObjectAnchor
{
    public bool is_on = false;
    public string controlled_light = "MainLight1";
    public AudioSource sound;

    private Vector3 last_position; // store last position of the hand controller
    private Transform trans;

    private float avaliable_rotation_to_on;// set the rotation range to 60f

    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        if (is_on)
        {
            avaliable_rotation_to_on = 0f; // corresponding rotation to off state is 70f
        }
        else
        {
            avaliable_rotation_to_on = 70f;
        }
    }

    // Update is called once per frame
    protected void Update()
    {

        if (this.hand_controller != null)
        {
            //check if the hand controller is far away from the switch, detach
            if (Vector3.Distance(this.hand_controller.transform.position, trans.position) > this.graspingRadius * 2)
            {
                this.detach_from(this.hand_controller);
                //Debug.LogWarning("detached!!!");
                return;
            }

            if (last_position != null)
            {
                float diff = (this.hand_controller.transform.position.y - last_position.y) * Mathf.Rad2Deg;// consider the change in y axis as the rad change, calculate degree
                float to_update;

                //try to turn off
                if(diff < 0)
                {
                    to_update = Mathf.Min(70f - avaliable_rotation_to_on, Mathf.Abs(5f * diff));
                    avaliable_rotation_to_on = avaliable_rotation_to_on + to_update;
                    to_update = to_update * -1f;
                    if(avaliable_rotation_to_on == 70f)
                    {
                        sound.Play();
                        this.is_on = false;
                    }
                }
                else
                {
                    to_update = Mathf.Min(avaliable_rotation_to_on, 5f * diff);
                    avaliable_rotation_to_on = avaliable_rotation_to_on - to_update;
                    if (avaliable_rotation_to_on == 0f)
                    {
                        sound.Play();
                        this.is_on = true;
                    }
                }
                trans.Rotate(new Vector3(0f, to_update, 0f));
            }
            last_position = this.hand_controller.transform.position;
        }
    }

    public override void attach_to(HandController hand_controller)
    {
        // Store the hand controller in memory
        this.hand_controller = hand_controller;
    }

}
