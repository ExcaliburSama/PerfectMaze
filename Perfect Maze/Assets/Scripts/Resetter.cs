using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the goal that uses DepthFirstSearch and MArbleCreator to reset the scene.
/// </summary>

public class Resetter : MonoBehaviour
{
    public DepthFirstSearch mazeGenerator;
    public MarbleCreator marbleCreator;

    void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
        marbleCreator = GameObject.FindGameObjectWithTag("marbleman").GetComponent<MarbleCreator>();
    }

    private void OnTriggerEnter(Collider other)//the goal only resets the maze when its made contact with the marble.
    {
        if (other.gameObject.tag == "marble")
        {
            mazeGenerator.CreateWalls();
            marbleCreator.CreateMarble();
        }
    }
}
