using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used in creating and managing theMarble and its goal counterpart.
/// </summary>

public class MarbleCreator : MonoBehaviour
{
    public GameObject  marble, currentMarble, goal, currentgoal;
    public DepthFirstSearch mazeGenerator;
    public Slider slider;

    private bool marbleActive;
    private float disAdjust = 0.5f;
    private BallMovement marbleSpeed;

    private void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
    }

    public void ApplySpeed()
    {
        if (marbleActive)//Checks if there's a marble active and then applies the value of the slider to the marble.
        {
            marbleSpeed.speed = slider.value;
        }
    }

    public void CreateMarble()
    {
        if (mazeGenerator.totalCells > 1 && !marbleActive)//uses totalcells to check if a maze has already been created, if not, no marble can be spawned.
        {
            Instantiate(marble, new Vector3 (mazeGenerator.initialPos.x, mazeGenerator.initialPos.y, mazeGenerator.initialPos.z - disAdjust), Quaternion.identity); //disAdjust is so that the goal doesnt spawn in a wall.
            Instantiate(goal, new Vector3 (mazeGenerator.myPos.x, mazeGenerator.myPos.y, mazeGenerator.myPos.z - disAdjust), Quaternion.Euler(-90,0,0));//Uses myPos because it's always the position of the last wall built after a maze is built. 
            
            //takes currentMarble in the scene as reference, this because the instantiated marble is a clone of a prefab which isn't inherently recognized.
            currentMarble = GameObject.FindGameObjectWithTag("marble");
            currentgoal = GameObject.FindGameObjectWithTag("goal");
            marbleSpeed = GameObject.FindGameObjectWithTag("marble").GetComponent<BallMovement>();//used to create a reference to the amrble speed so that i can be adjusted through a slider in the UI.
            marbleActive = true;
        }      
        else
        {                              
            currentMarble.transform.position = new Vector3(mazeGenerator.initialPos.x, mazeGenerator.initialPos.y, mazeGenerator.initialPos.z - disAdjust); //Use this instead of destroy or setactive, easier on the processing.
            currentgoal.transform.position = new Vector3 (mazeGenerator.myPos.x, mazeGenerator.myPos.y, mazeGenerator.myPos.z - disAdjust);
        }
    }
}
