using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;

public class PlantDelete : Interactable
{
    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<Plant>())
        {
            //TODO - Deduct money from player here intead of when plant dies?
            //       What about if player trashes an alive plant?

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }
}
