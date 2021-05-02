using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBox : MonoBehaviour
{

    public AudioSource hit_sound;
    public AudioSource destroy_sound;
    public float life = 3f;

    public void hit(Sword sword, Vector3 velocity){
        life -= sword.damage;
        if(life <= 0f){
            Destroy(this.gameObject);
            destroy_sound.Play();
        }
        else{
            hit_sound.Play();
            this.gameObject.GetComponent<Rigidbody>().velocity = velocity * 3f;
        }
    }
}
