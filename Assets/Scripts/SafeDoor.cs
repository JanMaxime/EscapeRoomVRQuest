using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDoor : ContainerBeHacked
{
    public bool locked = true;
    protected float max_y = 0.7f; 
    // Update is called once per frame
    void Update()
    {
        if (!locked && transform.position.y < max_y)
        {
            // Animate the door
            //Debug.LogWarning("DOOR OPEN");
            transform.position = transform.position + new Vector3(0f, 0.01f, 0f);
        }
    }

    public override bool isHacked() // only if the door is open can the player get what's inside
    {
        return !locked;
    }
}
