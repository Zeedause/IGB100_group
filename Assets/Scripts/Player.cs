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
    public float moveSpeed;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Interaction")]
    public Camera cam;
    public float maxDistance;
    public bool interactingCooldown = false;

    public Transform holdDisplacement;
    public GameObject heldObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.InProgress)
        {
            StopPlayer();
            return;
        }

        Movement();

        //Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.green);
        if (Input.GetMouseButton(0) && !interactingCooldown)
            Interact();
        else if (!Input.GetMouseButton(0) && interactingCooldown)
            interactingCooldown = false;

        if (heldObject)
            MoveHeldObject();
    }

    //Stops player movement
    private void StopPlayer()
    {
        rb.velocity = Vector3.zero;
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
            GameObject hitObject = hitInfo.collider.GameObject();
            if (!heldObject)
            {
                interactingCooldown = true;

                if (hitObject.CompareTag("Plant"))
                    hitObject.GetComponent<Plant>().Interact();

                else if (hitObject.CompareTag("WateringCan"))
                    hitObject.GetComponent<WateringCan>().Interact();
            }
            else
            {
                if (heldObject.CompareTag("Plant"))
                {
                    interactingCooldown = true;

                    if (hitObject.CompareTag("PlantPlacement"))
                        hitObject.GetComponent<PlantPlacement>().Interact();

                    else if (hitObject.CompareTag("LightPlacement"))
                        hitObject.GetComponent<LightPlacement>().Interact();

                    else if (hitObject.CompareTag("PlantFinish"))
                        hitObject.GetComponent<PlantFinish>().Interact();

                    else if (hitObject.CompareTag("PlantDelete"))
                        hitObject.GetComponent<PlantDelete>().Interact();
                }
                else if (heldObject.CompareTag("WateringCan"))
                {
                    if (hitObject.CompareTag("Plant"))
                        heldObject.GetComponent<WateringCan>().WaterPlant(hitObject);

                    else if (hitObject.CompareTag("WaterPlacement"))
                    {
                        interactingCooldown = true;
                        hitObject.GetComponent<WaterPlacement>().Interact();
                    }
                }   
            }
        }
    }

    //Move the held object to the player's hands
    private void MoveHeldObject()
    {
        heldObject.transform.position = holdDisplacement.position;
        heldObject.transform.rotation = transform.rotation;
    }
}
