using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandleAnchor : ObjectAnchor
{

    public GameObject anchored_object;

    protected Transform anchored_object_parent;


    /// <summary>
    /// At the start of the game, the initial parent of the the handle and of the whole objects are being stored in parameters
    /// </summary>
    void Start()
    {
        initial_transform_parent = transform.parent;
        anchored_object_parent = anchored_object.transform.parent;
    }

    /// <summary>
    /// Attaches the object to the hand controller passed as parameter
    /// </summary>
    /// <param name="hand_controller">The hand to attach the object to</param>
    public override void attach_to(HandController hand_controller)
    {
        this.hand_controller = hand_controller;
        anchored_object.transform.SetParent(hand_controller.transform);
        disable_rigidbody(anchored_object);
    }

    /// <summary>
    /// Detaches the object from the hand controller passed as parameter
    /// </summary>
    /// <param name="hand_controller">The hand to detach the object from</param>
    public override void detach_from(HandController hand_controller)
    {
        anchored_object.transform.SetParent(anchored_object_parent);
        activate_rigidbody(anchored_object);
        this.hand_controller = null;
    }
}
