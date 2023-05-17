using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;
    
    //Game State Variables
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        LevelMessage,
        UpgradeMenu,
        LevelWon,
        LevelLost
    }
    public GameState gameState;
    public int levelNumber = 0;

    //Gameplay Variables
    private float timeLimit;
    private float timer;
    public int money;
    public int moneyTotal = 0;
    private int moneyGoal;

    //Level Settings
    public bool spawnRose = false;
    public bool spawnCactus = false;
    public bool spawnLily = false;

    [Header("References")]
    public GameObject player;
    public GameObject wateringCan;

    [Header("HUD")]
    public GameObject mainMenuHUD;
    public GameObject gameplayHUD;
    public GameObject PauseHUD;
    public GameObject[] levelMessageHUD;
    public GameObject upgradeHUD;
    public GameObject levelWonHUD;
    public GameObject levelLostHUD;
    public GameObject PlantBook;
    public Image timerImage;

    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    void Start()
    {
        InitialiseGame();
    }

    private void Update()
    {
        //Game State switch block
        switch (gameState)
        {
            case GameState.MainMenu:
                MainMenu();
                break;

            case GameState.Gameplay:
                Gameplay();
                break;

            case GameState.Paused:
                Paused();
                break;

            case GameState.LevelMessage:
                LevelMessage();
                break;

            case GameState.UpgradeMenu:
                UpgradeMenu();
                break;

            case GameState.LevelWon:
                LevelWon();
                break;

            case GameState.LevelLost:
                LevelLost();
                break;
        }
    }

    //Initialise the game
    private void InitialiseGame()
    {
        //Inialise Varaibles
        levelNumber = 1;

        //Initial Game State
        gameState = GameState.MainMenu;
    }

    //Game State - Main Menu
    private void MainMenu()
    {
        //Show the Main Menu HUD
        if (mainMenuHUD.activeSelf == false)
            mainMenuHUD.SetActive(true);

        //If 'Escape' key is pressed, exit the application
        if (Input.GetKeyDown("escape"))
            Application.Quit();

        //Wait for player to start the game
        if (Input.GetKeyDown("space"))
        {
            LevelSetup(levelNumber);

            mainMenuHUD.SetActive(false);

            gameState = GameState.LevelMessage;
        }
    }

    //Game State - Gameplay
    private void Gameplay()
    {
        //Show the Gameplay HUD
        if (gameplayHUD.activeSelf == false)
            gameplayHUD.SetActive(true);

        //If 'Escape' key is pressed, pause the game
        if (Input.GetKeyDown("escape"))
        {
            PauseHUD.SetActive(true);

            gameState = GameState.Paused;
            return;
        }

        //Decrease timer
        UpdateTimer();

        //Update HUD elements
        UpdateGameplayHUD();

        //Check if time has expired
        if (timer <= 0)
        {
            //Check if player has won the level...
            if (money >= moneyGoal)
            {
                gameplayHUD.SetActive(false);

                //TODO - levelNumber == 5: End Game

                // REMOVED FOR PLAYTESTING BALANCE moneyTotal += money - moneyGoal;
                moneyTotal += money;

                gameState = GameState.LevelWon;
            }
            //... or lost the level
            else
            {
                gameplayHUD.SetActive(false);

                gameState = GameState.LevelLost;
            }
        }
    }

    //Game State - Paused
    private void Paused()
    {
        //Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //If 'Escape' key is pressed, un-pause the game
        if (Input.GetKeyDown("escape"))
        {
            PauseHUD.SetActive(false);

            //Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameState = GameState.Gameplay;
            return;
        }
    }

    //Game State - Level Message
    private void LevelMessage()
    {
        //Show the Level Message HUD
        if (levelMessageHUD[levelNumber - 1].activeSelf == false)
            levelMessageHUD[levelNumber - 1].SetActive(true);

        //Wait for player to transition to the upgrade screen
        if (Input.GetKeyDown("space"))
        {
            LevelSetup(levelNumber);

            levelMessageHUD[levelNumber - 1].SetActive(false);

            gameState = GameState.UpgradeMenu;
        }
    }

    //Game State - Upgrade
    private void UpgradeMenu()
    {
        //Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Show the Upgrade HUD
        if (upgradeHUD.activeSelf == false)
            upgradeHUD.SetActive(true);

        //Wait for player to start the level
        if (Input.GetKeyDown("space"))
        {
            LevelSetup(levelNumber);

            upgradeHUD.SetActive(false);

            //Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameState = GameState.Gameplay;
        }
    }

    //Game State - Level Won
    private void LevelWon()
    {
        //Show the Level Won HUD
        if (levelWonHUD.activeSelf == false)
            levelWonHUD.SetActive(true);

        //Wait for player to start the next level
        if (Input.GetKeyDown("space"))
        {
            levelNumber++;
            LevelSetup(levelNumber);

            levelWonHUD.SetActive(false);

            gameState = GameState.LevelMessage;
        }

        //If 'Escape' key is pressed, return to the main menu
        if (Input.GetKeyDown("escape"))
        {
            //Functaionality is equivalent to simply restarting the game:
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Game State - Level Lost
    private void LevelLost()
    {
        //Show the Level Lost HUD
        if (levelLostHUD.activeSelf == false)
            levelLostHUD.SetActive(true);

        //Wait for the player to restart the same level
        if (Input.GetKeyDown("space"))
        {
            LevelSetup(levelNumber);

            levelLostHUD.SetActive(false);

            gameState = GameState.LevelMessage;
        }

        //If 'Escape' key is pressed, return to the main menu
        if (Input.GetKeyDown("escape"))
        {
            //Functaionality is equivalent to simply restarting the game:
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Decreases time left
    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }

    //Add specified amount to the money counter, negative to subract
    public void AddMoney(int value)
    {
        money += value;
    }

    //Updates HUD values
    private void UpdateGameplayHUD()
    {
        if (Input.GetMouseButton(1))
        {
            PlantBook.SetActive(true);
        }
        else 
            PlantBook.SetActive(false);

        gameplayHUD.transform.Find("MoneyCounter").GameObject().GetComponent<TextMeshProUGUI>().text = "$" + money + " / $" + moneyGoal;
        timerImage.fillAmount = timer / timeLimit;
    }

    //Set up the level before level starts
    private void LevelSetup(int levelNumber)
    {
        //Reset player position & rotation
        player.GetComponent<Player>().Respawn();

        //Reset watering can position & rotation
        wateringCan.GetComponent<WateringCan>().Respawn();

        //Delete Plants
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject plant in plants)
            Destroy(plant);

        //Remove object references from placements
        GameObject[] placements = GameObject.FindGameObjectsWithTag("Placement");
        for (int i = 0; i < placements.Length; i++)
            placements[i].GetComponent<Placement>().placedObject = null;

        //Level settings switch block
        //NOTE: enabling/disabling a feature boolean enables/disables that features for all subsequent levels
        switch (levelNumber)
        {
            case 1:
                //Plants
                spawnRose = true;

                //Timer & Money
                timeLimit = 60;
                money = 0;
                moneyGoal = 40;
                break;

            case 2:
                //Plants
                spawnRose = true;
                spawnCactus = true;
                spawnLily= true;

                //Timer & Money 
                timeLimit = 120;
                money = 0;
                moneyGoal = 100;
                break;

            case 3:
                //Timer & Money 
                spawnRose = true;
                spawnCactus = true;
                spawnLily = true;

                timeLimit = 90;
                money = 0;
                moneyGoal = 100;
                break;

            case 4:
                //Timer & Money 
                timeLimit = 90;
                money = 0;
                moneyGoal = 100;
                break;

            case 5:
                //TODO - Implement Weather
                //weatherEnabled = true;

                //Timer & Money 
                timeLimit = 240;
                money = 0;
                moneyGoal = 300;
                break;
        }

        //Gameplay Variables
        timer = timeLimit;
    }
}
