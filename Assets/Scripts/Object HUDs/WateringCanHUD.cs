using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WateringCanHUD : MonoBehaviour
{
    public void UpdateWater(float amount, float maxCapacity)
    {
        TextMeshProUGUI waterValueLabel = this.gameObject.transform.Find("WaterValueLabel").gameObject.GetComponent<TextMeshProUGUI>();

        waterValueLabel.text = "Water:\n\r" + Mathf.Ceil(amount) + "/" + maxCapacity;
    }
}
