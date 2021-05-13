using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text time_display;
    public int count_down_time; // in seconds, 30min = 1800s

    protected int remaining_time;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        remaining_time = count_down_time;
        time_display.text = string.Format("{00:00}", (int)(remaining_time / 60)) + ":" + string.Format("{00:00}", remaining_time % 60);
    }

    // Update is called once per frame
    void Update()
    {
        if (remaining_time > 0) {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0;
                remaining_time = remaining_time - 1;
                time_display.text = string.Format("{00:00}", (int)(remaining_time / 60)) + ":" + string.Format("{00:00}", remaining_time % 60);
                if ((int)(remaining_time / 60) <= 0 & remaining_time % 60 <= 0)
                {
                    //end game
                    Debug.LogWarning("End Game!");
                }
            }
        }
    }
}
