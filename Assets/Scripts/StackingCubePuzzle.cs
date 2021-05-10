using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingCubePuzzle : MonoBehaviour
{
	private StackingCube[] cubes;
    public AudioSource solvedSound;

    static bool alreadySolved = false;

    void Start(){
        if(cubes == null) cubes = GameObject.FindObjectsOfType<StackingCube>();
    }

    public void checkPuzzleSolved(){
        if(alreadySolved) return;
        Debug.Log(cubes.Length);
        bool solved = true;
        foreach(StackingCube cube in cubes){
            Debug.LogWarning("o");
            if(!cube.getIsStacked()){
                Debug.Log(cube.name);
                solved = false;
                break;
            }
        }
        if(solved){
            Debug.LogWarning("SOLVED");
            alreadySolved = true;
            solvedSound.Play();
        }
    }
}
