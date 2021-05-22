using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : ObjectAnchor
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            // painting stay on wall
            rigidbody.isKinematic = true; //If isKinematic is enabled, Forces, collisions or joints will not affect the rigidbody anymore.
        }
    }

}
