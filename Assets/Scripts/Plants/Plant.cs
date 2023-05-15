using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Plant : Interactable
{
    public GameObject placement;
    internal PlantHUD plantHUD;
    internal GameManager gameManager;

    public int sellValue = 20;
    public int lossDivisor = 2;

    public enum GrowthState
    {
        Seedling,
        Growing,
        Grown,
        Dead
    }
    [Header("Growth/Water/Light")]
    public GrowthState growthState;
    internal float growth = 0f;
    public float growthRate = 1f;
    public float fullGrowth = 100f;
    internal float light;
    public float maxLight = 100f;
    public float lightRate = -1f;
    internal float water;
    public float maxWater = 100f;
    public float waterRate = -1f;

    [Header("Animation")]
    public GameObject plantModel;
    public Vector3 growthScaleMin = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 growthScaleMax = new Vector3(0.2f, 0.2f, 0.2f);



    internal void Start()
    {
        //Get references
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Get components
        plantHUD = transform.Find("PlantHUD").gameObject.GetComponent<PlantHUD>();

        InitialisePlant();
    }

    internal void Update()
    {
        //Don't process unless gameState == GameState.Gameplay
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Growth State switch block
        switch (growthState)
        {
            //case GrowthState.Seedling:

            //    break;

            case GrowthState.Growing:
                Growing();
                break;

            //case GrowthState.Grown:

            //    break;

            //case GrowthState.Dead:

            //    break;
        }
    }

    internal void InitialisePlant()
    {
        //Set the initial growth state
        growthState = GrowthState.Seedling;

        //Set initial values
        light = 25f;
        water = 25f;

        //Set Plant HUD
        plantHUD.SetGrowthState("Seedling");
        plantHUD.InitialiseUI(fullGrowth, maxLight, maxWater);
        plantHUD.SetGrowthStateVisibility(false);
        plantHUD.SetGrowthStatsVisibility(false);
        UpdateHUD();

        //Update Model
        UpdateModel();
    }

    //Growth State - Growing
    internal virtual void Growing()
    {
        //Increment growth stats
        growth += growthRate * Time.deltaTime;
        AddLight(lightRate * Time.deltaTime);
        AddWater(waterRate * Time.deltaTime);

        //Update plant HUD & Model
        UpdateHUD();
        UpdateModel();

        //Check for 'fully grown' or 'dead' conditions
        if (growth >= fullGrowth)
        {
            //The plant is fully grown
            growthState = GrowthState.Grown;

            //Set Plant HUD
            plantHUD.SetGrowthState("Grown");
            plantHUD.SetGrowthStateVisibility(true);
            plantHUD.SetGrowthStatsVisibility(false);
        }
        else if (light <= 0f || water <= 0f)
        {
            //The plant dies
            growthState = GrowthState.Dead;

            //Player loses money
            gameManager.AddMoney(-sellValue / lossDivisor);

            //Set Plant HUD
            plantHUD.SetGrowthState("Dead");
            plantHUD.SetGrowthStateVisibility(true);
            plantHUD.SetGrowthStatsVisibility(false);
        }
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        water += amount;
        if (water > maxWater)
            water = maxWater;
    }

    //Add the specified amount of light to this object, negative to subtract
    public void AddLight(float amount)
    {
        light += amount;
        if (light > maxLight)
            light = maxLight;
    }

    //Updates the plant's HUD to show current growth stats
    internal void UpdateHUD()
    {
        //Growth
        plantHUD.UpdateGrowth(growth);

        //Light
        if (light <= 15.0f)
            plantHUD.UpdateLight(light, Color.red);
        else
            plantHUD.UpdateLight(light, Color.white);

        //Water
        if (water <= 15.0f)
            plantHUD.UpdateWater(water, Color.red);
        else
            plantHUD.UpdateWater(water, Color.white);
    }

    //Updates the plant's model based on current growth stats
    internal void UpdateModel()
    {
        float growthPerc = growth / fullGrowth;
        plantModel.transform.localScale = Vector3.Lerp(growthScaleMin, growthScaleMax, growthPerc);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the player isn't holding an object, pick the object up
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //If picked up for first time, start growing
            if (growthState == GrowthState.Seedling)
            {
                //Set growth state
                growthState = GrowthState.Growing;

                //Set Plant HUD
                plantHUD.SetGrowthState("Growing");
                plantHUD.SetGrowthStateVisibility(true);
                plantHUD.SetGrowthStatsVisibility(true);
            }

            //Disable object collision
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            //Tell the player to carry this object
            GameManager.instance.player.GetComponent<Player>().heldObject = this.gameObject;

            //If picked up from placement, remove this object from it
            if (placement)
            {
                placement.GetComponent<Placement>().placedObject = null;
                placement = null;
            }
        }
        //Otherwise, if the player is holidng a watering can and this plant is growing, water this plant
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<WateringCan>() && growthState == GrowthState.Growing)
        {
            GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<WateringCan>().WaterPlant(this.gameObject);
        }
    }
}
