using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public bool active;
    public GameObject plant;

    private void Start()
    {
        SetActive(false);
    }

    private void Update()
    {
        if (!active && IsValidPlacement())
            SetActive(true);
        else if (active && !IsValidPlacement())
            SetActive(false);
    }

    //Check if this object is valid to be placed on
    private bool IsValidPlacement()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        return !plant && heldObject && GameManager.instance.player.GetComponent<Player>().heldObject.CompareTag("Plant");
    }

    //If the player successfully interacts with this object
    public void Interact()
    {
        GameObject heldObject = GameManager.instance.player.GetComponent<Player>().heldObject;
        if (heldObject && heldObject.CompareTag("Plant"))
        {
            GameManager.instance.player.GetComponent<Player>().heldObject = null;
            plant = heldObject;
            heldObject.GetComponent<Plant>().placement = this.gameObject;

            heldObject.transform.position = transform.position;
            heldObject.transform.rotation = transform.rotation;

            heldObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    //Turns this object on or off
    public void SetActive(bool b)
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = b;
        this.gameObject.GetComponent<BoxCollider>().enabled = b;
        active = b;
    }
}
