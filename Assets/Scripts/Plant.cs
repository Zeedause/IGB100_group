using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject placement;
    private PlantHUD plantHUD;

    public Material[] materials;
    private MeshRenderer meshRenderer;

    public enum GrowthState
    {
        Seedling,
        Growing,
        Grown,
        Dead
    }
    public GrowthState growthState;
    private float growth = 0f;
    public float growthRate = 1f;
    public float fullGrowth = 10f;
    private float light;
    public float lightRate = -1f;
    public float water;
    public float waterRate = -1f;

    private void Start()
    {
        //Get components
        plantHUD = transform.Find("PlantHUD").gameObject.GetComponent<PlantHUD>();
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

        //Set the initial growth state
        SetGrowthState(GrowthState.Seedling);
    }

    private void Update()
    {
        if (growthState == GrowthState.Growing)
        {
            if (growth >= fullGrowth) //Fully grown
                SetGrowthState(GrowthState.Grown);
            else if (light <= 0f || water <= 0f) //Dead
                SetGrowthState(GrowthState.Dead);
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
        if (light > 100f)
            light = 100f;
        if (light <= 15.0f)
            plantHUD.UpdateLight(light, Color.red);
        else
            plantHUD.UpdateLight(light, Color.white);

        //Water
        water += waterRate * Time.deltaTime;
        if (water > 100f)
            water = 100f;
        if (water <= 15.0f)
            plantHUD.UpdateWater(water, Color.red);
        else
            plantHUD.UpdateWater(water, Color.white);
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
                meshRenderer.material = materials[0];
                break;

            case GrowthState.Growing:
                plantHUD.SetGrowthState("Growing");
                plantHUD.SetGrowthStatsVisibility(true);
                meshRenderer.material = materials[1];
                break;

            case GrowthState.Grown:
                plantHUD.SetGrowthState("Grown");
                plantHUD.SetGrowthStatsVisibility(false);
                meshRenderer.material = materials[2];
                break;

            case GrowthState.Dead:
                plantHUD.SetGrowthState("Dead");
                plantHUD.SetGrowthStatsVisibility(false);
                meshRenderer.material = materials[3];
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
                    lightRate -= placement.gameObject.GetComponent<LightPlacement>().lightRate;
                placement.GetComponent<Placement>().placedObject = null;
                placement = null;
            }
        }
    }
}
