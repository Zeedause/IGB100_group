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

    //Upgrade State Variables
    private GameObject[] upgrades;
    private object[] upgradeStates = new object[]
    {
        0,
        false,
        false,
        false,
        false
    };

    // DISABLED - Fertiliser enabled as level progression
    //[Header("Fertiliser Upgrade")]
    //public GameObject fertiliserUpgrade;
    //public GameObject fertiliserSpawner;

    //Method call to externally initialise this object
    public void Initialise()
    {
         upgrades = new GameObject[]
         {
            wateringCanUpgrade,
            windowUpgrade,
            plantLadderUpgrade,
            runningShoesUpgrade
         };
    }

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

        //Set the upgrade purchase state
        wateringCanUpgrade.GetComponent<Upgrade>().purchased = true;

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= wateringCanUpgrade.GetComponent<Upgrade>().price;

        //Upgrade the watering can
        GameManager.instance.wateringCan.GetComponent<WateringCan>().Upgrade();

        //Upgrade the water placement
        waterPlacement.GetComponent<WaterPlacement>().Upgrade();
    }

    //Button Event - Purchases the window upgrade
    public void WindowUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < windowUpgrade.GetComponent<Upgrade>().price)
            return;

        //Set the upgrade purchase state
        windowUpgrade.GetComponent<Upgrade>().purchased = true;

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

        //Set the upgrade purchase state
        plantLadderUpgrade.GetComponent<Upgrade>().purchased = true;

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

        //Set the upgrade purchase state
        runningShoesUpgrade.GetComponent<Upgrade>().purchased = true;

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= runningShoesUpgrade.GetComponent<Upgrade>().price;

        //Enable dashing on the player
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

    //Saves the current purchase state of all upgrades so that they may be reverted to
    public void SaveUpgradeState()
    {
        //Save the money total
        upgradeStates[0] = GameManager.instance.moneyTotal;

        //Save the upgrade states
        for (int i = 1; i < upgrades.Length; i++)
            upgradeStates[i] = upgrades[i].GetComponent<Upgrade>().purchased;
    }

    //Reverts the upgrade states to the previously saved states
    public void RevertUpgradeState()
    {
        //Revert the money total
        GameManager.instance.moneyTotal = (int)upgradeStates[0];

        //Revert upgrades
        if (upgrades[0].GetComponent<Upgrade>().purchased && !(bool)upgradeStates[1])
            WateringCanRevert();
        if (upgrades[1].GetComponent<Upgrade>().purchased && !(bool)upgradeStates[2])
            WindowRevert();
        if (upgrades[2].GetComponent<Upgrade>().purchased && !(bool)upgradeStates[3])
            PlantLadderRevert();
        if (upgrades[3].GetComponent<Upgrade>().purchased && !(bool)upgradeStates[4])
            RunningShoesRevert();
    }

    //Reverts the purchase of the watering can upgrade
    public void WateringCanRevert()
    {
        //Set the upgrade purchase state
        wateringCanUpgrade.GetComponent<Upgrade>().purchased = false;

        //Revert the watering can
        GameManager.instance.wateringCan.GetComponent<WateringCan>().Downgrade();

        //Revert the water placement
        waterPlacement.GetComponent<WaterPlacement>().Downgrade();
    }

    //Reverts the purchase of the window upgrade
    public void WindowRevert()
    {
        //Set the upgrade purchase state
        windowUpgrade.GetComponent<Upgrade>().purchased = false;

        //Disable the second window
        windowHole2.SetActive(true);
        window2.SetActive(false);
    }

    //Reverts the purchase of the watering can upgrade
    public void PlantLadderRevert()
    {
        //Set the upgrade purchase state
        plantLadderUpgrade.GetComponent<Upgrade>().purchased = false;

        //Disable the second plant ladder
        plantLadder2.SetActive(false);
    }

    //Reverts the purchase of the watering can upgrade
    public void RunningShoesRevert()
    {
        //Set the upgrade purchase state
        runningShoesUpgrade.GetComponent<Upgrade>().purchased = false;

        //Disable dashing on the player
        GameManager.instance.player.GetComponent<Player>().dashEnabled = false;
    }
}
