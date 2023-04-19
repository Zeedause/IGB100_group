using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedlingSpawner : MonoBehaviour
{
    private Placement placement;
    public GameObject spawnRose;
    public GameObject spawnCactus;

    private void Start()
    {
        placement = this.gameObject.GetComponent<Placement>();
    }

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.InProgress)
            return;

        if (!placement.placedObject)
        {
            SpawnPlant();
        }
    }

    //Creates a new plant at the spawner's position & rotation
    private void SpawnPlant()
    {
        // select which plant to spawn, 33% chance to spawn cactus (int overload for Random.Range has exclusive maxValue)
        int spawnID = UnityEngine.Random.Range(1, 4);
        if(spawnID < 3)
        {
            placement.placedObject = Instantiate(spawnRose, transform.position, transform.rotation);
            placement.placedObject.GetComponent<Plant>().placement = this.gameObject;
        }
        else {
            placement.placedObject = Instantiate(spawnCactus, transform.position, transform.rotation);
            placement.placedObject.GetComponent<Plant>().placement = this.gameObject;
        }
    }
}
