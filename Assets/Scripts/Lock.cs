using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public string attched_door = "Door1";
    public bool is_locked = true;// initially all locks are locked
    public AudioSource wrong_key_sound;
    public AudioSource unlock_sound;

    private Transform trans;
    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other) //if a collider coming, check if the collier is key, if their names correspond to the same door
    {
        // Retreive the key if it exits
        Key key_collide = other.GetComponent<Key>();
        if (key_collide == null) return;

        if (is_locked)
        {
            //not correct key
            if (key_collide.corresponding_door != attched_door)
            {
                wrong_key_sound.Play();
                return;
            }

            //correct key, open the door
            unlock_sound.Play();
            is_locked = false;
        }
    }
    void OnTriggerStay(Collider other) //if a collider coming, check if the collier is key, if their names correspond to the same door
    {
        // Retreive the key if it exits
        Key key_collide = other.GetComponent<Key>();
        if (key_collide == null) return;

        //if player release the key
        if (key_collide.is_available())
        {
            // the key will be attached (inserted) to the lock
            key_collide.transform.SetParent(trans);
            //Get the object Rigidbody
            Rigidbody rigidbody = key_collide.GetComponent<Rigidbody>();

            //If there is a RigidBody, then desactivate its physics properties so that it doesn't follow gravity anymore.
            if (rigidbody)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }
        }

    }
}
