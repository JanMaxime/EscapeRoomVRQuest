using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingCubePuzzle : MonoBehaviour
{

    [Header("Sound")]
    //Sound to play when the puzzle is solved
    public AudioSource solvedSound;

    [Header("Lightsaver")]
    //As the lightsaber is dropped when the puzzle is solved, it needs to be passed as parameter
    public GameObject lightsaber;

    //Cubes in the puzzle
    private StackingCube[] cubes;

    static bool alreadySolved = false;

    void Start()
    {
        //Get all the puzzle cubes in the scene
        if (cubes == null) cubes = GameObject.FindObjectsOfType<StackingCube>();
    }

    /// <summary>
    /// Checks whether the puzzle is solved or not and handles the behaviour when it is solved
    /// </summary>
    public void checkPuzzleSolved()
    {
        //If the puzzle has already been solved, do nothing
        if (alreadySolved) return;

        //Check all cubes in the puzzle to see if they all have their correct one on top
        //"solved" only stays true if they all are solved
        bool solved = true;
        foreach (StackingCube cube in cubes)
        {
            if (!cube.getIsStacked())
            {
                solved = false;
                break;
            }
        }

        //If the puzzle is solved play a sound and drop the lightsaver
        if (solved)
        {
            alreadySolved = true;
            solvedSound.Play();
            lightsaber.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
