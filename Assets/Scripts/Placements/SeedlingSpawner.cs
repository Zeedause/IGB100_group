using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedlingSpawner : Placement
{
    public GameObject rose;
    public GameObject cactus;
    public GameObject lily;

    public float roseWeight = 33;
    public float cactusWeight = 20;
    public float lilyWeight = 20;

    private void Update()
    {
        //Don't process unless gameState == GameState.Gameplay
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        if (!placedObject)
        {
            SpawnRandomPlant();
        }
    }

    //Randomly chooses a plant to spawn based on plant weightings
    private void SpawnRandomPlant()
    {
        float range = 0f;
        if (GameManager.instance.spawnRose)
            range += roseWeight;
        if (GameManager.instance.spawnCactus)
            range += cactusWeight;
        if (GameManager.instance.spawnLily)
            range += lilyWeight;

        float rv = Random.Range(0, range);

        //Rose
        if (GameManager.instance.spawnRose)
        {
            if (rv <= roseWeight)
            {
                SpawnPlant(rose);
                return;
            }
            rv -= roseWeight;
        }


        //Cactus
        if (GameManager.instance.spawnCactus)
        {
            if (rv <= cactusWeight)
            {
                SpawnPlant(cactus);
                return;
            }
            rv -= cactusWeight;
        }

        //Lily
        if (GameManager.instance.spawnLily)
        {
            if (rv <= lilyWeight)
            {
                SpawnPlant(lily);
                return;
            }
        }

        //This should be un-reachable
        Debug.Log("Something has gone wrong with plant spawning!");
    }

    //Creates a new plant at the spawner's position & rotation
    private void SpawnPlant(GameObject plant)
    {
        placedObject = Instantiate(plant, transform.position, transform.rotation);
        placedObject.GetComponent<Plant>().placement = this.gameObject;
    }
}
