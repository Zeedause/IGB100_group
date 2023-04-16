using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedlingSpawner : MonoBehaviour
{
    private Placement placement;
    public GameObject spawnPlant;

    private void Start()
    {
        placement = this.gameObject.GetComponent<Placement>();
    }

    private void Update()
    {
        if (!placement.placedObject)
        {
            SpawnPlant();
        }
    }

    //Creates a new plant at the spawner's position & rotation
    private void SpawnPlant()
    {
        placement.placedObject = Instantiate(spawnPlant, transform.position, transform.rotation);
        placement.placedObject.GetComponent<Plant>().placement = this.gameObject;
    }
}
