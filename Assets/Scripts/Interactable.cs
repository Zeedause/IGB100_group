using cakeslice;
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

    //Returns whether or not the object is valid to be interacted with, given what the player is holding
    public virtual bool IsValidInteractable()
    {
        //Default return as subclasses will override
        return true;
    }

    //Set whether the outline of the object is active
    public void SetOutlineActive(bool b)
    {
        //Initialise new list of objects, with this object already in it
        List<GameObject> objects = new List<GameObject>() { this.gameObject };

        //Loop until all objects & (recursive) child objects have been processed
        while (objects.Count > 0)
        {
            //Get next GameObject
            GameObject o = objects[0];

            //Find all children and add to objects list
            foreach (Transform child in o.transform)
                objects.Add(child.gameObject);

            //Toggle objects outline componenet (if exists)
            if (o.GetComponent<Outline>())
                o.GetComponent<Outline>().enabled = b;

            //Remove current object from objects list
            objects.Remove(o);
        }
    }

    //Set the color of the outline on this object
    public void SetOutlineColor(int color)
    {
        //Initialise new list of objects, with this object already in it
        List<GameObject> objects = new List<GameObject>() { this.gameObject };

        //Loop until all objects & (recursive) child objects have been processed
        while (objects.Count > 0)
        {
            //Get next GameObject
            GameObject o = objects[0];

            //Find all children and add to objects list
            foreach (Transform child in o.transform)
                objects.Add(child.gameObject);

            //Change the color of the outline
            if (o.GetComponent<Outline>())
                o.GetComponent<Outline>().color = color;

            //Remove current object from objects list
            objects.Remove(o);
        }
    }
}
