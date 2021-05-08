using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VibrationManager
{

   private static bool vibrating = false;


    public static IEnumerator vibrate(bool right, float time, float intensity){
        if(!vibrating){
            vibrating = true;
            OVRInput.Controller controller = right ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;
            OVRInput.SetControllerVibration(intensity,intensity,controller);
            yield return new WaitForSeconds(time);
            OVRInput.SetControllerVibration(0,0,controller);
            vibrating = false;
            
        }

    }



}
