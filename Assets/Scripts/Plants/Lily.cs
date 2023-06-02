using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lily : Plant
{
    public float dryRate = -1f;

    ////Growth State - Growing
    //internal override void Growing()
    //{
    //    if (fertilised)
    //    {
    //        FertilisedGrowing();
    //    }
    //    else
    //    {        
    //        plantHUD.SetFertilised(false);
    //        if (water >= minWaterSweetspot && water <= maxWaterSweetspot &&
    //                light >= minLightSweetspot && light <= maxLightSweetspot)
    //        {
    //            plantHUD.SetGrowthState("Growing");

    //            //Increment growth stats
    //            growth += growthRate * Time.deltaTime;
    //            AddLight(lightRate * Time.deltaTime);
    //            AddWater(waterRate * Time.deltaTime);

    //            //Check if placed in light
    //            if (placement && placement.GetComponent<LightPlacement>())
    //            {
    //                AddWater(dryRate * Time.deltaTime);
    //            }

    //            //Update plant HUD & Model
    //            UpdateGrowthStatsHUD();
    //            UpdateModel();
    //        }
    //        else
    //        {
    //            // grow slowly while needs not met
    //            growth += growthRate * slowGrowthStrength * Time.deltaTime;

    //            AddLight(lightRate * Time.deltaTime);
    //            AddWater(waterRate * Time.deltaTime);

    //            UpdateGrowthStatsHUD();
    //            UpdateModel();
    //            plantHUD.SetGrowthState("Growing Slowly");
    //            if (water < minWaterSweetspot || water > maxWaterSweetspot)
    //            {
    //                plantHUD.UpdateWater(water, Color.red);
    //            }
    //            if (light < minLightSweetspot || light > maxLightSweetspot)
    //            {
    //                plantHUD.UpdateLight(light, Color.red);
    //            }
    //        }
    //    }

    //    //Check for 'fully grown' or 'dead' conditions
    //    if (growth >= fullGrowth)
    //    {
    //        //The plant is fully grown
    //        growthState = GrowthState.Grown;

    //        //Play 'Spare Ding 1' sound
    //        GameManager.instance.audioManager.Play("Spare Ding 1");

    //        //Set Plant HUD
    //        plantHUD.SetGrowthState("Grown");
    //        plantHUD.SetGrowthStateVisibility(true);
    //        plantHUD.SetGrowthStatsVisibility(false);
    //    }
    //    else if (light <= 0f || water <= 0f)
    //    {
    //        //The plant dies
    //        growthState = GrowthState.Dead;

    //        //Player loses money
    //        gameManager.AddMoney(-sellValue / lossDivisor);

    //        //Set Plant HUD
    //        plantHUD.SetGrowthState("Dead");
    //        plantHUD.SetGrowthStateVisibility(true);
    //        plantHUD.SetGrowthStatsVisibility(false);
    //    }
    //}

    //Growth State - Growing
    internal override void Growing()
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
        if (placement && placement.GetComponent<LightPlacement>())
            AddWater(dryRate * Time.deltaTime);

        //Update HUD & model
        UpdateGrowthStatsHUD();
        UpdateModel();
    }

    //internal override void FertilisedGrowing()
    //{
    //    plantHUD.SetFertilised(true);
    //    if (water >= minWaterFertilised && light >= minLightFertilised)
    //    {
    //        plantHUD.SetGrowthState("Rapidly Growing");

    //        //Increment growth stats
    //        growth += growthRate * fertiliserStrength * Time.deltaTime;
    //        AddLight(lightRate * Time.deltaTime);
    //        AddWater(waterRate * Time.deltaTime);

    //        //Check if placed in light
    //        if (placement && placement.GetComponent<LightPlacement>())
    //        {
    //            AddWater(dryRate * Time.deltaTime);
    //        }

    //        //Update plant HUD & Model
    //        UpdateGrowthStatsHUD();
    //        UpdateModel();
    //    }
    //    else
    //    {
    //        // grow slowly while needs not met
    //        growth += growthRate * slowGrowthStrength * Time.deltaTime;

    //        AddLight(lightRate * Time.deltaTime);
    //        AddWater(waterRate * Time.deltaTime);

    //        UpdateGrowthStatsHUD();
    //        UpdateModel();
    //        plantHUD.SetGrowthState("Growing");
    //        if (water < minWaterFertilised)
    //        {
    //            plantHUD.UpdateWater(water, Color.red);
    //        }
    //        if (light < minLightFertilised)
    //        {
    //            plantHUD.UpdateLight(light, Color.red);
    //        }
    //    }
    //}
}
