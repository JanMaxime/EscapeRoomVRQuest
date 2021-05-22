using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    private int currentWeights = 0;
    private int totalWeights = 5;

    public Text text;

    public Transform weights_initial_parent;

    public Color incomplete_color;
    public Color complete_color;

    void Start(){
        updateText();
    }

    void updateText(){
        text.text = currentWeights + "/" + totalWeights + " ton";
        if(isFull()){
            text.color = complete_color;
        }
        else{
            text.color = incomplete_color;
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "weight"){
            other.transform.parent = this.transform;
            currentWeights += 1;
            updateText();
        }
    }

    void OnTriggerExit(Collider other){
        if (other.tag == "weight"){
            other.transform.parent = weights_initial_parent;
            currentWeights -= 1;
            updateText();
        }
    }
    public bool isFull(){
        return currentWeights == totalWeights;
    }
}
