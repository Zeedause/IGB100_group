using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    //References
    public GameObject moneyCounter;

    [Header("Watering Can")]
    public GameObject wateringCanUpgrade;

    private void Update()
    {
        //Update money counter
        moneyCounter.GetComponent<TextMeshProUGUI>().text = "Money - $" + GameManager.instance.moneyTotal;
    }
    
    public void UpgradeWateringCan()
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
}
