using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBeHacked : MonoBehaviour
{
    private bool hacked = false;
    public virtual bool isHacked() // only if the door is open can the player get what's inside
    {
        return hacked;
    }
}
