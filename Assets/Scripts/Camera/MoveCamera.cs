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
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        transform.position = cameraPosition.position;
    }
}
