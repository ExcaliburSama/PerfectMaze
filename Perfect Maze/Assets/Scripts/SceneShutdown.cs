using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Method to shutdown application.
/// </summary>

public class SceneShutdown : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            ShutDown();
        }
    }

    void ShutDown()
    {
        Application.Quit();
    }
}
