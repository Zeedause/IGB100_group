using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rose : Plant
{
    internal override void Growing()
    {
        if (testing)
        {
            plantHUD.SetTesting(true);
            if (water > minWaterSweetspot && water < maxWaterSweetspot &&
            light > minLightSweetspot && water < maxWaterSweetspot)
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
                AddLight(lightRate * Time.deltaTime);
                AddWater(waterRate * Time.deltaTime);

                UpdateHUD();
                plantHUD.SetGrowthState("Needs not Met");
                if (water < minWaterSweetspot || water > maxWaterSweetspot)
                {

                    plantHUD.UpdateWater(water, Color.red);
                }
                else
                {
                    plantHUD.UpdateLight(light, Color.red);
                }
            }
        }
        else
        {
            plantHUD.SetTesting(false);
            //Increment growth stats
            growth += growthRate * Time.deltaTime;
            AddLight(lightRate * Time.deltaTime);
            AddWater(waterRate * Time.deltaTime);

            //Update plant HUD & Model
            UpdateHUD();
            UpdateModel();
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