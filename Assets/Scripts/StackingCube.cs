using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingCube : MonoBehaviour
{

    [Header("Cube to place on top")]
    //In the puzzle, the following instance has to be placed on top of this cube
    public GameObject cubeOnTop;

    [Header("Puzzle")]
    //Puzzle related to this cube
    public StackingCubePuzzle puzzle;

    //Is true when the correct gameobject is stacked on this cube
    private bool isStacked = false;

    /// <summary>
    /// When an object enters the trigger, checks whether it is the one that should be stacked
    /// If it is the case, we check whether the whole puzzle is solved or not
    /// </summary>
    void OnTriggerEnter(Collider other)
    {

        if (GameObject.ReferenceEquals(cubeOnTop, other.gameObject))
        {
            isStacked = true;
            puzzle.checkPuzzleSolved();
        }
    }


    /// <summary>
    /// When an object exits the trigger, checks whether it is the one that should be stacked
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (GameObject.ReferenceEquals(cubeOnTop, other.gameObject))
        {
            isStacked = false;
        }
    }

    /// <summary>
    /// Checks whether this cube has its correct cube on top
    /// If there is no cube that should come on top, it means that it is at the top of the stack
    /// and therefore it always returns true
    /// </summary>
    /// <returns>true when the correct cube is stacked on top of this one</returns>
    public bool getIsStacked()
    {
        if (!cubeOnTop) return true;
        return isStacked;
    }
}
