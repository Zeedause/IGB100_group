using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlacement : MonoBehaviour
{
    public bool active;
    private Placement placement;

    public float waterRate = 15f;

    private void Start()
    {
        placement = this.gameObject.GetComponent<Placement>();

        //
        // Always visible for playtesting
        //
        SetActive(true);
    }

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.InProgress)
            return;

        /*
         * irrelevant for playtesting
         * 
        if (!active && IsValidPlacement())
            SetActive(true);
        else if (active && !IsValidPlacement())
            SetActive(false);
        */

        if (placement.placedObject)
            WaterPlacedObject();
    }

    //Adds water to the placed object's water value
    private void WaterPlacedObject()
    {
        placement.placedObject.GetComponent<WateringCan>().AddWater(waterRate * Time.deltaTime);
    }

    //Check if this object is valid to be placed on
    private bool IsValidPlacement()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        return !placement.placedObject && heldObject && heldObject.CompareTag("WateringCan");
    }

    //If the player successfully interacts with this object
    public void Interact()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.CompareTag("WateringCan"))
        {
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
            placement.placedObject = heldObject;
            heldObject.GetComponent<WateringCan>().placement = this.gameObject;

            heldObject.transform.position = transform.position;
            heldObject.transform.rotation = transform.rotation;

            // CHANGED FOR PLAYTESTING
            //
            // move the watering can up while in the water for visilibity
            heldObject.transform.position += new Vector3(0, 0.15f, 0);


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
