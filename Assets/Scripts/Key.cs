using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ObjectAnchor
{
    // keys need to be collider
    public string corresponding_door = "Door1"; //Door1 is the door from room1 to room2
    Collider box_collider;

    // gameobject in which the key is hidden, this is to make sure the key can only be grabbed if the safe/floating cube is hacked
    public ContainerBeHacked related_container; 
    protected bool can_be_grabbed = false;
    private int count = 1;

    void Start()
    {
        box_collider = gameObject.GetComponent<Collider>();
        box_collider.isTrigger = false;
    }

    void Update()
    {
        if (this.is_available())
        {
            box_collider.isTrigger = false; // uncheck the trigger for collider otherwise object will pass through collider
        }
        else
        {
            box_collider.isTrigger = true;
        }
        if (related_container.isHacked() && count==1)
        {
            count += 1; // avoid continuous set of isKinematic
            can_be_grabbed = true;
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.isKinematic = false; //If isKinematic is enabled, Forces, collisions or joints will not affect the rigidbody anymore.
            }

        }
    }

    public override void attach_to(HandController hand_controller)
    {
        if (can_be_grabbed) // check if the key can be grabbed
        {
            // Store the hand controller in memory
            this.hand_controller = hand_controller;
            
            // Set the object to be placed in the hand controller referential
            transform.SetParent(hand_controller.transform);

            // Set the object to be placed in the hand controller referential
            disable_rigidbody(this.gameObject);
        }
    }
}
