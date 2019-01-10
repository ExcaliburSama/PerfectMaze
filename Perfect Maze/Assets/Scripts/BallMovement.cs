using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement scritp for the marble.
/// </summary>

public class BallMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Applying movement this way gives the object a sort easing effect in collaberation with teh mass and drag on the rigidbody.
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
}
