using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject placement;

    //If the player successfully interacts with this object
    public void Interact()
    {
        if (!GameManager.instance.player.GetComponent<Player>().heldObject)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.player.GetComponent<Player>().heldObject = this.gameObject;

            if (placement)
            {
                placement.GetComponent<PlantPlacement>().plant = null;
                placement = null;
            }
        }
    }
}
