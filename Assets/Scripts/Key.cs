using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ObjectAnchor
{
    // keys need to be collider
    public string corresponding_door = "Door1"; //Door1 is the door from room1 to room2
    Collider box_collider;

    public ContainerBeHacked related_container;
    protected bool can_be_grabbed = false;
    protected Rigidbody rigidbody;

    void Start()
    {
        box_collider = gameObject.GetComponent<Collider>();
        box_collider.isTrigger = false;
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (this.is_available())
        {
            box_collider.isTrigger = false;
        }
        else
        {
            box_collider.isTrigger = true;
        }
        if (related_container.isHacked())
        {
            can_be_grabbed = true;
            rigidbody.useGravity = true;
        }
    }

    public override void attach_to(HandController hand_controller)
    {
        if (can_be_grabbed)
        {
            // Store the hand controller in memory
            this.hand_controller = hand_controller;

            // Set the object to be placed in the hand controller referential
            transform.SetParent(hand_controller.transform);

            disable_rigidbody(this.gameObject);
        }
    }
}
