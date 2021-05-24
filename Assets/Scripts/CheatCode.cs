using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{

    [Header("Sequence of inputs")]
    public OVRInput.RawButton[] code;

    [Header("Object to cheat with")]
    public GameObject related_object;
    private int currentIndex = 0;

    private bool activable = true;


    /// <summary>
    /// Handles the development of the cheat code
    /// </summary>
    /// <param name="buttonPressed">Button pressed by the player</param>
    public void handleCodeIndex(OVRInput.RawButton buttonPressed)
    {
        //If the player pressed the correct button in the sequence, the index is increased
        if (code[currentIndex] == buttonPressed)
        {
            currentIndex += 1;
        }
        //If it isn't the correct button, but the first one, the index is reset to 1
        else if (code[0] == buttonPressed)
        {
            currentIndex = 1;
        }
        //Otherwise, the index is reset to 0
        else
        {
            currentIndex = 0;
        }
    }

    /// <summary>
    /// Checks whether the code is complete or not
    /// A code can only be completed once
    /// </summary>
    /// <returns>true if the code is complete, false otherwise</returns>
    public bool completed()
    {
        bool is_completed = (code.Length == currentIndex) && activable;
        if (is_completed)
        {
            activable = false;
            currentIndex = 0;
        }
        return is_completed;
    }
}
