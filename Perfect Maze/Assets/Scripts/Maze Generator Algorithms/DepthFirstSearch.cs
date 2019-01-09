using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DepthFirstSearch Algorithm, Builds a grid of walls after which walls get assigned cells and a random cell is taken as starting point to create maze.
/// </summary>

public class DepthFirstSearch : MonoBehaviour
{
    public GameObject wall;
    public Camera mainCamera;
    public Text xSizeInput;
    public Text ySizeInput;

    [SerializeField]
    private bool realTimeGeneration = true;

    public Cell[] cells;
    private GameObject[] allWalls;
    private List<int> lastCells;
    private float wallSpace = 1.0f;
    private Vector3 initialPos, myPos;
    private GameObject tempWall, wallHolder;
    private bool startedBuilding = false, containsMaze = false;
    private int currentCell, eastWestValue, childProcess, termCount, neighbourCheck, currentNeighbour, totalCells, visitedCells, backingUp, wallToBreak, defCamDis = 10,
                xSize, ySize;

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

    public void ToggleRealtime()
    {
        realTimeGeneration = !realTimeGeneration;
    }

    public void CreateWalls()//Creates initial 'grid' of walls based on the x and y size, stores all these walls within the "wallHolder" gameObject.
    { 
        if (containsMaze)//Checks if there already is a maze within the scene, if so it calls upon Init() to scrub the scene.
        {
            Init();
        }

        int.TryParse(xSizeInput.text, out xSize);
        int.TryParse(ySizeInput.text, out ySize);

        totalCells = xSize * ySize;
        initialPos = new Vector3((-xSize / 2) + wallSpace / 2, 0.0f, (-ySize / 2) + wallSpace / 2);
        myPos = initialPos;

        //Used to create the walls on the X axis of the maze.
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j*wallSpace)-wallSpace/2, 0.0f, initialPos.z + (i * wallSpace)-wallSpace/2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity);
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //Used to create the walls on the Y axis of the maze.
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

    void CreateCells ()//Creates and stores cells out of walls, the algorithm later uses this to check which walls need to be removed to create a 'maze'.
    {   
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[0];
        cells = new Cell[totalCells];

        //Gets all child objects in wallHolder
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assigns walls to cells by checking each wall from left to right and assigning their respective cardinal point.
        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            if (termCount == xSize)
            {
                eastWestValue++;
                termCount = 0;
            }

            cells[cellProcess] = new Cell();
            cells[cellProcess].east = allWalls[eastWestValue];
            cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];
                   
            eastWestValue++;
            termCount++;
            childProcess++;

            cells[cellProcess].west = allWalls[eastWestValue];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize)+xSize-1];
        }

        if (realTimeGeneration)
        {
            CreateMazeRealTime();
        }
        else
        {
            CreateMazeInstant();
        }
        
       AdjustCamera();
    }

    void CreateMazeRealTime()//Checks each cell one by one and removes a wall from the correpsonding cell to create a maze.
    {
        if(visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeighbour();//Checks neighbouring cells so the algorithm knows from where to remove walls next.
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)//checks if the algorithm has already been through the current cell 
                {
                    CheckCells();
                }
            }
            else
            {
                AssignStartingCell();
            }
            Invoke("CreateMazeRealTime", 0.0f);
        }
    }

    void CreateMazeInstant()
    {
        while(visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeighbour();//Checks neighbouring cells so the algorithm knows from where to remove walls next.
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)//checks if the algorithm has already been through the current cell 
                {
                    CheckCells();
                }
            }
            else
            {
                AssignStartingCell();
            }
        }
    }

    void CheckCells()
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

    void AssignStartingCell()
    {
        currentCell = Random.Range(0, totalCells);
        cells[currentCell].visited = true;
        visitedCells++;
        startedBuilding = true;
    }

    void BreakWall()//Destroys walls based on given case value.
    {
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currentCell].north); break;
            case 2: Destroy(cells[currentCell].east); break;
            case 3: Destroy(cells[currentCell].west); break;
            case 4: Destroy(cells[currentCell].south); break;
        }
        containsMaze = true;//set to true so that script knows to scrub scene when next "CreateWalls()" is called.
    }

    void GiveMeNeighbour()  //Checks neighbouring cells so that the algorithm knows which cells to visit next, prevents it from going out of bounds
                            //or unnecessarily removing multiple walls from a single cell.
    {
        //Values are instantiated becuase the script throws an 'out of index array' error if instantiated elsewhere.
        int neighbourLength = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];//assigned bassed on walls cardinal point, BreakWall() uses this to know which wall to remove from a cell.

        //Value used to ensure nothing is removed from the outer walls of the maze, calculation is simply to make sure the neighbouring cells checked stay within
        //the outer limits of the maze. 
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

        if (neighbourLength != 0)//initializes the first wallBreak on a random position and from there checks which walls need to be broken
                                 //next based on the 'chosenCell'.
        {
            int chosenCell = Random.Range(0, neighbourLength);
            currentNeighbour = neighbours[chosenCell];
            wallToBreak = connectingWall[chosenCell];
        }
        else
        {
            if (backingUp > 0)//Used to return to previous cells in case multiple walls need to be removed from a singel cell.
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
            mainCamera.transform.position = new Vector3(0, xSize+2, 0.3f);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, ySize+2, 0.3f);
        }

        if (xSize <= defCamDis && ySize <= defCamDis)//Minimal distance of camera must remain 10, otherwise the camera zooms in too far on the maze, showing only a fraction of its totality
        {
            mainCamera.transform.position = new Vector3(0, defCamDis+2, 0.3f);
        }
    }

    private void Init()//Returns certain values to default to allow the maze to be regenerated multiple times within a single runtime.
    {
        currentCell = 0;
        visitedCells = 0;
        eastWestValue = 0;
        childProcess = 0;
        termCount = 0;
        startedBuilding = false;
        lastCells.Clear();

        Destroy(wallHolder.gameObject);

        wallHolder = new GameObject();
        wallHolder.name = "Maze";
        containsMaze = false;
    }
}
