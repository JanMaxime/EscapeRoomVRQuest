using UnityEngine;

public class ObjectAnchor : MonoBehaviour {

	[Header( "Grasping Properties" )]
	public float graspingRadius = 0.1f;

	// Store initial transform parent
	protected Transform initial_transform_parent;
	void Start () {
		initial_transform_parent = transform.parent;
	}

	public void setInitialParent(Transform transform){
		initial_transform_parent = transform;
	}

	protected void disable_rigidbody(GameObject gameObject){
		//Get the object Rigidbody
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

		//If there is a RigidBody, then desactivate its physics properties so that it doesn't follow gravity anymore.
		if(rigidbody){
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
	}

	protected void activate_rigidbody(GameObject gameObject){
		//Get the object Rigidbody
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		//If there is a RigidBody, then activate gravity and disable kinematics so that it falls again.
		if(rigidbody){
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
			if(hand_controller.handType == HandController.HandType.LeftHand){
                    rigidbody.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                }
            else{
                rigidbody.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            }
		}
	}


	// Store the hand controller this object will be attached to
	public HandController hand_controller {get;set;}

	public virtual void attach_to ( HandController hand_controller ) {
		// Store the hand controller in memory
		this.hand_controller = hand_controller;

		// Set the object to be placed in the hand controller referential
		transform.SetParent( hand_controller.transform );

		disable_rigidbody(this.gameObject);
	}

	public virtual void detach_from ( HandController hand_controller ) {
		// Make sure that the right hand controller ask for the release
		if ( this.hand_controller != hand_controller ) return;

		activate_rigidbody(this.gameObject);
		// Detach the hand controller
		this.hand_controller = null;

		// Set the object to be placed in the original transform parent
		transform.SetParent( initial_transform_parent );

	}

	public void detach(){
		transform.parent = null;
	}

	public bool is_available () { return hand_controller == null; }

	public float get_grasping_radius () { return graspingRadius; }
}