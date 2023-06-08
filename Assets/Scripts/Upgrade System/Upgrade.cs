using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int price;
    public bool purchased = false;
    private bool soundPlayed = false;

    private void Start()
    {
        //Set price text
        transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "$" + price + " -";

        //Set purchase state to false
        purchased = false;

        soundPlayed = false;
    }

    private void Update()
    {
        //Enable/Disable upgrade
        if (purchased)
        {
            if (!soundPlayed)
            {
                GameManager.instance.audioManager.Play("Click");
                soundPlayed = true;
            }            
            DisableUpgrade();
        }
        else
            EnableUpgrade();
    }

    //Disables the upgrade within the HUD
    public void DisableUpgrade()
    {
        //Disable Button
        transform.Find("PurchaseButton").GetComponent<Button>().interactable = false;
    }

    //Disables the upgrade within the HUD
    public void EnableUpgrade()
    {
        //Enable Button
        transform.Find("PurchaseButton").GetComponent<Button>().interactable = true;
    }
}
