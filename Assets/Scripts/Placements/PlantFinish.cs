using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;

public class PlantFinish : Interactable
{
    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<Plant>() && heldObject.GetComponent<Plant>().growthState == GrowthState.Grown)
        {
            //Play 'Ding' sound
            GameManager.instance.audioManager.Play("Ding");

            //Update the money counter with sell value
            GameManager.instance.AddMoney(GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Plant>().sellValue);

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }
}
