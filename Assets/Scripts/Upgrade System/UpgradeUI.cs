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

    [Header("Fertiliser Upgrade")]
    public GameObject fertiliserUpgrade;
    public GameObject fertiliserSpawner;

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
    }

    //Button Event - Purchases the fertiliser upgrade
    public void FertiliserUpgrade()
    {
        //If not enough money, don't purchase the upgrade
        if (GameManager.instance.moneyTotal < fertiliserUpgrade.GetComponent<Upgrade>().price)
            return;

        //Disable the upgrade within the HUD
        fertiliserUpgrade.GetComponent<Upgrade>().DisableUpgrade();

        //Subtract the upgrade price from the money total
        GameManager.instance.moneyTotal -= fertiliserUpgrade.GetComponent<Upgrade>().price;

        //Enable the fertiliser spawner
        fertiliserSpawner.SetActive(true);
    }
}
