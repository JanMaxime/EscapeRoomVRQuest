using UnityEngine;
using static OVRHand;

public class FingerLocomotion : MonoBehaviour {

	[Header( "Hands" )]
	// Bindings with OVR Hands
	public OVRHand leftHand;
	public OVRHand rightHand;

	[Header( "Move speed" )]
	[Range( 10f, 100f )]
	public float speed = 100f;

	// Retrieve the character controller used later to move the player in the environment
	protected CharacterController character_controller;
	void Start () { character_controller = this.GetComponent<CharacterController>(); }



    private bool movement_started = false;

    private float lastLeftHandPos;
    private float lastRightHandPos;
    private float lastHandsDistance;
	void Update () {
        if(leftHand.GetFingerIsPinching(HandFinger.Pinky) && rightHand.GetFingerIsPinching(HandFinger.Pinky) ){
                    if (movement_started){
                    float diffenreces_of_distances = Mathf.Abs(lastHandsDistance - Mathf.Abs(leftHand.transform.position.y - rightHand.transform.position.y));
                    character_controller.SimpleMove(character_controller.transform.forward * speed * diffenreces_of_distances);
                    }

                    lastLeftHandPos = leftHand.transform.position.y;
                    lastRightHandPos = rightHand.transform.position.y;
                    lastHandsDistance = Mathf.Abs(lastLeftHandPos - lastRightHandPos);
                    movement_started = true;

            }
        
        else{
            movement_started = false;
        }
	}
}