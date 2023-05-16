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
    public float waterCapacity = 500f;
    public float wateringRate = 15f;

    [Header("Upgrades")]
    public int upgradeLevel = 0;
    public float[] waterCapacities = new float[] { 500f, 750f };
    public float[] wateringRates = new float[] { 15f, 30f };

    // Start is called before the first frame update
    void Start()
    {
        //Get components
        wateringCanHUD = transform.Find("HUD").gameObject.GetComponent<WateringCanHUD>();

        water = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

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
        water = 0f;
        waterCapacity = waterCapacities[upgradeLevel];
        wateringRate = wateringRates[upgradeLevel];
        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        water += amount;
        if (water > waterCapacity)
            water = waterCapacity;
    }

    //Take water from this object and add it to another
    public void WaterPlant(GameObject plant)
    {
        //If empty, do nothing
        if (water == 0) return;

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
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

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
}
