using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject spawner;
    public Slider DashMeter;

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

    private GameObject objectLookedAt;

    void Start()
    {
        //Get references
        rb = GetComponent<Rigidbody>();

        //Lock rigidbody rotation
        rb.freezeRotation = true;

        //Set up dash variables
        dashEnabled = false;
        dashing = false;
        dashTimer = 0;
        dashCooldownTimer = 0;

        //Set dash meter HUD
        DashMeter.maxValue = dashCooldown;
        DashMeter.value = dashCooldownTimer;
    }

    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
        {
            //Halt player movement
            StopPlayer();
            return;
        }

        //Process player movement
        Movement();

        //Process what the player is looking at
        PlayerLook();

        //If the player is not on cooldown and trying to interact
        if (!interactionCooldown && Input.GetMouseButton(0))
            Interact();
        //Otherwise if on cooldown and player isn't trying to interact
        else if (interactionCooldown && !Input.GetMouseButton(0))
            //Off cooldown
            interactionCooldown = false;

        //If player is not interacting...
        if (!Input.GetMouseButton(0))
        {
            //If holding the watering can...
            if (heldObject && heldObject.GetComponent<WateringCan>())
            {
                //Stop pouring sound
                GameManager.instance.audioManager.Stop("Water Pour");

                //Stop Watering Can Particles
                heldObject.GetComponent<WateringCan>().waterParticles.Stop();
                heldObject.GetComponent<WateringCan>().waterParticles.Clear();
            }
        }

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

                //Play 'Boost' sound
                GameManager.instance.audioManager.Play("Dash");

                //(Re)Set timers
                dashTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
            }

            //Update dash HUD
            DashMeter.value = dashCooldownTimer;
        }

        //Get movement direction from key inputs
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
            //Apply movement
            rb.velocity = direction * moveSpeed;

            //Decrement the dash cooldown timer
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownTimer = Mathf.Clamp(dashCooldownTimer, 0f, 100f);
        }
    }

    //Determine if the player is looking at an interactable object and set it's outline colour
    private void PlayerLook()
    {
        //Raycast a limited distance to find an object in the direction the player is looking
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.GameObject();

            //If the object is the same as what was already being looked at, do nothing
            if (hitObject == objectLookedAt)
                return;

            //If new object is different from what was being looked at...
            if (hitObject != objectLookedAt && objectLookedAt)
            {
                //Revert outline colour
                objectLookedAt.GetComponent<Interactable>().SetOutlineColor(0);

                //Remove reference
                objectLookedAt = null;
            }

            //If the object is interactable...
            if (hitObject.GetComponent<Interactable>())
            {
                //Check if the object is a vaild interactable, and change outline colour accordingly
                if (hitObject.GetComponent<Interactable>().IsValidInteractable())
                    hitObject.GetComponent<Interactable>().SetOutlineColor(1);
                else
                    hitObject.GetComponent<Interactable>().SetOutlineColor(2);

                //Reference object
                objectLookedAt = hitObject;
            }
        }
        //If raycast missed but an object was being looked at...
        else if (objectLookedAt)
        {
            //Change outline colour
            objectLookedAt.GetComponent<Interactable>().SetOutlineColor(0);

            //Remove reference
            objectLookedAt = null;
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
}
