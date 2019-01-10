using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleCreator : MonoBehaviour
{
    public GameObject  marble;
    public DepthFirstSearch mazeGenerator;

    private int totalMarbles;

    private void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
    }

    public void CreateMarble()
    {
        if (mazeGenerator.totalCells > 1 && totalMarbles < 1)
        {
            Instantiate(marble, mazeGenerator.initialPos, Quaternion.identity);
            totalMarbles++;
        }
    }
}
