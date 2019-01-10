using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used in creating and managing theMarble and its goal counterpart.
/// </summary>

public class MarbleCreator : MonoBehaviour
{
    public GameObject  marble, currentMarble, goal, currentgoal;
    public DepthFirstSearch mazeGenerator;

    private int totalMarbles;
    private float disAdjust = 0.5f;

    private void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
    }

    public void CreateMarble()
    {
        if (mazeGenerator.totalCells > 1 && totalMarbles < 1)//uses totalcells to check if a maze has already been created, if not, no marble can be spawned.
        {
            Instantiate(marble, mazeGenerator.initialPos, Quaternion.identity);
            Instantiate(goal, new Vector3 (mazeGenerator.myPos.x, mazeGenerator.myPos.y, mazeGenerator.myPos.z - disAdjust), Quaternion.Euler(-90,0,0));//Uses myPos because it's always the position of the last wall built after a maze is built. disAdjust is so that the goal doesnt spawn in a wall.
            totalMarbles++;
        }
        
        else
        {
            //takes current marble in the scene as reference, this because the instantiated marble is a clone of a prefab which isn't inherently recognized.
            currentMarble = GameObject.FindGameObjectWithTag("marble");
            currentgoal = GameObject.FindGameObjectWithTag("goal");
            currentMarble.transform.position = mazeGenerator.initialPos; //Use this instead of destroy or setactive, easier on the processing.
            currentgoal.transform.position = new Vector3 (mazeGenerator.myPos.x, mazeGenerator.myPos.y, mazeGenerator.myPos.z - disAdjust);
        }
    }
}
