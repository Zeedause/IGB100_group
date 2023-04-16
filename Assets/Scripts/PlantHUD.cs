using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantHUD : MonoBehaviour
{
    public void SetGrowthState(string text)
    {
        this.gameObject.transform.Find("GrowthStateLabel").gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetGrowthStatsVisibility(bool visible)
    {
        this.gameObject.transform.Find("GrowthLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("GrowthValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("LightLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("LightValueLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("WaterLabel").gameObject.SetActive(visible);
        this.gameObject.transform.Find("WaterValueLabel").gameObject.SetActive(visible);
    }

    public void UpdateGrowth(float value)
    {
        this.gameObject.transform.Find("GrowthValueLabel").gameObject.GetComponent<TextMeshProUGUI>().text = (Mathf.Floor(value)).ToString();
    }

    public void UpdateLight(float value, Color color)
    {
        TextMeshProUGUI LabelMesh = this.gameObject.transform.Find("LightLabel").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ValueMesh = this.gameObject.transform.Find("LightValueLabel").gameObject.GetComponent<TextMeshProUGUI>();

        ValueMesh.text = (Mathf.Ceil(value)).ToString();
        LabelMesh.color = color;
        ValueMesh.color = color;
    }

    public void UpdateWater(float value, Color color)
    {
        TextMeshProUGUI LabelMesh = this.gameObject.transform.Find("WaterLabel").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ValueMesh = this.gameObject.transform.Find("WaterValueLabel").gameObject.GetComponent<TextMeshProUGUI>();

        ValueMesh.text = (Mathf.Ceil(value)).ToString();
        LabelMesh.color = color;
        ValueMesh.color = color;
    }
}
