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
}
