using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject spawner;
    public Slider DashMeter;
    public float dashOrigin;
    public float dashFinal;

    [Header("Movement")]
    public float moveSpeed;
    public bool dashEnabled;
    public bool dashing;
    public float dashSpeed;
    public float dashDuration;
    private float dashTimer;
    public float dashCooldown;
    private float dashCooldownTimer;

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

        dashEnabled = false;
        dashing = false;
        dashTimer = 0;
        dashCooldownTimer = 0;
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

        //Reset Movement/Dashing
        StopPlayer();
        dashing = false;
        dashTimer = 0;
        dashCooldownTimer = 0;

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
        //If dashing has been unlocked
        if (dashEnabled)
        {
            //If off cooldown and space is pressed
            if (dashCooldownTimer <= 0 && Input.GetKeyDown("space"))
            {
                //Set to dashing
                dashing = true;

                //Set timers
                dashTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
                dashOrigin = Time.time;
                dashFinal = dashOrigin + dashCooldown;
            }
        }

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

        //Calculate Direction
        Vector3 direction = (transform.forward * forwardInput + transform.right * horizontalInput).normalized;

        //If dashing, apply dash movement
        if (dashing)
        {
            //Dash
            rb.velocity = direction * Mathf.Lerp(moveSpeed, dashSpeed, dashTimer / dashDuration);

            //Decrement dashTimer
            dashTimer -= Time.deltaTime;
            dashTimer = Mathf.Clamp(dashTimer, 0f, 100f);

            //Check dashTimer elapse
            if (dashTimer <= 0)
                dashing = false;
        }
        //Otehrwise, move normally
        else
        {
            //float forwardInput = 0f;
            //float horizontalInput = 0f;

            //// Right movement
            //if (Input.GetKey("d"))
            //    horizontalInput += 1;
            //// Left Movement
            //if (Input.GetKey("a"))
            //    horizontalInput -= 1;
            //// Forward Movement
            //if (Input.GetKey("w"))
            //    forwardInput += 1;
            //// Backwards Movement
            //if (Input.GetKey("s"))
            //    forwardInput -= 1;

            ////Apply movement
            //Vector3 direction = (transform.forward * forwardInput + transform.right * horizontalInput).normalized;
            rb.velocity = direction * moveSpeed;

            //Decrement the dash cooldown timer
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownTimer = Mathf.Clamp(dashCooldownTimer, 0f, 100f);
        }
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

    private void FixedUpdate()
    {
        DashMeter.value = dashFinal - Time.time;
    }
}
