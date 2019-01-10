using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used in creating and managing theMarble and its goal counterpart.
/// </summary>

public class MarbleCreator : MonoBehaviour
{
    public GameObject  marble, currentMarble;
    public DepthFirstSearch mazeGenerator;

    private int totalMarbles;

    private void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
    }

    public void CreateMarble()
    {
        if (mazeGenerator.totalCells > 1 && totalMarbles < 1)//uses totalcells to check if a maze has already been created, if not, no marble can be spawned.
        {
            Instantiate(marble, mazeGenerator.initialPos, Quaternion.identity);
            totalMarbles++;
        }
        
        else
        {
            //takes current marble in the scene as reference, this because the instantiated marble is a clone of a prefab which isn't inherently recognized.
            currentMarble = GameObject.FindGameObjectWithTag("marble");
            currentMarble.transform.position = mazeGenerator.initialPos; //Use this instead of destroy or setactive, easier on the processing.
        }
    }
}
