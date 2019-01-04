using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour
{
    public GameObject wall;
    public int xSize = 5;
    public int ySize = 5;

    private float wallSpace = 1.0f;
    private int eastWestProcess = 0;
    private int childProcess = 0;
    private int termCount = 0;
    private Vector3 initialPos;
    private Vector3 myPos;
    private GameObject tempWall;
    private GameObject wallHolder;
    private GameObject[] allWalls;
    [SerializeField]
    private Cell[] cells;

    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;
        public GameObject east;
        public GameObject west;
        public GameObject south;
    }

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
        CreateCells();
    }

    void CreateCells ()
    {
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];

        //Gets all child objects in wallHolder
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assigns walls to cells
        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            cells[cellProcess] = new Cell();
            cells[cellProcess].east = allWalls[eastWestProcess];
            cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];
            if (termCount == xSize)
            {
                eastWestProcess += 2;
                termCount = 0;
            }
            else
                eastWestProcess++;
            termCount++;
            childProcess++;

            cells[cellProcess].west = allWalls[eastWestProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize)+xSize-1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
