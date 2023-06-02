using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlacement : Placement
{
    public float waterRate = 15f;

    [Header("Upgrades")]
    public int upgradeLevel = 0;
    public float[] waterRates = new float[] { 15f, 30f };

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        if (placedObject)
            WaterPlacedObject();
    }

    //Adds water to the placed object's water value
    private void WaterPlacedObject()
    {
        placedObject.GetComponent<WateringCan>().AddWater(waterRate * Time.deltaTime);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<WateringCan>())
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Play 'Placement' sound
            GameManager.instance.audioManager.Play("Placement");

            //Remove the held object from the player
            GameManager.instance.player.GetComponent<Player>().heldObject = null;

            //Add the object to this placement
            placedObject = heldObject;

            //Add this placement to the object
            heldObject.GetComponent<WateringCan>().placement = this.gameObject;

            //Move and rotate the object to this placement
            heldObject.transform.position = transform.position;
            heldObject.transform.rotation = transform.rotation;
            //Move the watering can up while in the water for visilibity
            heldObject.transform.position += new Vector3(0, 0.15f, 0);

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
    }

    //Increments the upgrade level and applies new stats
    public void Upgrade()
    {
        upgradeLevel++;

        waterRate = waterRates[upgradeLevel];
    }

    //Decrements the upgrade level and applies new stats
    public void Downgrade()
    {
        upgradeLevel--;

        waterRate = waterRates[upgradeLevel];
    }
}
