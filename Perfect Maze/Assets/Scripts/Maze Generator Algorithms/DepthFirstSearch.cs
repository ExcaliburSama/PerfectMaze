using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour
{
    public GameObject wall;
    public Camera mainCamera;
    public int xSize = 5;
    public int ySize = 5;
    

    private bool startedBuilding = false;
    private bool containsWalls = false;
    private int currentCell = 0;
    private int eastWestProcess = 0;
    private int childProcess = 0;
    private int termCount = 0;
    private int neighbourCheck;
    private int currentNeighbour;
    private int totalCells;
    private int visitedCells;
    private int backingUp;
    private int wallToBreak;
    private List<int> lastCells;
    private float wallSpace = 1.0f;
    private Vector3 initialPos;
    private Vector3 myPos;
    [SerializeField]
    private Cell[] cells;
    private GameObject tempWall;
    private GameObject wallHolder;
    [SerializeField]
    private GameObject[] allWalls;
    
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;    //1
        public GameObject east;     //2
        public GameObject west;     //3
        public GameObject south;    //4
    }

    void Start()
    {
        mainCamera = Camera.main;

        wallHolder = new GameObject();
        lastCells = new List<int>();
        wallHolder.name = "Maze";
    }

    public void CreateWalls()
    {

        Debug.Log("making walls");
        
        if (containsWalls)
        {
            foreach (GameObject walls in allWalls)
            {
                Destroy(walls.gameObject);
                Debug.Log("walls removed");
            }

            //visitedCells;
            currentCell = 0;
            eastWestProcess = 0;
            childProcess = 0;
            termCount = 0;
            neighbourCheck = 0;
            currentNeighbour = 0;
            totalCells = 0;
            startedBuilding = false;
            backingUp = 0;
            //wallToBreak = 0;

            Destroy(wallHolder.gameObject);
            lastCells.Clear();
            //Destroy(cells);
           //Destroy(allWalls[]);

            wallHolder = new GameObject();
            wallHolder.name = "Maze";
            //Destroy(wallHolder);
            //wallHolder = new GameObject();
            //wallHolder.name = "Maze";
            containsWalls = false;
        }
        totalCells = xSize * ySize;
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
        Debug.Log("creating cells");
        lastCells.Clear ();
        
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[0];
        cells = new Cell[totalCells];

        //Gets all child objects in wallHolder
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assigns walls to cells
        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            Debug.Log("Assigning walls to cells");
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }

            cells[cellProcess] = new Cell();
            cells[cellProcess].east = allWalls[eastWestProcess];
            cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];
                   
            eastWestProcess++;
            termCount++;
            childProcess++;

            cells[cellProcess].west = allWalls[eastWestProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize)+xSize-1];
        }
        CreateMaze();
        AdjustCamera();
    }

    void CreateMaze()
    {
        Debug.Log("creating maze");
        while(visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                Debug.Log("started building");
                GiveMeNeighbour();
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)
                {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
             Debug.Log("Finished!");
        }
        visitedCells = 0;
        Debug.Log("Last stone set");
        //Invoke("CreateMaze", 0.0f);
    }

    void BreakWall()
    {
        Debug.Log("Destroying wall");
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currentCell].north); break;
            case 2: Destroy(cells[currentCell].east); break;
            case 3: Destroy(cells[currentCell].west); break;
            case 4: Destroy(cells[currentCell].south); break;
        }
        containsWalls = true;
    }

    void GiveMeNeighbour()
    {
        Debug.Log("Checking for neighbours");
        int neighbourLength = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];

        neighbourCheck = ((currentCell + 1) / xSize);
        neighbourCheck -= 1;
        neighbourCheck *= xSize;
        neighbourCheck += xSize;

        //Checks for neighbours on the west of the current cell.
        if (currentCell +1 < totalCells && (currentCell+1) != neighbourCheck)
        {
            if (cells[currentCell+1].visited == false)
            {
                neighbours[neighbourLength] = currentCell + 1;
                connectingWall[neighbourLength] = 3;
                neighbourLength++;
            }
        }

        //Checks for neighbours on the east of the current cell.
        if (currentCell + 1 >= 0 && currentCell != neighbourCheck)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours[neighbourLength] = currentCell - 1;
                connectingWall[neighbourLength] = 2;
                neighbourLength++;
            }
        }

        //Checks for neighbours on the north of the current cell.
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours[neighbourLength] = currentCell+xSize;
                connectingWall[neighbourLength] = 1;
                neighbourLength++;
            }
        }

        //Checks for neighbours on the south of the current cell.
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours[neighbourLength] = currentCell - xSize;
                connectingWall[neighbourLength] = 4;
                neighbourLength++;
            }
        }
        if (neighbourLength != 0)
        {
            int chosenCell = Random.Range(0, neighbourLength);
            currentNeighbour = neighbours[chosenCell];
            wallToBreak = connectingWall[chosenCell];
        }
        else
        {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

    void AdjustCamera()//Changes camera distance based on the width/length of the maze. Camera distance is equal to maze size so it only requires an incremental increase.
    {
        if (xSize > ySize)
        {
            // X and Z get hardcoded values because these need to remain constant.
            mainCamera.transform.position = new Vector3(0, xSize, -0.5f);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, ySize, -0.5f);
        }
    }
}
