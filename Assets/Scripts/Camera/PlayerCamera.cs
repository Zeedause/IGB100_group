using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Don't process unless gameState == GameState.InProgress
        if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
            return;

        //Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X");// * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y");// * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        GameManager.instance.player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
