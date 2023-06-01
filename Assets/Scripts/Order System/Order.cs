using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Plant;

public class Order : Interactable
{
    //Object References
    public GameObject spawnedPlant;
    public Slider timerSlider;

    //Timer Variables
    public float despawnTime = 12;
    private float despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Start despawn timer
        despawnTimer = despawnTime;

        timerSlider.maxValue = despawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.Gameplay
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Operate despawn timer
        DespawnTimer();
    }

    //Operates the timer to check when the order should timeout
    private void DespawnTimer()
    {
        //Decrement the timer
        despawnTimer -= Time.deltaTime;

        //Update timer HUD
        timerSlider.value = despawnTimer;

        //If timer has elapsed, destory this object
        if (despawnTimer <= 0)
            Destroy(this.gameObject);
    }

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the player isn't holding an object, spawn the new plant in their hand
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Spawn plant and start it's growth
            GameObject plant = Instantiate(spawnedPlant);
            plant.GetComponent<Plant>().StartGrowth();

            //Disable plant collision
            plant.GetComponent<BoxCollider>().enabled = false;

            //Tell the player to carry the plant
            GameManager.instance.player.GetComponent<Player>().heldObject = plant;

            //Destory this object
            Destroy(this.gameObject);
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the player isn't holding an object
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
            return true;

        return false;
    }
}
