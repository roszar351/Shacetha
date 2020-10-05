using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for handling input and logic behind player movement.
 * 
 */
public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;

    private float horizontalSpeed = 0f;
    private float verticalSpeed = 0f;
    private bool stopInput = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopInput)
            return;

        horizontalSpeed = 0;
        verticalSpeed = 0;

        if (stopInput)
            return;

        if (Input.GetButton("Up"))
        {
            verticalSpeed = 1f;
        }
        if (Input.GetButton("Down"))
        {
            verticalSpeed = -1f;
        }
        if (Input.GetButton("Right"))
        {
            horizontalSpeed = 1f;
        }
        if (Input.GetButton("Left"))
        {
            horizontalSpeed = -1f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void StopMovement()
    {
        stopInput = true;
    }
    public void ResumeMovement()
    {
        stopInput = false;
    }


    private void Move()
    {
        rb.MovePosition(rb.position + new Vector2(horizontalSpeed, verticalSpeed).normalized * speed * Time.fixedDeltaTime);
    }
}
