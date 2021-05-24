using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnWall : MonoBehaviour
{
    public Light control_light_comp;
      
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;//hide the text if light is on
    }

    // Update is called once per frame
    void Update()
    {
        if (control_light_comp.enabled)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            // if the light component is off, show the puzzel on the wall
            gameObject.GetComponent<Renderer>().enabled = true;
            //Debug.LogWarning("The light is off! show the puzzle!");
        }
    }
}
