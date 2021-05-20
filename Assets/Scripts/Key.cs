using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ObjectAnchor
{
    // keys need to be collider
    public string corresponding_door = "Door1"; //Door1 is the door from room1 to room2
    Collider box_collider;

    void Start()
    {
        box_collider = gameObject.GetComponent<Collider>();
        box_collider.isTrigger = false;
    }

    void Update()
    {
        if (this.is_available())
        {
            box_collider.isTrigger = false;
        }
        else
        {
            box_collider.isTrigger = true;
        }
    }
}
