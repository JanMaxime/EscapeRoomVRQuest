using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VibrationManager
{

   private static bool right_vibrating = false;
   private static bool left_vibrating = false;


    public static IEnumerator vibrate(bool right, float time, float frequency, float amplitude){
        if((!left_vibrating && !right) || (!right_vibrating && right)){
            if(right){
                right_vibrating = true;
            }
            else{
                left_vibrating = true;
            }
            OVRInput.Controller controller = right ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;
            OVRInput.SetControllerVibration(frequency,amplitude,controller);
            yield return new WaitForSeconds(time);
            OVRInput.SetControllerVibration(0,0,controller);
            if(right){
                right_vibrating = false;
            }
            else{
                left_vibrating = false;
            }
        }
    }

    public static void start_vibrate(bool right, float frequency, float amplitude){
        if((!left_vibrating && !right) || (!right_vibrating && right)){
            if(right){
                right_vibrating = true;
            }
            else{
                left_vibrating = true;
            }
            OVRInput.Controller controller = right ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;
            OVRInput.SetControllerVibration(frequency,amplitude,controller);            
        }
    }

        public static void end_vibrate(bool right){
        if((left_vibrating && !right) || (right_vibrating && right)){

            OVRInput.Controller controller = right ? OVRInput.Controller.RTouch :  OVRInput.Controller.LTouch;
            OVRInput.SetControllerVibration(0,0,controller);

            if(right){
                right_vibrating = false;
            }
            else{
                left_vibrating = false;
            }       
        }
    }



}
