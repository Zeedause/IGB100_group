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

    public void SetFertilised(bool visible)
    {
        if (visible)
        {
            this.gameObject.transform.Find("Sweetspots").gameObject.SetActive(false);
            this.gameObject.transform.Find("FertilisedSweetspots").gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.transform.Find("FertilisedSweetspots").gameObject.SetActive(false);
            this.gameObject.transform.Find("Sweetspots").gameObject.SetActive(true);
        }
    }

    public void SetGrowthStatsVisibility(bool visible)
    {
        this.gameObject.transform.Find("GrowthValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("LightValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("WaterValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("Sweetspots").gameObject.SetActive(visible);
        this.gameObject.transform.Find("FertilisedSweetspots").gameObject.SetActive(visible);
    }

    public void UpdateGrowth(float value, Color color)
    {
        growthSlider.value = value;
        Image growthImage = growthSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        growthImage.color = color;
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
