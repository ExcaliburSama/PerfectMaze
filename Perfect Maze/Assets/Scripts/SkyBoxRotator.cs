using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the skybox.
/// </summary>

public class SkyBoxRotator : MonoBehaviour
{
    // Speed multiplier
    public float speedMultiplier = 1f;

    void Update()
    {
        //Calls upon method that allows the skybox to rotate, using time multiplied by a given speed.
        RenderSettings.skybox.SetFloat("Rotation", Time.time * speedMultiplier);
    }
}
