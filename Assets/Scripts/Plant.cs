using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject placement;
    private PlantHUD plantHUD;
    private GameManager gameManager;

    public int sellValue = 20;

    public enum GrowthState
    {
        Seedling,
        Growing,
        Grown,
        Dead
    }
    [Header("Growth/Water/Light")]
    public GrowthState growthState;
    private float growth = 0f;
    public float growthRate = 1f;
    public float fullGrowth = 100f;
    private float light;
    public float maxLight = 100f;
    public float lightRate = -1f;
    private float water;
    public float maxWater = 100f;
    public float waterRate = -1f;
    public float dryRate = -1f;

    [Header("Animation")]
    public GameObject plantModel;
    public Vector3 growthScaleMin = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 growthScaleMax = new Vector3(0.2f, 0.2f, 0.2f);



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Get components
        plantHUD = transform.Find("PlantHUD").gameObject.GetComponent<PlantHUD>();
        plantHUD.SetUI(fullGrowth, maxLight, maxWater);

        //Set the initial growth state
        SetGrowthState(GrowthState.Seedling);
    }

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.InProgress)
            return;

        if (growthState == GrowthState.Growing)
        {
            if (growth >= fullGrowth) //Fully grown
                SetGrowthState(GrowthState.Grown);
            else if (light <= 0f || water <= 0f)
            {
                //Dead
                SetGrowthState(GrowthState.Dead);
                gameManager.LoseMoney(sellValue);
            } 
            else
                Grow(); //Continue to grow
        }
    }

    //Advance plant growth for this frame
    private void Grow()
    {
        //Growth
        growth += growthRate * Time.deltaTime;
        plantHUD.UpdateGrowth(growth);

        //Light
        light += lightRate * Time.deltaTime;
        if (light > maxLight)
            light = maxLight;
        if (light <= 15.0f)
            plantHUD.UpdateLight(light, Color.red);
        else
            plantHUD.UpdateLight(light, Color.white);

        //Water
        AddWater(waterRate * Time.deltaTime);
        if (water <= 15.0f)
            plantHUD.UpdateWater(water, Color.red);
        else
            plantHUD.UpdateWater(water, Color.white);

        //Apply growth changes to model
        UpdateModel();
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        water += amount;
        if (water > maxWater)
            water = maxWater;
    }

    //Updates the plant's model based on current growth stats
    private void UpdateModel()
    {
        float growthPerc = growth / fullGrowth;
        plantModel.transform.localScale = Vector3.Lerp(growthScaleMin, growthScaleMax, growthPerc);
    }

    //Adds to the current light rate
    public void ChangeLightRate(float rate)
    {
        lightRate += rate;
    }

    //Adds to the current water rate
    public void ChangeWaterRate(float rate)
    {
        waterRate += rate;
    }

    //Changes the growth state of the plant
    private void SetGrowthState(GrowthState state)
    {
        switch (state)
        {
            case GrowthState.Seedling:
                plantHUD.SetGrowthState("Seedling");
                plantHUD.SetGrowthStatsVisibility(false);
                break;

            case GrowthState.Growing:
                plantHUD.SetGrowthState("Growing");
                plantHUD.SetGrowthStatsVisibility(true);
                break;

            case GrowthState.Grown:
                plantHUD.SetGrowthState("Grown");
                plantHUD.SetGrowthStatsVisibility(false);
                break;

            case GrowthState.Dead:
                plantHUD.SetGrowthState("Dead");
                plantHUD.SetGrowthStatsVisibility(false);
                break;
        }

        growthState = state;
    }

    //If the player successfully interacts with this object
    public void Interact()
    {
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //If picked up for first time, start growing
            if (growthState == GrowthState.Seedling)
            {
                SetGrowthState(GrowthState.Growing);
                light = 25f;
                water = 25f;
            }

            //Disable object collision
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            //Tell the player to carry this object
            GameManager.instance.player.GetComponent<Player>().heldObject = this.gameObject;

            //If picked up from placement, remove this object from it
            if (placement)
            {
                if (placement.gameObject.CompareTag("WaterPlacement"))
                    waterRate -= placement.gameObject.GetComponent<WaterPlacement>().waterRate;
                else if (placement.gameObject.CompareTag("LightPlacement"))
                {
                    lightRate -= placement.gameObject.GetComponent<LightPlacement>().lightRate;

                    if (this.gameObject.name == "Lily(Clone)")
                    {
                        Debug.Log("Healed");
                        waterRate -= dryRate;
                    }
                }
                placement.GetComponent<Placement>().placedObject = null;
                placement = null;
            }
        }
    }
}
