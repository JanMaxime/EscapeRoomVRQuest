using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

public float damage = 1f;


private Vector3 last_position;


protected Vector3 velocity;
public void OnCollisionEnter(Collision collision){
    HittableBox hittableBox = collision.gameObject.GetComponent<HittableBox>();
    if(hittableBox == null) return;
    Debug.LogWarning(velocity);
    hittableBox.hit(this, velocity);
}


public void Update(){
    if(last_position !=null){
        velocity = (transform.position - last_position) /Time.deltaTime; 
    }
    last_position = transform.position;
    }


}
