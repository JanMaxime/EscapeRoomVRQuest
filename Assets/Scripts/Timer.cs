using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text time_display;
    public int count_down_time; // in seconds, 30min = 1800s

    public Door final_door; // for checking winning status, if the door is open, stop countdown and show the winning text
    public GameObject win_text;
    public GameObject lose_text;
    public AudioSource win_sound;
    public AudioSource lose_sound;

    static protected Light[] light_comps_in_the_scene;
    protected int remaining_time;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        light_comps_in_the_scene = FindObjectsOfType(typeof(Light)) as Light[];

        remaining_time = count_down_time;
        time_display.text = string.Format("{00:00}", (int)(remaining_time / 60)) + ":" + string.Format("{00:00}", remaining_time % 60);

        //hide the both text
        win_text.GetComponent<Renderer>().enabled = false;
        lose_text.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // there is still time but the door is not open, keep countdown
        if (remaining_time > 0 && !final_door.isOpen())
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0;
                remaining_time = remaining_time - 1;
                time_display.text = string.Format("{00:00}", (int)(remaining_time / 60)) + ":" + string.Format("{00:00}", remaining_time % 60);

                //the door is not open but no more time left
                if ((int)(remaining_time / 60) <= 0 & remaining_time % 60 <= 0)
                {
                    //end game
                    foreach (Light light in light_comps_in_the_scene)
                    {
                        light.intensity = 0.15f;
                    }
                    lose_text.GetComponent<Renderer>().enabled = true;
                    lose_sound.Play();
                }
            }
        }
        // win game
        if (final_door.isOpen())
        {
            win_sound.Play();
            win_text.GetComponent<Renderer>().enabled = true;
        }

    }
}
