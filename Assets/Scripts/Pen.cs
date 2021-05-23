using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspired but largely adapted and debugged from https://developpaper.com/unity-realizes-the-effect-of-writing-on-the-blackboard-in-vr/
public class Pen : MonoBehaviour
{
    [Header("Notepad")]
    //The notepad on which the pen will draw
    public Notepad notepad;


    /// <summary>
    /// Checks whether the player wants to write by checking the state of the "B" button
    /// </summary>
    /// <returns>true if the player wants to write, false otherwise</returns>
    private bool isWriting()
    {
        return OVRInput.Get(OVRInput.RawButton.B);
    }

    /// <summary>
    /// Handles the drawing on the notepad
    /// </summary>
    private void Update()
    {
        //If the player does not want to write (anymore) the status of the notepad needs to be updated accordingly
        if (!isWriting())
        {
            notepad.IsDrawing = false;
            return;
        }

        //A Ray is cast from the pen to the notepad to determine the position of the pen on it
        RaycastHit hit;
        Ray r = new Ray(this.transform.position, this.transform.forward);

        //If the ray hits the notepad close enough
        if (Physics.Raycast(r, out hit, 0.2f) && hit.collider.tag == "Notepad")
        {
            //Sets the pen positions on the board 
            notepad.SetPainterPositon(hit.textureCoord.x, hit.textureCoord.y);
            notepad.IsDrawing = true;
        }
        else
        {
            notepad.IsDrawing = false;
        }
    }
}
