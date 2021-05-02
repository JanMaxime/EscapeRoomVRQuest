using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandleAnchor : ObjectAnchor
{

    public GameObject anchored_object;

    public Transform trackingSpace;
    public Vector3 rotation;

    protected Transform anchored_object_parent;

    void Start () {
		initial_transform_parent = transform.parent;
        anchored_object_parent = anchored_object.transform.parent;
	}


    public override void attach_to ( HandController hand_controller ) {
        //base.attach_to(hand_controller);
        this.hand_controller = hand_controller;
        anchored_object.transform.SetParent(hand_controller.transform);

        //THIS NEEDS TO BE CORRECTED
        if(hand_controller.handType == HandController.HandType.RightHand){
            anchored_object.transform.rotation = trackingSpace.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        }
        else{
            anchored_object.transform.rotation = trackingSpace.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.LHand);
        }

        disable_rigidbody(anchored_object);

    }

        public override void detach_from ( HandController hand_controller ) {
        anchored_object.transform.SetParent(anchored_object_parent);
        activate_rigidbody(anchored_object);
        this.hand_controller = null;
        //base.detach_from(hand_controller);
	}
}
