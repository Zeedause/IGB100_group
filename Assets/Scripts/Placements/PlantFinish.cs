using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;
using static UnityEngine.ParticleSystem;

public class PlantFinish : Interactable
{
    public ParticleSystem particles;

    //If the player successfully interacts with this object
    public override void Interact()
    {
        //If the object held by the player is a fully grown plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<Plant>() && heldObject.GetComponent<Plant>().growthState == GrowthState.Grown)
        {
            //Play 'Ding' sound
            GameManager.instance.audioManager.Play("Ding");

            particles.Play();

            //Update the money counter with sell value
            GameManager.instance.AddMoney(GameManager.instance.player.GetComponent<Player>().heldObject.GetComponent<Plant>().sellValue);

            //Destroy object and remove reference from player
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public override bool IsValidInteractable()
    {
        //If the object held by the player is a fully grown plant
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.GetComponent<Plant>() && heldObject.GetComponent<Plant>().growthState == GrowthState.Grown)
            return true;

        return false;
    }
}
