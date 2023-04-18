using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public GameObject placement;
    private WateringCanHUD wateringCanHUD;

    private float water;
    public float waterCapacity = 500f;
    public float wateringRate = 15f;

    [Header("Materials")]
    public Material placedMaterial;
    public Material heldMaterial;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Get components
        wateringCanHUD = transform.Find("HUD").gameObject.GetComponent<WateringCanHUD>();
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

        water = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.InProgress)
            return;

        UpdateMaterial();

        wateringCanHUD.UpdateWater(water, waterCapacity);
    }

    //Add the specified amount of water to this object, negative to subtract
    public void AddWater(float amount)
    {
        water += amount;
        if (water > waterCapacity)
            water = waterCapacity;
    }

    //Take water from this object and add it to another
    public void WaterPlant(GameObject plant)
    {
        //If empty, do nothing
        if (water == 0) return;

        //Calulate water to be transferred
        float waterTransfer = wateringRate * Time.deltaTime;
        if (water < waterTransfer)
            waterTransfer = water;
        
        //Remove water from this object
        AddWater(-waterTransfer);

        //Add water to targer object
        plant.GetComponent<Plant>().AddWater(waterTransfer);
    }

    //Checks if the the object is held and updates material accordingly
    private void UpdateMaterial()
    {
        if (GameManager.instance.player.GetComponent<Player>().heldObject == this.gameObject)
            meshRenderer.material = heldMaterial;
        else
            meshRenderer.material = placedMaterial;

    }

    //If the player successfully interacts with this object
    public void Interact()
    {
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Disable object collision
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            //Tell the player to carry this object
            GameManager.instance.player.GetComponent<Player>().heldObject = this.gameObject;

            //If picked up from placement, remove this object from it
            if (placement)
            {
                //Remove references
                placement.GetComponent<Placement>().placedObject = null;
                placement = null;
            }
        }
    }
}
