using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertiliserSpawner : Interactable
{
    public GameObject fertiliser;

    //Creates a new fertiliser object at the spawner's position & rotation
    public override void Interact()
    {
        //If the player is not holding an object, spawn a fertiliser object and give it to them
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Spawn a fertiliser object and give it to the player
            GameManager.instance.player.GetComponent<Player>().heldObject = Instantiate(fertiliser, transform.position, transform.rotation);
        }
        //Otherwise, if holding a fertiliser object, destory it
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Fertiliser>())
        {
            //Trigger player interaction cooldown
            GameManager.instance.player.GetComponent<Player>().interactionCooldown = true;

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the player is not holding an object
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
            return true;
        //Otherwise, if holding a fertiliser object
        else if (GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Fertiliser>())
            return true;

        return false;
    }
}
