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
    private Vector2 touchDeltaPosition;
    private Vector3 touchPosition;

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

        if (Input.touchCount > 0)
        {
            // The screen has been touched so store the touch
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // If the finger is on the screen, move the object smoothly to the touch position
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 18));
                transform.position = Vector3.Lerp(transform.position, touchPosition, Time.deltaTime);
            }
        }

        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
}
