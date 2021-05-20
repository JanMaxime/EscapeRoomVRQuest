using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspired but largely adapted and debugged from https://developpaper.com/unity-realizes-the-effect-of-writing-on-the-blackboard-in-vr/
public class Pen : MonoBehaviour
{

    public Color32 penColor;


    public Notepad notepad;


    private bool isWriting()
    {
        return OVRInput.Get(OVRInput.RawButton.B);
    }

    private void Update()
    {
        if (!isWriting())
        {
            notepad.IsDrawing = false;
            return;
        }
        RaycastHit hit;
        Ray r = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(r, out hit, 0.2f) && hit.collider.tag == "Notepad")
        {
            //Set the UV coordinates of the corresponding board picture where the brush is located 
            notepad.SetPainterPositon(hit.textureCoord.x, hit.textureCoord.y);
            notepad.IsDrawing = true;
        }
        else
        {
            notepad.IsDrawing = false;
        }
    }
}
