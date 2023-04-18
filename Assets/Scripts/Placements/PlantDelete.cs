using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;

public class PlantDelete : MonoBehaviour
{
    //If the player successfully interacts with this object
    public void Interact()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.CompareTag("Plant"))
        {
            Destroy(GameManager.instance.player.GetComponent<Player>().heldObject);
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
        }
    }
}
