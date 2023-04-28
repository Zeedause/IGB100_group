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

    public void SetUI(float fullGrowth, float maxLight, float maxWater)
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

    public void SetGrowthStatsVisibility(bool visible)
    {
        // this.gameObject.transform.Find("GrowthLabel").gameObject.SetActive(visible);
        // this.gameObject.transform.Find("LightLabel").gameObject.SetActive(visible);
        // this.gameObject.transform.Find("WaterLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("GrowthValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("LightValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("WaterValueLabel").gameObject.SetActive(visible);
    }

    public void UpdateGrowth(float value)
    {
        // this.gameObject.transform.Find("GrowthValueLabel").gameObject.GetComponent<TextMeshProUGUI>().text = (Mathf.Floor(value)).ToString();
        growthSlider.value = value;
    }

    public void UpdateLight(float value, Color color)
    {
        /*
        TextMeshProUGUI LabelMesh = this.gameObject.transform.Find("LightLabel").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ValueMesh = this.gameObject.transform.Find("LightValueLabel").gameObject.GetComponent<TextMeshProUGUI>();
        ValueMesh.text = (Mathf.Ceil(value)).ToString();
        LabelMesh.color = color;
        ValueMesh.color = color;
        */

        lightSlider.value = value;
        Image lightImage = lightSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        lightImage.color = color;
    }

    public void UpdateWater(float value, Color color)
    {
        /*
        TextMeshProUGUI LabelMesh = this.gameObject.transform.Find("WaterLabel").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ValueMesh = this.gameObject.transform.Find("WaterValueLabel").gameObject.GetComponent<TextMeshProUGUI>();
        ValueMesh.text = (Mathf.Ceil(value)).ToString();
        LabelMesh.color = color;
        ValueMesh.color = color;
        */

        waterSlider.value = value;
        Image waterImage = waterSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        waterImage.color = color;
    }
}
