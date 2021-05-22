using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButton : MonoBehaviour
{
    public Closet related_closet; 
    public AudioSource sound;

    protected Container container = null;

    void OnTriggerEnter(Collider other) //if a collider coming, check if the collier is the container
    {
        container = other.GetComponent<Container>();
        if (container == null) return;
        
        // if the container is full and the closet hasn't been moved, move the closet
        if (container.isFull() && !related_closet.isMoveable())
        {
            related_closet.setStatus(true); // move the closet
            sound.Play();
        }
    }

    void Update()
    {
        // if the container is already on the button, check if new weight comes
        if(container != null && !related_closet.isMoveable())
        {
            Debug.LogWarning(container.isFull());
            if (container.isFull())
            {
                related_closet.setStatus(true); // move the closet
                sound.Play();
            }
        }
    }
}
