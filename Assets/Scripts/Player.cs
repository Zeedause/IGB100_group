using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* Camera rotation & Player movement
     * https://www.youtube.com/watch?v=f473C43s8nE
     */

    [Header("Movement")]
    //public Transform orientation;
    private Rigidbody rb;

    public float moveSpeed;
    private Vector3 moveDirection;

    [Header("Interaction")]
    public Camera cam;
    public float maxDistance;

    public Transform holdDisplacement;
    public GameObject heldObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Movement();

        //Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            Interact();
        }

        if (heldObject)
            MoveHeldObject();
    }

    void FixedUpdate()
    {
        
    }

    //Player input controls
    private void Movement()
    {
        float forwardInput = 0f;
        float horizontalInput = 0f;

        // Right movement
        if (Input.GetKey("d"))
            horizontalInput += 1;
        // Left Movement
        if (Input.GetKey("a"))
            horizontalInput -= 1;
        // Forward Movement
        if (Input.GetKey("w"))
            forwardInput += 1;
        // Backwards Movement
        if (Input.GetKey("s"))
            forwardInput -= 1;

        rb.velocity = transform.forward * forwardInput + transform.right * horizontalInput;
        rb.velocity = rb.velocity.normalized * moveSpeed;
    }

    //Attempt to interact with an object
    private void Interact()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxDistance))
        {
            if (hitInfo.collider.GameObject().CompareTag("Plant"))
                hitInfo.collider.GameObject().GetComponent<Plant>().Interact();

            else if (hitInfo.collider.GameObject().CompareTag("PlantPlacement"))
                hitInfo.collider.GameObject().GetComponent<PlantPlacement>().Interact();

            else if (hitInfo.collider.GameObject().CompareTag("WaterPlacement"))
                hitInfo.collider.GameObject().GetComponent<WaterPlacement>().Interact();

            else if (hitInfo.collider.GameObject().CompareTag("LightPlacement"))
                hitInfo.collider.GameObject().GetComponent<LightPlacement>().Interact();

            else if (hitInfo.collider.GameObject().CompareTag("PlantFinish"))
                hitInfo.collider.GameObject().GetComponent<PlantFinish>().Interact();

            else if (hitInfo.collider.GameObject().CompareTag("PlantDelete"))
                hitInfo.collider.GameObject().GetComponent<PlantDelete>().Interact();

        }
    }

    //Move the held object to the player's hands
    private void MoveHeldObject()
    {
        heldObject.transform.position = holdDisplacement.position;
        heldObject.transform.rotation = transform.rotation;
    }
}
