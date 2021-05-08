using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject handle;

    void OnCollisionEnter ( Collision collision ) {

        if(collision.rigidbody == null) return;
		HittableBox hittableBox = collision.rigidbody.GetComponent<HittableBox>();
		if ( hittableBox == null ) return;

        if (this.handle.GetComponent<ObjectHandleAnchor>().hand_controller.handType == HandController.HandType.RightHand){
            hittableBox.hit(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude + Vector3.Distance(handle.transform.position, collision.GetContact(0).point) * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude, true);    
        }
        else{
            hittableBox.hit( OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude + Vector3.Distance(handle.transform.position, collision.GetContact(0).point) * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude, false);    
        }
		
	}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
