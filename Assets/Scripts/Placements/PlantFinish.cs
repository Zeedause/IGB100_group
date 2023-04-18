using System.Collections;
using System.Collections.Generic;
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
            //Update the money counter will sell value
            GameManager.instance.AddMoney(GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Plant>().sellValue);

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }
}
