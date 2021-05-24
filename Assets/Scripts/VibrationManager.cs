using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VibrationManager
{

    private static bool right_vibrating = false;
    private static bool left_vibrating = false;

    /// <summary>
    /// Coroutine to vibrate the controllers
    /// </summary>
    /// <param name="right">true if you want to vibrate the right controller, false for the left one</param>
    /// <param name="time">The amount of time to vibrate the controller. Oculus limits the time to 1s maximum</param>
    /// <param name="frequency">The vibration frequency</param>
    /// <param name="amplitude">The vibration amplitude</param>
    /// <returns></returns>
    public static IEnumerator vibrate(bool right, float time, float frequency, float amplitude)
    {

        //Sets the current vibrating state of the controllers
        if ((!left_vibrating && !right) || (!right_vibrating && right))
        {
            if (right)
            {
                right_vibrating = true;
            }
            else
            {
                left_vibrating = true;
            }
            OVRInput.Controller controller = right ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;

            //Starts the vibrations
            OVRInput.SetControllerVibration(frequency, amplitude, controller);
            //Wait
            yield return new WaitForSeconds(time);
            //Ends the vibrations
            OVRInput.SetControllerVibration(0, 0, controller);

            //Sets the current vibrating state of the controllers
            if (right)
            {
                right_vibrating = false;
            }
            else
            {
                left_vibrating = false;
            }
        }
    }

}
