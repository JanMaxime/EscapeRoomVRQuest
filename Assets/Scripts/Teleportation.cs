using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{


    [Header("Player Transform")]
    public Transform playerTransform;

    [Header("Teleportation sphere")]
    public ObjectAnchor teleportation_sphere;

    [Range(1f, 5f)]
    public float rotation_speed = 2f;
    protected ObjectAnchor teleportation_sphere_instantiated;

    [Header("Tracking space")]
    public Transform trackingSpace;

    bool sphere_thrown = false;

    protected bool tp_sphere_button_pushed_previous_frame = false;

    public CharacterController characterController;
    protected bool is_tp_sphere_button_pushed()
    {
        return OVRInput.Get(OVRInput.RawButton.A);
    }

    protected bool is_tp_activated_button_pushed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.X);
    }

    protected bool is_tp_canceled_button_pushed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.B);
    }

    // Update is called once per frame
    void Update()
    {
        handle_tp_behavior();
    }

    protected void handle_tp_behavior()
    {

        bool tp_sphere_button_pushed = is_tp_sphere_button_pushed();
        bool tp_activated_button_pushed = is_tp_activated_button_pushed();
        bool tp_canceled_button_pushed = is_tp_canceled_button_pushed();
        //if ( tp_sphere_button_pushed == tp_sphere_button_pushed_previous_frame) return;

        tp_sphere_button_pushed_previous_frame = tp_sphere_button_pushed;



        //If there are no TP sphere in game, instantiate one
        if (tp_sphere_button_pushed && teleportation_sphere_instantiated == null && !sphere_thrown)
        {
            teleportation_sphere_instantiated = GameObject.Instantiate(teleportation_sphere, this.transform.position, this.transform.rotation);
            teleportation_sphere_instantiated.attach_to(GetComponent<HandController>());
            Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        //If the user releases the button, throw the sphere
        else if (!tp_sphere_button_pushed && teleportation_sphere_instantiated != null && !sphere_thrown)
        {
            teleportation_sphere_instantiated.detach();
            Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            //From local coord to world coor.
            rigidbody.velocity = trackingSpace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * 1.5f;
            teleportation_sphere_instantiated.transform.eulerAngles = new Vector3(0, teleportation_sphere_instantiated.transform.eulerAngles.y, 0);
            sphere_thrown = true;
        }

        if (sphere_thrown)
        {

            teleportation_sphere_instantiated.transform.RotateAround(teleportation_sphere_instantiated.transform.position, Vector2.up, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x * rotation_speed);

            if (tp_activated_button_pushed)
            {
                characterController.Move(teleportation_sphere_instantiated.transform.position - playerTransform.position);
                characterController.transform.eulerAngles = new Vector3(0f, teleportation_sphere_instantiated.transform.eulerAngles.y, 0f);
            }

            if (tp_activated_button_pushed || tp_canceled_button_pushed)
            {
                GameObject.DestroyImmediate(teleportation_sphere_instantiated.gameObject);
                teleportation_sphere_instantiated = null;
                sphere_thrown = false;
            }
        }




    }
}
