using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int price;
    public bool purchased = false;

    private void Start()
    {
        //Set price text
        transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "$" + price + " -";

        //Set purchase state to false
        purchased = false;
    }

    //Disables the upgrade within the HUD
    public void DisableUpgrade()
    {
        //Strikethrough text
        //transform.Find("Title").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        //transform.Find("Descriptor").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        //transform.Find("Price").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        //transform.Find("Button").transform.Find("Text").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

        //Disable Button
        transform.Find("PurchaseButton").GetComponent<Button>().interactable = false;
    }

    //Disables the upgrade within the HUD
    public void EnableUpgrade()
    {
        //Remove strikethrough text
        //transform.Find("Title").GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Strikethrough;
        //transform.Find("Descriptor").GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Strikethrough;
        //transform.Find("Price").GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Strikethrough;
        //transform.Find("Button").transform.Find("Text").GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Strikethrough;

        //Enable Button
        transform.Find("Button").GetComponent<Button>().interactable = true;
    }
}
