using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //If the player successfully interacts with this object
    public virtual void Interact()
    {
        //Empty as subclasses will override
    }
}
