using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;

public class PlantDelete : Interactable
{
    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a plant or fertiliser
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && (heldObject.GetComponent<Plant>() || heldObject.GetComponent<Fertiliser>()))
        {
            //Play 'Bin' sound
            GameManager.instance.audioManager.Play("Bin");

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the object held by the player is a plant or fertiliser
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && (heldObject.GetComponent<Plant>() || heldObject.GetComponent<Fertiliser>()))
            return true;

        return false;
    }
}
