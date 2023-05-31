using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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

    [Header("Sweetspot Values")]
    public float minWaterSweetspot;
    public float maxWaterSweetspot;
    public float minLightSweetspot;
    public float maxLightSweetspot;

    [Header("Fertilised Values")]
    public float minLightFertilised;
    public float minWaterFertilised;

    public bool testing = false;
    public float fertiliserStrength;
    public float slowGrowthStrength = 0.7f;

    [Header("Animation")]
    public GameObject plantModel;
    public Vector3 growthScaleMin = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 growthScaleMax = new Vector3(0.2f, 0.2f, 0.2f);

    internal virtual void Awake()
    {
        //Get references
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Get components
        plantHUD = transform.Find("PlantHUD").gameObject.GetComponent<PlantHUD>();

        InitialisePlant();
    }

    internal virtual void Start()
    {
        
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
        light = maxLight / 3.8f;
        water = maxWater / 3.8f;

        /*
         * Switched to inspector set values
         * 
        // set sweetspot values
        // messy figures but it aligns with UI constraints niceley, roughly bottom 0.30% and top 0.66%
        minWaterSweetspot = maxWater * 0.62f;
        maxWaterSweetspot = maxWater;
        minLightSweetspot = maxLight * 0.62f;
        maxLightSweetspot = maxLight;
        */

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
        if (testing)
        {
            FertilisedGrowing();
        }
        else
        {
            plantHUD.SetTesting(false);
            if (water >= minWaterSweetspot && water <= maxWaterSweetspot &&
                    light >= minLightSweetspot && light <= maxLightSweetspot)
            {
                plantHUD.SetGrowthState("Growing");

                //Increment growth stats
                growth += growthRate * Time.deltaTime;
                AddLight(lightRate * Time.deltaTime);
                AddWater(waterRate * Time.deltaTime);

                //Update plant HUD & Model
                UpdateHUD();
                UpdateModel();
            }
            else
            {

                // grow slowly while needs not met
                growth += growthRate * slowGrowthStrength * Time.deltaTime;

                AddLight(lightRate * Time.deltaTime);
                AddWater(waterRate * Time.deltaTime);

                UpdateHUD();
                plantHUD.SetGrowthState("Growing Slowly");
                if (water < minWaterSweetspot || water > maxWaterSweetspot)
                {
                    plantHUD.UpdateWater(water, Color.red);
                }
                if (light < minLightSweetspot || light > maxLightSweetspot)
                {
                    plantHUD.UpdateLight(light, Color.red);
                }
            }
        }

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
            if (growth > 0) 
            {
                growth -= growthRate * Time.deltaTime;
                plantHUD.SetGrowthState("Dying!");
            }
            else
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
    }

    internal virtual void FertilisedGrowing()
    {
        plantHUD.SetTesting(true);
        if (water >= minWaterFertilised && light >= minLightFertilised)
        {
            plantHUD.SetGrowthState("Rapidly Growing");

            //Increment growth stats
            growth += growthRate * fertiliserStrength * Time.deltaTime;
            AddLight(lightRate * Time.deltaTime);
            AddWater(waterRate * Time.deltaTime);

            //Update plant HUD & Model
            UpdateHUD();
            UpdateModel();
        }
        else
        {
            // grow slowly while needs not met
            growth += growthRate * slowGrowthStrength * Time.deltaTime;

            AddLight(lightRate * Time.deltaTime);
            AddWater(waterRate * Time.deltaTime);

            UpdateHUD();
            plantHUD.SetGrowthState("Growing");
            if (water < minWaterFertilised)
            {
                plantHUD.UpdateWater(water, Color.red);
            }
            if (light < minLightFertilised)
            {
                plantHUD.UpdateLight(light, Color.red);
            }
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
        if (light <= 0 || water <= 0)
            plantHUD.UpdateGrowth(growth, Color.red);
        else
            plantHUD.UpdateGrowth(growth, Color.white);


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

    //Starts the plant growth state
    public void StartGrowth()
    {
        Interact();
        ////Set growth state
        //growthState = GrowthState.Growing;

        ////Set Plant HUD
        //plantHUD.SetGrowthState("Growing");
        //plantHUD.SetGrowthStateVisibility(true);
        //plantHUD.SetGrowthStatsVisibility(true);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the player isn't holding an object, pick the object up
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //DISABLED - Moved to StartGrowth() called by spawning order
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
        //Otherwise, if the player is holidng fertiliser and this plant is growing, enable sweetspots
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Fertiliser>() && growthState == GrowthState.Growing)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Destroy fertiliser object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;

            //Enable sweetspots
            testing = true;
        }
    }
}
