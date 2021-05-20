using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private int currentWeights = 0;
    private int totalWeights = 5;

    void OnTriggerEnter(Collider other){
        if (other.tag == "weight"){
            currentWeights += 1;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.tag == "weight"){
            currentWeights -= 1;
        }
    }
    public bool isFull(){
        return currentWeights == totalWeights;
    }
}
