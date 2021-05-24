using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLight : MonoBehaviour
{
    public string light_name = "MainLight1";
    public string lightna_name = "MainLight1null";
    public bool is_on = false;


    public Light light_comp; //component
    static protected LightSwitch[] light_switches_in_the_scene; // an list of all light switches including for two main lights.

    protected GameObject main_light_prefab;
    protected Material material;

    void Start()
    {
        main_light_prefab = gameObject.transform.Find("Large_round_lamp").gameObject;

        material = main_light_prefab.GetComponent<Renderer>().material;

        // Prevent multiple fetch
        if (light_switches_in_the_scene == null)
        {
            light_switches_in_the_scene = GameObject.FindObjectsOfType<LightSwitch>();
        }

        light_comp.enabled = is_on;
        // set the material of the light
        if (is_on)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool status_temp = true;
        for (int i = 0; i < light_switches_in_the_scene.Length; i++)
        {
            // check corresponding light switches
            if (light_switches_in_the_scene[i].controlled_light == this.light_name && light_switches_in_the_scene[i].is_on == false)
            {
                status_temp = false; // one switch is not on then the light will be off;
            }
            if (light_switches_in_the_scene[i].controlled_light == this.lightna_name && light_switches_in_the_scene[i].is_on == true)
            {
                status_temp = false; // one wrong switch being on will make the light remain off;
            }
        }
        light_comp.enabled = status_temp;
        // set the material of the light
        if (status_temp)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
