using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    public DepthFirstSearch mazeGenerator;
    public MarbleCreator marbleCreator;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("mazegenerator").GetComponent<DepthFirstSearch>();
        marbleCreator = GameObject.FindGameObjectWithTag("marbleman").GetComponent<MarbleCreator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "marble")
        {
            mazeGenerator.CreateWalls();
            marbleCreator.CreateMarble();
        }
    }
}
