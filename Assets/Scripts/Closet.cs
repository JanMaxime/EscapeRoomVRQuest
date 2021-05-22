using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    protected bool moveable = false;
    protected float max_z = 0.7f;
    // Update is called once per frame
    void Update()
    {
        // if triggered, the closet will move horizontally and show the door
        if(moveable == true && transform.position.z < max_z)
        {
            transform.position = transform.position + new Vector3(0f, 0f, 0.1f);
        }
    }
    public bool isMoveable()
    {
        return moveable == true;
    }
    public void setStatus(bool new_status)
    {
        moveable = new_status;
    }
}
