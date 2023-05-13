using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lily : Plant
{
    public float dryRate = -1f;

    //Growth State - Growing
    internal override void Growing()
    {
        //Increment growth stats
        growth += growthRate * Time.deltaTime;
        AddLight(lightRate * Time.deltaTime);
        AddWater(waterRate * Time.deltaTime);

        //Check if placed in light
        if (placement && placement.GetComponent<LightPlacement>())
        {
            AddWater(dryRate * Time.deltaTime);
        }

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
}
