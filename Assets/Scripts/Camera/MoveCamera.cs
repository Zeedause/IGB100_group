using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    /* Camera rotation
     * https://www.youtube.com/watch?v=f473C43s8nE
     */

    public Transform cameraPosition;

    
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
