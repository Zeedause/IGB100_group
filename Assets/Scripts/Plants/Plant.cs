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
    public float growth = 0f;
    public float growthRate = 1f;
    public float fullGrowth = 100f;
    public float light;
    public float maxLight = 100f;
    public float lightRate = -1f;
    public float water;
    public float maxWater = 100f;
    public float waterRate = -1f;

    [Header("Sweetspot Values")]
    public float minWaterSweetspot;
    public float maxWaterSweetspot;
    public float minLightSweetspot;
    public float maxLightSweetspot;

    [Header("Fertilised Values")]
    public float minLightFertilised;
    public float maxLightFertilised;
    public float minWaterFertilised;
    public float maxWaterFertilised;

    public bool fertilised = false;
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

    internal void Update()
    {
        //Don't process unless gameState == GameState.Gameplay
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Growth State switch block
        switch (growthState)
        {
            case GrowthState.Growing:
                Growing();
                break;
        }
    }

    internal void InitialisePlant()
    {
        //Set the initial growth state
        growthState = GrowthState.Seedling;

        //Set initial values
        light = maxLight / 3.8f;
        water = maxWater / 3.8f;

        //Set Plant HUD
        plantHUD.SetGrowthState("Seedling");
        plantHUD.InitialiseUI(fullGrowth, maxLight, maxWater);
        plantHUD.SetGrowthStateVisibility(false);
        plantHUD.SetGrowthStatsVisibility(false);
        UpdateGrowthStatsHUD();

        //Update Model
        UpdateModel();
    }

    //Growth State - Growing
    internal virtual void Growing()
    {
        //If plant is fully grown
        if (growth >= fullGrowth)
        {
            //The plant is fully grown
            growthState = GrowthState.Grown;

            //Play 'Spare Ding 1' sound
            GameManager.instance.audioManager.Play("Spare Ding 1");

            //Set Plant HUD
            plantHUD.SetGrowthState("Grown");
            plantHUD.SetGrowthStateVisibility(true);
            plantHUD.SetGrowthStatsVisibility(false);

            //Process no further
            return;
        }
        //Otherwise, if plant is dying
        else if (light <= 0f || water <= 0f)
        {
            //If plant is not yet dead
            if (growth > 0)
            {
                //Decay growth
                growth -= growthRate * Time.deltaTime;

                //Set plant HUD state
                plantHUD.SetGrowthState("Dying!");
            }
            //Otherwise if plant is at zero growth
            else
            {
                //The plant dies
                growthState = GrowthState.Dead;

                //Play 'Failed' sound
                GameManager.instance.audioManager.Play("Failed");

                //Player loses money
                gameManager.AddMoney(-sellValue / lossDivisor);

                //Set Plant HUD
                plantHUD.SetGrowthState("Dead");
                plantHUD.SetGrowthStateVisibility(true);
                plantHUD.SetGrowthStatsVisibility(false);

                //Process no further
                return;
            }
        }
        //Otherwise, grow the plant
        else
        {
            if (fertilised)
            {
                //Set plant HUD to fertilised
                plantHUD.SetFertilised(true);

                //If water & light are both within sweetspots, grow normally
                if (water >= minWaterFertilised && water <= maxWaterFertilised &&
                        light >= minLightFertilised && light <= maxLightFertilised)
                {
                    //Set plant HUD growth state
                    plantHUD.SetGrowthState("Rapidly Growing");

                    //Increment growth rapidly
                    growth += growthRate * fertiliserStrength * Time.deltaTime;
                }
                //Otherwise, grow slowly
                else
                {
                    //Set plant HUD growth state
                    plantHUD.SetGrowthState("Growing Slowly");

                    //Increment growth slowly
                    growth += growthRate * slowGrowthStrength * Time.deltaTime;
                }
            }
            else
            {
                //Set plant HUD to not fertilised
                plantHUD.SetFertilised(false);

                //If water & light are both within sweetspots, grow normally
                if (water >= minWaterSweetspot && water <= maxWaterSweetspot &&
                        light >= minLightSweetspot && light <= maxLightSweetspot)
                {
                    //Set plant HUD growth state
                    plantHUD.SetGrowthState("Growing");

                    //Increment growth normally
                    growth += growthRate * Time.deltaTime;
                }
                //Otherwise, grow slowly
                else
                {
                    //Set plant HUD growth state
                    plantHUD.SetGrowthState("Growing Slowly");

                    //Increment growth slowly
                    growth += growthRate * slowGrowthStrength * Time.deltaTime;
                }
            }
        }

        //Increment water & light
        AddLight(lightRate * Time.deltaTime);
        AddWater(waterRate * Time.deltaTime);

        //Update HUD & model
        UpdateGrowthStatsHUD();
        UpdateModel();
    }

    //internal virtual void FertilisedGrowing()
    //{
    //    //plantHUD.SetFertilised(true);
    //    //if (water >= minWaterFertilised && light >= minLightFertilised)
    //    {
    //        //plantHUD.SetGrowthState("Rapidly Growing");

    //        //Increment growth stats
    //        //growth += growthRate * fertiliserStrength * Time.deltaTime;
    //        //AddLight(lightRate * Time.deltaTime);
    //        //AddWater(waterRate * Time.deltaTime);

    //        //Update plant HUD & Model
    //        //UpdateGrowthStatsHUD();
    //        //UpdateModel();
    //    }
    //    //else
    //    //{
    //    //    // grow slowly while needs not met
    //    //    growth += growthRate * slowGrowthStrength * Time.deltaTime;

    //    //    AddLight(lightRate * Time.deltaTime);
    //    //    AddWater(waterRate * Time.deltaTime);

    //    //    UpdateGrowthStatsHUD();
    //    //    plantHUD.SetGrowthState("Growing");
    //    //    if (water < minWaterFertilised)
    //    //    {
    //    //        plantHUD.UpdateWater(water, Color.red);
    //    //    }
    //    //    if (light < minLightFertilised)
    //    //    {
    //    //        plantHUD.UpdateLight(light, Color.red);
    //    //    }
    //    //}
    //}

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        //Save water value, pre-operation
        float initialWater = water;

        //Increment water and clamp it to max capacity
        water += amount;
        if (water > maxWater)
            water = maxWater;

        //If new value enters sweetspot, play sound
        if (fertilised)
        {
            if (minWaterFertilised >= initialWater && minWaterFertilised <= water)
            {
                //Play 'Requirement Met 2' sound
                GameManager.instance.audioManager.Play("Requirement Met 2");
            }
        }
        else
        {
            if (minWaterSweetspot >= initialWater && minWaterSweetspot <= water)
            {
                //Play 'Requirement Met 2' sound
                GameManager.instance.audioManager.Play("Requirement Met 2");
            }
        }
    }

    //Add the specified amount of light to this object, negative to subtract
    public void AddLight(float amount)
    {
        //Save light value, pre-operation
        float initialLight = light;

        //Increment light and clamp it to max capacity
        light += amount;
        if (light > maxLight)
            light = maxLight;

        //If new value enters sweetspot, play sound
        if (fertilised)
        {
            if (minLightFertilised >= initialLight && minLightFertilised <= light)
            {
                //Play 'Requirement Met 1' sound
                GameManager.instance.audioManager.Play("Requirement Met 1");
            }
        }
        else
        {
            if (minLightSweetspot >= initialLight && minLightSweetspot <= light)
            {
                //Play 'Requirement Met 1' sound
                GameManager.instance.audioManager.Play("Requirement Met 1");
            }
        }
    }

    //Updates the plant's HUD to show current growth stats
    internal void UpdateGrowthStatsHUD()
    {
        //Growth
        if (light <= 0 || water <= 0)
            plantHUD.UpdateGrowth(growth, Color.red);
        else
            plantHUD.UpdateGrowth(growth, Color.white);

        //Water
        if (fertilised)
        {
            if (water < minWaterFertilised || water > maxWaterFertilised)
                plantHUD.UpdateWater(water, Color.red);
            else
                plantHUD.UpdateWater(water, Color.white);
        }
        else
        {
            if (water < minWaterSweetspot || water > maxWaterSweetspot)
                plantHUD.UpdateWater(water, Color.red);
            else
                plantHUD.UpdateWater(water, Color.white);
        }

        //Light
        if (fertilised)
        {
            if (light < minLightFertilised || light > maxLightFertilised)
                plantHUD.UpdateLight(light, Color.red);
            else
                plantHUD.UpdateLight(light, Color.white);
        }
        else
        {
            if (light < minLightSweetspot || light > maxLightSweetspot)
                plantHUD.UpdateLight(light, Color.red);
            else
                plantHUD.UpdateLight(light, Color.white);
        }
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
        //Set growth state
        growthState = GrowthState.Growing;

        //Set Plant HUD
        plantHUD.SetGrowthState("Growing");
        plantHUD.SetGrowthStateVisibility(true);
        plantHUD.SetGrowthStatsVisibility(true);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the player isn't holding an object, pick the object up
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

            //Play 'Fertiliser' sound
            GameManager.instance.audioManager.Play("Fertiliser");

            //Destroy fertiliser object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;

            //Enable sweetspots
            fertilised = true;
        }
        //Otherwise, if the player is holding another plant, swap this plant and the held plant
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Plant>())
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Play 'Placement' sound
            GameManager.instance.audioManager.Play("Placement");

            //Get the held plant refernce
            GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;

            //Disable collision for this plant, and remove this plant from the placement
            gameObject.GetComponent<BoxCollider>().enabled = false;
            placement.GetComponent<Placement>().placedObject = null;

            //Add the held plant to the placement
            placement.GetComponent<Placement>().Interact();

            //Tell the player to hold this object, and remove the placement reference from this plant
            GameManager.instance.player.GetComponent<Player>().heldObject = gameObject;
            placement = null;
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the player isn't holding an object
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
            return true;
        //Otherwise, if the player is holidng a watering can and this plant is growing
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<WateringCan>() && growthState == GrowthState.Growing)
            return true;
        //Otherwise, if the player is holidng fertiliser and this plant is growing
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Fertiliser>() && growthState == GrowthState.Growing)
            return true;
        //Otherwise, if the player is holding another plant
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Plant>())
            return true;

        return false;
    }
}
