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
    public ParticleSystem waterParticles;

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

        //Set inital water
        water = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Update HUD
        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Increments the upgrade level and applies new stats
    public void Upgrade()
    {
        //Increment upgrade level
        upgradeLevel++;

        //Apply upgrade changes
        waterCapacity = waterCapacities[upgradeLevel];
        wateringRate = wateringRates[upgradeLevel];
        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Decrements the upgrade level and applies new stats
    public void Downgrade()
    {
        //Decrement upgrade level
        upgradeLevel--;

        //Apply downgrade changed
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

        //De-reference placement
        placement = null;

        //Enable object collision
        this.gameObject.GetComponent<BoxCollider>().enabled = true;

        //Initial water value
        water = 20f;
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        //Add the amount of water
        water += amount;

        //Clamp water to max capacity
        if (water > waterCapacity)
            water = waterCapacity;

        //If filling and not yet at capacity, play filling sound
        if (water < waterCapacity && amount > 0)
        {
            //Play 'Water Fill' sound
            GameManager.instance.audioManager.ExclusivePlay("Water Fill");
        }
        //Otherwise, stop filling sound
        else
        {
            //Stop 'Water Fill' sound
            GameManager.instance.audioManager.Stop("Water Fill");
        }
    }

    //Take water from this object and add it to another
    public void WaterPlant(GameObject plant)
    {
        //If empty, do nothing and stop water pour sound
        if (water == 0)
        {
            //Stop 'Water Pour' sound
            GameManager.instance.audioManager.Stop("Water Pour");
            waterParticles.Stop();

            return;
        }

        //Play 'Water Pour' sound
        GameManager.instance.audioManager.ExclusivePlay("Water Pour");

        if (!waterParticles.isPlaying)
            waterParticles.Play();

        //Calulate water to be transferred
        float waterTransfer = wateringRate * Time.deltaTime;
        if (water < waterTransfer) //Clamp to remaining water value
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
