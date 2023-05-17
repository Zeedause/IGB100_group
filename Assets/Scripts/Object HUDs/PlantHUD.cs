using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantHUD : MonoBehaviour
{
    public Slider growthSlider;
    public Slider lightSlider;
    public Slider waterSlider;

    public GameObject waterTest;
    public GameObject lightTest;

    public void InitialiseUI(float fullGrowth, float maxLight, float maxWater)
    {
        growthSlider.maxValue = fullGrowth;
        lightSlider.maxValue = maxLight;
        waterSlider.maxValue = maxWater;
    }

    public void SetGrowthState(string text)
    {
        this.gameObject.transform.Find("GrowthStateLabel").gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetGrowthStateVisibility(bool visible)
    {
        this.gameObject.transform.Find("GrowthStateLabel").gameObject.SetActive(visible);
    }

    public void SetTesting(bool visible)
    {
        this.gameObject.transform.Find("Sweetspots").gameObject.SetActive(visible);
    }

    public void SetGrowthStatsVisibility(bool visible)
    {
        this.gameObject.transform.Find("GrowthValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("LightValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("WaterValueLabel").gameObject.SetActive(visible);
    }

    public void UpdateGrowth(float value)
    {
        growthSlider.value = value;
    }

    public void UpdateLight(float value, Color color)
    {
        lightSlider.value = value;
        Image lightImage = lightSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        lightImage.color = color;
    }

    public void UpdateWater(float value, Color color)
    {
        waterSlider.value = value;
        Image waterImage = waterSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        waterImage.color = color;
    }
}
