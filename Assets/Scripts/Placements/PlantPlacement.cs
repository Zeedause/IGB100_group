using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPlacement : Placement
{
    public bool active;

    private void Start()
    {
        SetActive(false);
    }

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        if (!active && IsValidPlacement())
            SetActive(true);
        else if (active && !IsValidPlacement())
            SetActive(false);
    }

    //Check if this object is valid to be placed on
    private bool IsValidPlacement()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        return !placedObject && heldObject && heldObject.GetComponent<Plant>();
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<Plant>())
        {
            //Add the object to this placement
            placedObject = heldObject;

            //Add this placement to the object
            heldObject.GetComponent<Plant>().placement = this.gameObject;

            //Move and rotate the object to this placement
            heldObject.transform.position = transform.position;
            heldObject.transform.rotation = transform.rotation;

            //Enable the collider of the object
            heldObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    //Turns this object on or off
    public void SetActive(bool b)
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = b;
        this.gameObject.GetComponent<BoxCollider>().enabled = b;
        this.transform.Find("HUD").gameObject.SetActive(b);
        active = b;
    }
}
