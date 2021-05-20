using UnityEngine;

public class HandController : MonoBehaviour {

	// Store the hand type to know which button should be pressed
	public enum HandType : int { LeftHand, RightHand };
	[Header( "Hand Properties" )]
	public HandType handType;


	// Store the player controller to forward it to the object
	[Header( "Player Controller" )]
	public MainPlayerController playerController;

    [Header("Teleportation sphere")]
    public ObjectAnchor teleportation_sphere;

    [Range (1f, 5f)]
    public float rotation_speed = 2f;
    protected ObjectAnchor teleportation_sphere_instantiated;

    [Header("Tracking space")]
    public Transform trackingSpace;

    bool sphere_thrown = false;

    public CharacterController characterController;



	// Store all gameobjects containing an Anchor
	// N.B. This list is static as it is the same list for all hands controller
	// thus there is no need to duplicate it for each instance
	static protected ObjectAnchor[] anchors_in_the_scene;
	void Start () {
		// Prevent multiple fetch
		if ( anchors_in_the_scene == null ) anchors_in_the_scene = GameObject.FindObjectsOfType<ObjectAnchor>();
	}


	// This method checks that the hand is closed depending on the hand side
	protected bool is_hand_closed () {
		// Case of a left hand
		if ( handType == HandType.LeftHand ) return
			OVRInput.Get( OVRInput.Axis1D.PrimaryIndexTrigger ) > 0.5;   // Check that the index finger is pressing


		// Case of a right hand
		else return
         OVRInput.Get( OVRInput.Axis1D.SecondaryIndexTrigger ) > 0.5; // Check that the index finger is pressing
	}

    protected bool is_tp_sphere_button_pushed(){
        if ( handType == HandType.RightHand ){
            return OVRInput.Get(OVRInput.RawButton.A);
        }
        else{
            return false;
        }

    }

    protected bool is_tp_activated_button_pushed(){
        return OVRInput.GetDown(OVRInput.RawButton.Y);
    }

    protected bool is_tp_canceled_button_pushed(){
        return OVRInput.GetDown(OVRInput.RawButton.B);
    }




	// Automatically called at each frame
	void Update () { 
        handle_grab_and_throw_behavior();
        handle_tp_behavior();
        }


	// Store the previous state of triggers to detect edges
	protected bool is_hand_closed_previous_frame = false;

	// Store the object atached to this hand
	// N.B. This can be extended by using a list to attach several objects at the same time
	protected ObjectAnchor object_grasped = null;

	/// <summary>
	/// This method handles the linking of object anchors to this hand controller
	/// </summary>
	protected void handle_grab_and_throw_behavior () {

		// Check if there is a change in the grasping state (i.e. an edge) otherwise do nothing
		bool hand_closed = is_hand_closed();
		if ( hand_closed == is_hand_closed_previous_frame) return;
		is_hand_closed_previous_frame = hand_closed;

		//==============================================//
		// Define the behavior when the hand get closed //
		//==============================================//
		if ( hand_closed ) {

			// Determine which object available is the closest from the left hand
			int best_object_id = -1;
			float best_object_distance = float.MaxValue;
			float oject_distance;

			// Iterate over objects to determine if we can interact with it
			for ( int i = 0; i < anchors_in_the_scene.Length; i++ ) {

				// Skip object not available
				if ( !anchors_in_the_scene[i].is_available() ) continue;

				// Compute the distance to the object
				oject_distance = Vector3.Distance( this.transform.position, anchors_in_the_scene[i].transform.position );

				// Keep in memory the closest object
				// N.B. We can extend this selection using priorities
				if ( oject_distance < best_object_distance && oject_distance <= anchors_in_the_scene[i].get_grasping_radius() ) {
					best_object_id = i;
					best_object_distance = oject_distance;
				}
			}

			// If the best object is in range grab it
			if ( best_object_id != -1 ) {

				// Store in memory the object grasped
				object_grasped = anchors_in_the_scene[best_object_id];


				// Grab this object
				object_grasped.attach_to( this );
			}
		//==============================================//
		// Define the behavior when the hand get opened //
		//==============================================//
		} else if ( object_grasped != null ) {

			// Release the object
			object_grasped.detach_from( this );
            object_grasped = null;
		}

	}

    protected bool tp_sphere_button_pushed_previous_frame = false;

    protected void handle_tp_behavior(){

        bool tp_sphere_button_pushed = is_tp_sphere_button_pushed();
        bool tp_activated_button_pushed = is_tp_activated_button_pushed();  
        bool tp_canceled_button_pushed = is_tp_canceled_button_pushed();
		//if ( tp_sphere_button_pushed == tp_sphere_button_pushed_previous_frame) return;

        tp_sphere_button_pushed_previous_frame = tp_sphere_button_pushed;


        //If it is the right hand, handle TP sphere. Left hand does not deal with it.
        if(handType == HandType.RightHand){
            //If there are no TP sphere in game, instantiate one
            if (tp_sphere_button_pushed && teleportation_sphere_instantiated == null  && !sphere_thrown){
                teleportation_sphere_instantiated = GameObject.Instantiate(teleportation_sphere, this.transform.position, this.transform.rotation);
                teleportation_sphere_instantiated.attach_to(this);
                Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }

            //If the user releases the button, throw the sphere
            else if (!tp_sphere_button_pushed && teleportation_sphere_instantiated != null && !sphere_thrown){
                teleportation_sphere_instantiated.detach();
                Rigidbody rigidbody = teleportation_sphere_instantiated.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
                //From local coord to world coor.
                rigidbody.velocity = trackingSpace.rotation* OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * 1.5f;
                teleportation_sphere_instantiated.transform.eulerAngles = new Vector3(0,teleportation_sphere_instantiated.transform.eulerAngles.y, 0);
                sphere_thrown = true;
            }

            if(sphere_thrown){

                teleportation_sphere_instantiated.transform.RotateAround(teleportation_sphere_instantiated.transform.position, Vector2.up, OVRInput.Get( OVRInput.Axis2D.PrimaryThumbstick).x * rotation_speed);

                if (tp_activated_button_pushed){
                    characterController.Move(teleportation_sphere_instantiated.transform.position - playerController.transform.position);
                    characterController.transform.eulerAngles = new Vector3(0f, teleportation_sphere_instantiated.transform.eulerAngles.y, 0f);
                }

                if (tp_activated_button_pushed || tp_canceled_button_pushed){
                    GameObject.DestroyImmediate(teleportation_sphere_instantiated.gameObject);
                    teleportation_sphere_instantiated = null;
                    sphere_thrown = false;
                }
            }


        }

    }
}