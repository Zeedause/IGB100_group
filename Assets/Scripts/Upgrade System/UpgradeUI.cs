using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    //References
    public GameObject moneyCounter;

    [Header("Watering Can Upgrade")]
    public GameObject wateringCanUpgrade;
    public GameObject waterPlacement;

    [Header("Window Upgrade")]
    public GameObject windowUpgrade;
    public GameObject window2;
    public GameObject windowHole2;

    [Header("Plant Ladder Upgrade")]
    public GameObject plantLadderUpgrade;
    public GameObject plantLadder2;

    [Header("Running Shoes Upgrade")]
    public GameObject runningShoesUpgrade;

    // DISABLED - Fertiliser enabled as level progression
    //[Header("Fertiliser Upgrade")]
    //public GameObject fertiliserUpgrade;
    //public GameObject fertiliserSpawner;

    private void Update()
    {
        //Update money counter
        moneyCounter.GetComponent<TextMeshProUGUI>().text = "Money - $" + GameManager.instance.moneyTotal;
    }
    
    //Button Event - Purchases the watering can upgrade
    public void WateringCanUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < wateringCanUpgrade.GetComponent<Upgrade>().price)
            return;

        //Disable the upgrade within the HUD
        wateringCanUpgrade.GetComponent<Upgrade>().DisableUpgrade();

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= wateringCanUpgrade.GetComponent<Upgrade>().price;

        //Upgrade the watering can
        GameManager.instance.wateringCan.GetComponent<WateringCan>().upgradeLevel++;

        //Upgrade the water placement
        waterPlacement.GetComponent<WaterPlacement>().Upgrade();
    }

    //Button Event - Purchases the window upgrade
    public void WindowUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < windowUpgrade.GetComponent<Upgrade>().price)
            return;

        //Disable the upgrade within the HUD
        windowUpgrade.GetComponent<Upgrade>().DisableUpgrade();

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= windowUpgrade.GetComponent<Upgrade>().price;

        //Enable the second window
        windowHole2.SetActive(false);
        window2.SetActive(true);
    }

    //Button Event - Purchases the watering can upgrade
    public void PlantLadderUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < plantLadderUpgrade.GetComponent<Upgrade>().price)
            return;

        //Disable the upgrade within the HUD
        plantLadderUpgrade.GetComponent<Upgrade>().DisableUpgrade();

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= plantLadderUpgrade.GetComponent<Upgrade>().price;

        //Enable the second plant ladder
        plantLadder2.SetActive(true);
    }

    //Button Event - Purchases the watering can upgrade
    public void RunningShoesUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < runningShoesUpgrade.GetComponent<Upgrade>().price)
            return;

        //Disable the upgrade within the HUD
        runningShoesUpgrade.GetComponent<Upgrade>().DisableUpgrade();

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= runningShoesUpgrade.GetComponent<Upgrade>().price;

        //Upgrade the watering can
        GameManager.instance.player.GetComponent<Player>().dashEnabled = true;
    }

    // DISABLED - Fertiliser enabled as level progression
    ////Button Event - Purchases the fertiliser upgrade
    //public void FertiliserUpgrade()
    //{
    //    //If not enough money, don't purchase the upgrade
    //    if (GameManager.instance.moneyTotal < fertiliserUpgrade.GetComponent<Upgrade>().price)
    //        return;

    //    //Disable the upgrade within the HUD
    //    fertiliserUpgrade.GetComponent<Upgrade>().DisableUpgrade();

    //    //Subtract the upgrade price from the money total
    //    GameManager.instance.moneyTotal -= fertiliserUpgrade.GetComponent<Upgrade>().price;

    //    //Enable the fertiliser spawner
    //    fertiliserSpawner.SetActive(true);
    //}
}
