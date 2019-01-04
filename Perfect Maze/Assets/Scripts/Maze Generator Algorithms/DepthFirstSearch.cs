using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour
{
    public GameObject wall;
    public float wallSpace = 1.0f;
    public int xSize = 5;
    public int ySize = 5;

    private Vector3 initialPos;
    private Vector3 myPos;
    private GameObject tempWall;
    private GameObject wallHolder;

    void Start()
    {
        CreateWalls();
    }
    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        initialPos = new Vector3((-xSize / 2) + wallSpace / 2, 0.0f, (-ySize / 2) + wallSpace / 2);
        myPos = initialPos;

        //used to create the walls on the X axis of the maze
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j*wallSpace)-wallSpace/2, 0.0f, initialPos.z + (i * wallSpace)-wallSpace/2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity);
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //used to create the walls on the Y axis of the maze
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallSpace), 0.0f, initialPos.z + (i * wallSpace) - wallSpace);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
