using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderBoard : MonoBehaviour
{
    //Reference Variables
    private GameManager gameManager;

    private GameObject[] pendingOrders = new GameObject[3];

    [Header("Order Spawning")]
    public GameObject roseOrder;
    public GameObject cactusOrder;
    public GameObject lilyOrder;
    public float roseWeight = 50f;
    public float cactusWeight = 25f;
    public float lilyWeight = 25f;
    public GameObject[] spawnLocations;
    public float orderSpawnTime = 5;
    private float orderSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Get references
        gameManager = GameManager.instance;

        //Set timer
        orderSpawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't process unless gameState == GameState.Gameplay
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Operate order spawn timer
        OrderSpawnTimer();
    }

    //Operates the timer to check when to spawn a new order
    private void OrderSpawnTimer()
    {
        //Decrement spawn timer
        orderSpawnTimer -= Time.deltaTime;

        //If timer elapsed ...
        if (orderSpawnTimer <= 0)
        {
            //If there is space to spawn a new order ...
            if (pendingOrders.Count(o => o != null) < pendingOrders.Length)
            {
                //Spawn a new order
                SpawnOrder();
            }
            
            //Reset spawn timer
            orderSpawnTimer = orderSpawnTime;
        }
    }

    //Spawns a new order
    private void SpawnOrder()
    {
        //Calculate random value thresholds
        float[] roseThreshold = new float[] { 0f, roseWeight };
        float[] cactusThreshold = new float[] { roseThreshold[1], roseThreshold[1] + cactusWeight };
        float[] lilyThreshold = new float[] { cactusThreshold[1], cactusThreshold[1] + lilyWeight };

        //Calculate random value
        float range = roseWeight + cactusWeight + lilyWeight;
        float rv = Random.Range(0, range);
        GameObject order;

        //Compare random value to thresholds to determine order spawned
        if (rv > roseThreshold[0] && rv <= roseThreshold[1])
            order = roseOrder;
        else if (rv > cactusThreshold[0] && rv <= cactusThreshold[1])
            order = cactusOrder;
        else if (rv > lilyThreshold[0] && rv <= lilyThreshold[1])
            order = lilyOrder;
        else
        {
            Debug.Log("Something has gone wrong with order spawning!");
            return;
        }

        //Find first available spot to spawn order
        int position;
        for (position = 0; position < pendingOrders.Length; position++)
        {
            if (pendingOrders[position] == null)
                break;
        }

        //Spawn order
        GameObject spawnedOrder = GameObject.Instantiate(order, spawnLocations[position].transform.position, spawnLocations[position].transform.rotation, this.gameObject.transform);
        pendingOrders[position] = spawnedOrder;
    }
}
