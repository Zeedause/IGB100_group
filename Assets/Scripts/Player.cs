using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* Camera rotation & Player movement
     * https://www.youtube.com/watch?v=f473C43s8nE
     */

    public GameObject spawner;

    [Header("Movement")]
    public float moveSpeed;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Interaction")]
    public Camera cam;
    public float interactionDistance;
    public bool interactionCooldown = false;

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
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
        {
            StopPlayer();
            return;
        }

        //Process player movement
        Movement();

        //If the player is not on cooldown and trying to interact
        if (!interactionCooldown && Input.GetMouseButton(0))
            Interact();
        //Otherwise if on cooldown and player isn't trying to interact
        else if (interactionCooldown && !Input.GetMouseButton(0))
            //Off cooldown
            interactionCooldown = false;

        //Move held object with player
        if (heldObject)
            MoveHeldObject();
    }

    //Reset the object to it's initial state
    public void Respawn()
    {
        //Reset position & rotation
        transform.position = spawner.transform.position;
        transform.rotation = spawner.transform.rotation;

        //Held object & interaction
        heldObject = null;
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
        //Raycast a limited distance to find an object in the direction the player is looking
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.GameObject();

            //If the object is interactable, interact with it
            if (hitObject.GetComponent<Interactable>())
                hitObject.GetComponent<Interactable>().Interact();
        }
    }

    //Move the held object to the player's hands
    private void MoveHeldObject()
    {
        heldObject.transform.position = holdDisplacement.position;
        heldObject.transform.rotation = transform.rotation;
    }
}
