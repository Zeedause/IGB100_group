using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Plant;

public class PlantFinish : MonoBehaviour
{
    //If the player successfully interacts with this object
    public void Interact()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.CompareTag("Plant") && heldObject.GetComponent<Plant>().growthState == GrowthState.Grown)
        {
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }
}
