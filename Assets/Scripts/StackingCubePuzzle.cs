using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingCubePuzzle : MonoBehaviour
{
	private StackingCube[] cubes;
    public AudioSource solvedSound;

    public GameObject lightsaber;

    static bool alreadySolved = false;

    void Start(){
        if(cubes == null) cubes = GameObject.FindObjectsOfType<StackingCube>();
    }

    public void checkPuzzleSolved(){
        if(alreadySolved) return;
        Debug.Log(cubes.Length);
        bool solved = true;
        foreach(StackingCube cube in cubes){
            if(!cube.getIsStacked()){
                Debug.Log(cube.name);
                solved = false;
                break;
            }
        }
        if(solved){
            alreadySolved = true;
            solvedSound.Play();
            lightsaber.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
