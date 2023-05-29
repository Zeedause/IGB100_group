using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //'Resume' button click
    public void Resume()
    {
        GameManager.instance.PauseHUD.SetActive(false);

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.instance.gameState = GameManager.GameState.Gameplay;
    }

    //'Restart' button click
    public void RestartLevel()
    {
        //Restart the level
        GameManager.instance.RestartLevel();

        //Hide this HUD
        GameManager.instance.PauseHUD.SetActive(false);
    }

    //'MainMenu' button click
    public void MainMenu()
    {
        //Functaionality is equivalent to simply restarting the game:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
