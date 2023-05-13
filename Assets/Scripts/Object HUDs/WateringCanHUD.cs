using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WateringCanHUD : MonoBehaviour
{
    public Image waterCanImage;
    public Slider waterSlider;

    // set slider values to always be visible in model
    public float minValue = 0.666f;
    public float maxValue = 1.0f;

    public void UpdateWater(float amount, float maxCapacity)
    {
        waterSlider.maxValue = maxCapacity;

        // calculate ratio between the min/max of watering can and min/max of HUD element
        // http://james-ramsden.com/map-a-value-from-one-number-scale-to-another-formula-and-c-code/
        float scaledVal = minValue + (maxValue - minValue) * (amount / maxCapacity);
        
        // set values in HUD
        waterCanImage.fillAmount = scaledVal;
        waterSlider.value = amount;
    }
}
