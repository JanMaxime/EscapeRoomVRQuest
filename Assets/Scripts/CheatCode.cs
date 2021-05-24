using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    public OVRInput.RawButton[] code;
    public GameObject related_object;
    private int currentIndex = 0;

    private bool activable = true;


    public void handleCodeIndex(OVRInput.RawButton buttonPressed)
    {
        if (code[currentIndex] == buttonPressed)
        {
            currentIndex += 1;
        }
        else if (code[0] == buttonPressed)
        {
            currentIndex = 1;
        }
        else{
            currentIndex = 0;
        }
    }

    public bool completed(){
        bool is_completed = (code.Length == currentIndex) && activable;
        if (is_completed){
            activable = false;
            currentIndex = 0;
        }
        return is_completed;
    }
}
