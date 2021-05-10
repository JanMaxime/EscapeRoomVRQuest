using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGrabber : MonoBehaviour
{

    public GameObject luminous_ray;

    protected GameObject luminous_ray_instantiated;

    private HandController handController;
    

    private bool triggered_last_frame = false;

    private Transform parent_transform;

    public Transform canon;

    void Start()
    {
        parent_transform = this.GetComponentInParent<Transform>();

        
    }

    private bool is_trigger_pressed(){
	if ( handController.handType == HandController.HandType.LeftHand )
        return OVRInput.Get( OVRInput.Axis1D.PrimaryHandTrigger ) > 0.5;   // Check that the index finger is pressing


		// Case of a right hand
	else
        return OVRInput.Get( OVRInput.Axis1D.SecondaryHandTrigger ) > 0.5; // Check that the index finger is pressing
    }

    // Update is called once per frame
    void Update()
    {
        this.handController = GetComponent<ObjectHandleAnchor>().hand_controller;
        if(handController == null){
            if (triggered_last_frame){
                GameObject.DestroyImmediate(luminous_ray_instantiated.gameObject);
            }
            return;
        }
        bool trigger_pressed = is_trigger_pressed();
        if(trigger_pressed){

            RaycastHit hit;
            if(Physics.Raycast(canon.position, -parent_transform.right, out hit, Mathf.Infinity)){
                if(!triggered_last_frame){
                    luminous_ray_instantiated = GameObject.Instantiate(luminous_ray, canon.position, parent_transform.rotation);
                    luminous_ray_instantiated.transform.eulerAngles = new Vector3(luminous_ray_instantiated.transform.eulerAngles.x, luminous_ray_instantiated.transform.eulerAngles.y, luminous_ray_instantiated.transform.eulerAngles.z+90);  
                }
                luminous_ray_instantiated.transform.SetParent(null);
                luminous_ray_instantiated.transform.position = canon.position - transform.right * hit.distance /2;
                luminous_ray_instantiated.transform.localScale = new Vector3(luminous_ray_instantiated.transform.localScale.x, hit.distance/1.9f , luminous_ray_instantiated.transform.localScale.z);
                luminous_ray_instantiated.transform.SetParent(parent_transform);
                if(hit.rigidbody && hit.rigidbody.tag == "LaserGrabbable"){
                    OVRInput.Controller controller = handController.handType == HandController.HandType.RightHand ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;
                    float lateralSpeed = - (handController.trackingSpace.rotation* OVRInput.GetLocalControllerAngularVelocity(controller) * 2f).y;
                    hit.rigidbody.velocity = new Vector3(lateralSpeed, 0.5f, 0);
                    StartCoroutine(VibrationManager.vibrate(handController.handType == HandController.HandType.RightHand, 0.1f, 0.7f, 0.7f));
                }
                else{
                    StartCoroutine(VibrationManager.vibrate(handController.handType == HandController.HandType.RightHand, 0.1f, 0.2f, 0.2f));
                }
            }
        }

        if(triggered_last_frame && !trigger_pressed){
            GameObject.DestroyImmediate(luminous_ray_instantiated.gameObject);
        }

        triggered_last_frame = trigger_pressed;
    }
}
