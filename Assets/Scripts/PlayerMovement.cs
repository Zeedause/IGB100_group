using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* Camera rotation & Player movement
     * https://www.youtube.com/watch?v=f473C43s8nE
     */

    [Header("Movement")]

    private Rigidbody rb;

    public Transform orientation;

    public float moveSpeed;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Movement();
    }

    //Player input controls
    private void Movement()
    {
        Vector3 inputVector = Vector3.zero;
        float forwardInput = 0f;
        float horizontalInput = 0f;

        // Right movement
        if (Input.GetKey("d"))
            horizontalInput += 1;
        // Left Movement
        if (Input.GetKey("a"))
            horizontalInput -= 1;
        // Up movement
        if (Input.GetKey("w"))
            forwardInput += 1;
        // Down Movement
        if (Input.GetKey("s"))
            forwardInput -= 1;

        rb.velocity = orientation.forward * forwardInput + orientation.right * horizontalInput;
        rb.velocity = rb.velocity.normalized * moveSpeed;
    }
}
