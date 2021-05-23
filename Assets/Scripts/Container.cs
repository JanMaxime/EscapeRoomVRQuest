using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    [Header("Text")]
    //The text stating how many weights are in the container
    public Text text;
    [Header("Weights")]
    public Transform weights_initial_parent;

    [Header("Colours")]
    public Color incomplete_color;
    public Color complete_color;

    //The current number of weights in the container
    private int currentWeights = 0;

    //The total number of weights such that the container is full
    private int totalWeights = 5;
    /// <summary>
    /// Updates the text with the current number of weights
    /// </summary>
    void updateText(){
        text.text = currentWeights + "/" + totalWeights + " ton";
        if(isFull()){
            text.color = complete_color;
        }
        else{
            text.color = incomplete_color;
        }
    }

    /// <summary>
    /// At the start of the game, update the text with the current number of weights (zero)
    /// </summary>
    void Start(){
        updateText();
    }

    /// <summary>
    /// When an object enters the container, the number of weights is increased by one if it is indeed a weight
    /// </summary>
    void OnTriggerEnter(Collider other){
        if (other.tag == "weight"){
            other.transform.parent = this.transform;
            currentWeights += 1;
            updateText();
        }
    }

    /// <summary>
    /// When an object exits the container, the number of weights is decreased by one if it is indeed a weight
    /// </summary>
    void OnTriggerExit(Collider other){
        if (other.tag == "weight"){
            other.transform.parent = weights_initial_parent;
            currentWeights -= 1;
            updateText();
        }
    }
    /// <summary>
    /// Checks whether the container is full or not
    /// </summary>
    /// <returns>true when the container is full, false otherwise</returns>
    public bool isFull(){
        return currentWeights >= totalWeights;
    }
}
