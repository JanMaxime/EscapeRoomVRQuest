using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGrabber : MonoBehaviour
{

    [Header("Luminous ray")]
    public GameObject luminous_ray;

    [Header("Canon")]
    //Position of the gun's canon
    public Transform canon;

    protected GameObject luminous_ray_instantiated;

    private HandController handController;
    
    private bool triggered_last_frame = false;

    private Transform parent_transform;


    void Start()
    {
        parent_transform = this.GetComponentInParent<Transform>();
    }

    /// <summary>
    /// Checks whether the trigger of the gun is pressed or not
    /// </summary>
    /// <returns></returns>
    private bool is_trigger_pressed(){
	if ( handController.handType == HandController.HandType.LeftHand )
        return OVRInput.Get( OVRInput.Axis1D.PrimaryHandTrigger ) > 0.5;   // Check that the middle finger is pressing


		// Case of a right hand
	else
        return OVRInput.Get( OVRInput.Axis1D.SecondaryHandTrigger ) > 0.5; // Check that the middle finger is pressing
    }

    // Update is called once per frame
    void Update()
    {
        this.handController = GetComponent<ObjectHandleAnchor>().hand_controller;
        //If the gun is not grabbed, we destroy the ray if it was triggered in the last frame and then stops the update method
        if(handController == null){
            if (triggered_last_frame){
                GameObject.DestroyImmediate(luminous_ray_instantiated.gameObject);
            }
            return;
        }

        bool trigger_pressed = is_trigger_pressed();

        //Handles the behaviour of the luminous ray
        if(trigger_pressed){

            //A ray is cast to find the object hit by the ray
            RaycastHit hit;
            if(Physics.Raycast(canon.position, -parent_transform.right, out hit, Mathf.Infinity)){
                //If it is the first frame of triggering, the ray is created
                if(!triggered_last_frame){
                    luminous_ray_instantiated = GameObject.Instantiate(luminous_ray, canon.position, parent_transform.rotation);
                    luminous_ray_instantiated.transform.eulerAngles = new Vector3(luminous_ray_instantiated.transform.eulerAngles.x, luminous_ray_instantiated.transform.eulerAngles.y, luminous_ray_instantiated.transform.eulerAngles.z+90);  
                }

                //The ray is scaled to fit exactly between the gun and the object hit
                luminous_ray_instantiated.transform.SetParent(null);
                luminous_ray_instantiated.transform.position = canon.position - transform.right * hit.distance /2;
                luminous_ray_instantiated.transform.localScale = new Vector3(luminous_ray_instantiated.transform.localScale.x, hit.distance/1.9f , luminous_ray_instantiated.transform.localScale.z);
                luminous_ray_instantiated.transform.SetParent(parent_transform);

                //If the laser hits an object that can be lifted using the magnetic gun
                if(hit.rigidbody && hit.rigidbody.tag == "LaserGrabbable"){
                    OVRInput.Controller controller = handController.handType == HandController.HandType.RightHand ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;

                    //The lateral speed of the cube is computed using the angular speed of the controller
                    float lateralSpeed = - (handController.trackingSpace.rotation* OVRInput.GetLocalControllerAngularVelocity(controller) * 2f).y;
                    //This speed is applied as well as a constant speed on the y axis
                    hit.rigidbody.velocity = new Vector3(lateralSpeed, 0.75f, 0);
                    //A strong vibration is started
                    StartCoroutine(VibrationManager.vibrate(handController.handType == HandController.HandType.RightHand, 0.05f, 0.9f, 0.9f));
                }
                //If the ray hit another object, a very light vibration is started
                else{
                    StartCoroutine(VibrationManager.vibrate(handController.handType == HandController.HandType.RightHand, 0.05f, 0.15f, 0.15f));
                }
            }
        }
        //Destroy the ray when the player stops pressing the trigger
        if(triggered_last_frame && !trigger_pressed){
            GameObject.DestroyImmediate(luminous_ray_instantiated.gameObject);
        }

        triggered_last_frame = trigger_pressed;
    }
}
