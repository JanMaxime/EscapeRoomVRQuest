using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBox : ContainerBeHacked
{

    public AudioSource hit_sound;
    public AudioSource destroy_sound;
    public float total_life = 200f;

    public Material material;

    private Material material_instantiated;
    private float current_life;

    void Start(){
        current_life = total_life;
        material_instantiated = Instantiate(material);

    }


    public void hit(float velocity_magnitude, bool right){
        if(current_life<0){}
        material_instantiated.SetColor("_Color", material_instantiated.GetColor("_Color") - new Color(1f/total_life*velocity_magnitude,1f/total_life*velocity_magnitude,1f/total_life*velocity_magnitude, 0));
        foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>()){
            renderer.material.SetColor("_Color", material_instantiated.GetColor("_Color"));
        }
        current_life -= velocity_magnitude;
        if(current_life <= 0f){
            destroy_sound.Play();
            StartCoroutine(VibrationManager.vibrate(right, 0.3f, 1, Mathf.Min(1f, velocity_magnitude/40f)));
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            foreach (Transform child in transform){
                child.gameObject.AddComponent<Rigidbody>();
                child.GetComponent<Rigidbody>().AddExplosionForce(50, child.position, 2);
            }
            
        }   
        else{
            StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1, Mathf.Min(1f, velocity_magnitude/40f)));
            hit_sound.Play();
        }
    }

    void Update(){

    }

    public override bool isHacked() // only if the cube is hacked (broke) the key inside becomes avaliable
    {
        return current_life <= 0.0f;
    }
}


