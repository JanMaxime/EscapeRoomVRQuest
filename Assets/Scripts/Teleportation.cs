using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{


    [Header("Player information")]
    //The transform where the player stands
    public Transform playerTransform;
    public Transform trackingSpace;
    public CharacterController characterController;

    [Header("Teleportation sphere")]
    //Prefab of the teleporation sphere
    public ObjectAnchor teleportation_sphere;

    [Range(1f, 5f)]
    //Speed at which the sphere rotates
    public float rotation_speed = 2f;
    protected ObjectAnchor teleportation_sphere_instantiated;

    private bool sphere_thrown = false;

    private bool tp_sphere_button_pushed_previous_frame = false;

    /// <summary>
    /// Checks whether the button to spawn a tp sphere is pushed
    /// </summary>
    /// <returns>True when the  player has the TP button pressed, false otherwise</returns>
    protected bool is_tp_sphere_button_pushed()
    {
        return OVRInput.Get(OVRInput.RawButton.A);
    }

    
    /// <summary>
    /// Checks whether the button to activate the tp is pushed
    /// </summary>
    /// <returns>True when the  player has the TP activation button pressed, false otherwise</returns>
    protected bool is_tp_activated_button_pushed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.X);
    }

    /// <summary>
    /// Checks whether the button to cancel the tp is pushed
    /// </summary>
    /// <returns>True when the  player has the TP cancel button pressed, false otherwise</returns>
    protected bool is_tp_canceled_button_pushed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.B);
    }

    // Update is called once per frame
    void Update()
    {
        handle_tp_behavior();
    }

    /// <summary>
    /// Handles the behaviour of the teleportation with regards to the user inputs
    /// </summary>
    protected void handle_tp_behavior()
    {

        //Check for user inputs
        bool tp_sphere_button_pushed = is_tp_sphere_button_pushed();
        bool tp_activated_button_pushed = is_tp_activated_button_pushed();
        bool tp_canceled_button_pushed = is_tp_canceled_button_pushed();
        tp_sphere_button_pushed_previous_frame = tp_sphere_button_pushed;

        // If the player pushed the button to start and tp and there are no TP sphere in game, instantiate one with a disabled rigidbody
        if (tp_sphere_button_pushed && teleportation_sphere_instantiated == null && !sphere_thrown)
        {
            teleportation_sphere_instantiated = GameObject.Instantiate(teleportation_sphere, this.transform.position, this.transform.rotation);
            teleportation_sphere_instantiated.attach_to(GetComponent<HandController>());
            Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        //If the user releases the button, throw the sphere by giving it the speed of the controller and activating its rigidbody
        else if (!tp_sphere_button_pushed && teleportation_sphere_instantiated != null && !sphere_thrown)
        {
            teleportation_sphere_instantiated.detach();
            Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            //Conversion from local coordinates to world coordinates and multiply by a speed factor
            rigidbody.velocity = trackingSpace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * 1.5f;
            teleportation_sphere_instantiated.transform.eulerAngles = new Vector3(0, teleportation_sphere_instantiated.transform.eulerAngles.y, 0);
            sphere_thrown = true;
        }

        //Once the sphere is thrown
        if (sphere_thrown)
        {
            //The user can rotate it using the left joystick
            teleportation_sphere_instantiated.transform.RotateAround(teleportation_sphere_instantiated.transform.position, Vector2.up, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x * rotation_speed);

            //If the user activates the TP, the player is moved to its position and rotated in the direction faced by the sphere
            if (tp_activated_button_pushed)
            {
                characterController.Move(teleportation_sphere_instantiated.transform.position - playerTransform.position);
                characterController.transform.eulerAngles = new Vector3(0f, teleportation_sphere_instantiated.transform.eulerAngles.y, 0f);
            }

            //if the TP is canceled, or after activation, the sphere is destroyed
            if (tp_activated_button_pushed || tp_canceled_button_pushed)
            {
                GameObject.DestroyImmediate(teleportation_sphere_instantiated.gameObject);
                teleportation_sphere_instantiated = null;
                sphere_thrown = false;
            }
        }




    }
}
