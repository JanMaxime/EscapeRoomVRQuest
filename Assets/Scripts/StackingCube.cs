using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingCube : MonoBehaviour
{

    public GameObject cubeOnTop;
    public StackingCubePuzzle puzzle;

    private bool isStacked = false;

    void OnTriggerEnter(Collider other)
    {

        if (GameObject.ReferenceEquals(cubeOnTop, other.gameObject))
        {
            isStacked = true;
            puzzle.checkPuzzleSolved();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GameObject.ReferenceEquals(cubeOnTop, other.gameObject))
        {
            isStacked = false;
        }
    }

    public bool getIsStacked()
    {
        if (!cubeOnTop) return true;
        return isStacked;
    }
}
