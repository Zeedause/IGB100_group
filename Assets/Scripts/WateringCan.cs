using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using UnityEngine;

public class WateringCan : Interactable
{
    public GameObject spawner;
    public GameObject placement;
    private WateringCanHUD wateringCanHUD;

    [Header("Water")]
    public float water;
    public float waterCapacity = 150f;
    public float wateringRate = 15f;

    [Header("Upgrades")]
    public int upgradeLevel = 0;
    public float[] waterCapacities = new float[] { 200f, 750f };
    public float[] wateringRates = new float[] { 15f, 30f };

    // Start is called before the first frame update
    void Start()
    {
        //Get components
        wateringCanHUD = transform.Find("HUD").gameObject.GetComponent<WateringCanHUD>();

        water = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Increments the upgrade level and applies new stats
    public void Upgrade()
    {
        upgradeLevel++;

        waterCapacity = waterCapacities[upgradeLevel];
        wateringRate = wateringRates[upgradeLevel];
        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Decrements the upgrade level and applies new stats
    public void Downgrade()
    {
        upgradeLevel--;

        waterCapacity = waterCapacities[upgradeLevel];
        wateringRate = wateringRates[upgradeLevel];
        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Reset the object to it's initial state
    public void Respawn()
    {
        //Reset position & rotation
        transform.position = spawner.transform.position;
        transform.rotation = spawner.transform.rotation;

        //Placement
        placement = null;

        //Enable object collision
        this.gameObject.GetComponent<BoxCollider>().enabled = true;

        //Values
        water = 20f;
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        water += amount;
        if (water > waterCapacity)
            water = waterCapacity;

        if (water < waterCapacity && amount > 0)
        {
            //Play 'Water Fill' sound
            GameManager.instance.audioManager.ExclusivePlay("Water Fill");
        }
        else
        {
            //Stop 'Water Fill' sound
            GameManager.instance.audioManager.Stop("Water Fill");
        }
    }

    //Take water from this object and add it to another
    public void WaterPlant(GameObject plant)
    {
        //If empty, do nothing
        if (water == 0)
        {
            //Stop 'Water Pour' sound
            GameManager.instance.audioManager.Stop("Water Pour");

            return;
        }

        //Play 'Water Pour' sound
        GameManager.instance.audioManager.ExclusivePlay("Water Pour");

        //Calulate water to be transferred
        float waterTransfer = wateringRate * Time.deltaTime;
        if (water < waterTransfer)
            waterTransfer = water;
        
        //Remove water from this object
        AddWater(-waterTransfer);

        //Add water to target object
        plant.GetComponent<Plant>().AddWater(waterTransfer);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        if (IsValidInteractable())
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Stop 'Water Fill' sound
            GameManager.instance.audioManager.Stop("Water Fill");

            //Disable object collision
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            //Tell the player to carry this object
            GameManager.instance.player.GetComponent<Player>().heldObject = this.gameObject;

            //If picked up from placement, remove this object from it
            if (placement)
            {
                //Remove references
                placement.GetComponent<Placement>().placedObject = null;
                placement = null;
            }
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the player isn't holding an object
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
            return true;

        return false;
    }
}
