using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser handle")]
    public GameObject handle;

    /// <summary>
    /// Handles the behaviour when the laser hit an object and apply damages to the hittable cube if it is indeed such a cube in collision
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        //If the object does not have a rigidbody and is not a hittable box, do nothing
        if (collision.rigidbody == null) return;
        HittableBox hittableBox = collision.rigidbody.GetComponent<HittableBox>();
        if (hittableBox == null) return;

        //Apply damages to the cube with the correct strength of the hand that holds the handle
        if (this.handle.GetComponent<ObjectHandleAnchor>().hand_controller.handType == HandController.HandType.RightHand)
        {
            hittableBox.hit(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude + Vector3.Distance(handle.transform.position, collision.GetContact(0).point) * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude, true);
        }
        else
        {
            hittableBox.hit(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude + Vector3.Distance(handle.transform.position, collision.GetContact(0).point) * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude, false);
        }

    }
}
