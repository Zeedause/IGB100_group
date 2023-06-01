using cakeslice;
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

    [Header("Gameplay")]
    public int levelNumber = 0;
    public int money;
    public int moneyTotal = 0;
    private int moneyGoal;
    private float timeLimit;
    private float timer;

    [Header("Level Settings")]
    public bool fertiliser = false;

    [Header("References")]
    public GameObject player;
    public GameObject orderBoard;
    public GameObject wateringCan;
    public GameObject fertiliserSpawner;
    public GameObject camera;

    [Header("HUD")]
    public GameObject mainMenuHUD;
    public GameObject howtoPlayHUD;
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
        //Tell the camera outline effect script whether or not to process for this frame
        if (gameState == GameState.Gameplay)
            camera.GetComponent<OutlineEffect>().updating = true;
        else
            camera.GetComponent<OutlineEffect>().updating = false;

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

        //Initialise UpgradeHUD
        upgradeHUD.GetComponent<UpgradeUI>().Initialise();

        //Initial Game State
        gameState = GameState.MainMenu;
    }

    //Game State - Main Menu
    private void MainMenu()
    {
        //Show the Main Menu HUD
        if (mainMenuHUD.activeSelf == false)
            mainMenuHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartButton()
    {
        upgradeHUD.GetComponent<UpgradeUI>().SaveUpgradeState();

        LevelSetup(levelNumber);

        mainMenuHUD.SetActive(false);
        howtoPlayHUD.SetActive(false);

        gameState = GameState.LevelMessage;
    }

    public void HowtoButton()
    {
        if (PauseHUD.activeSelf == true)
            PauseHUD.SetActive(false);

        //Show the Gameplay HUD
        if (howtoPlayHUD.activeSelf == false)
            howtoPlayHUD.SetActive(true);
    }
    public void BackButton()
    {
        if (howtoPlayHUD.activeSelf == true)
            howtoPlayHUD.SetActive(false);

        if (gameState == GameState.MainMenu)
        {
            mainMenuHUD.SetActive(true);
        }
        if (gameState == GameState.Paused)
        {
            PauseHUD.SetActive(true);
        }
    }

    public void QuitButton()
    {
        Application.Quit();
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

        //If 'Escape' key is pressed (and not looking at the 'how to play' HUD), un-pause the game
        if (Input.GetKeyDown("escape") && howtoPlayHUD.activeSelf == false)
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
    }

    public void GoToUpgradeMenuButton()
    {
        levelMessageHUD[levelNumber - 1].SetActive(false);

        gameState = GameState.UpgradeMenu;
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
    }

    public void GoToGameplayButton()
    {
        //LevelSetup(levelNumber);

        upgradeHUD.SetActive(false);

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameState = GameState.Gameplay;
    }

    //Game State - Level Won
    private void LevelWon()
    {
        //Show the Level Won HUD
        if (levelWonHUD.activeSelf == false)
            levelWonHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        /*
        //If 'Escape' key is pressed, return to the main menu
        if (Input.GetKeyDown("escape"))
        {
            //Functaionality is equivalent to simply restarting the game:
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
    }

    public void NextLevelButton()
    {
        //Save current upgrade state
        upgradeHUD.GetComponent<UpgradeUI>().SaveUpgradeState();

        //Set up next level
        levelNumber++;
        LevelSetup(levelNumber);

        //Hide this HUD
        levelWonHUD.SetActive(false);

        //Change game state
        gameState = GameState.LevelMessage;
    }

    //Game State - Level Lost
    private void LevelLost()
    {
        //Show the Level Lost HUD
        if (levelLostHUD.activeSelf == false)
            levelLostHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitMenuButton()
    {
        //Functaionality is equivalent to simply restarting the game:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Restarts the current level
    public void RestartLevel()
    {
        //Revert upgrade state
        upgradeHUD.GetComponent<UpgradeUI>().RevertUpgradeState();

        //Reset level
        LevelSetup(levelNumber);

        //Hide this HUD
        levelLostHUD.SetActive(false);

        //Change game state
        gameState = GameState.LevelMessage;
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

        //Clear & reset order board
        orderBoard.GetComponent<OrderBoard>().ResetBoard();

        //Delete Plants
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject plant in plants)
            Destroy(plant);

        //Remove object references from placements
        GameObject[] placements = GameObject.FindGameObjectsWithTag("Placement");
        for (int i = 0; i < placements.Length; i++)
            placements[i].GetComponent<Placement>().placedObject = null;

        //Level settings switch block
        //NOTE: chaning a feature boolean or value changes that features for all subsequent levels
        switch (levelNumber)
        {
            case 1:
                //Plants
                orderBoard.GetComponent<OrderBoard>().roseWeight = 100f;
                orderBoard.GetComponent<OrderBoard>().cactusWeight = 0f;
                orderBoard.GetComponent<OrderBoard>().lilyWeight = 0f;

                //Fertiliser
                fertiliserSpawner.SetActive(false);

                //Timer & Money
                timeLimit = 80;
                money = 0;
                moneyGoal = 30;
                break;

            case 2:
                //Plants
                orderBoard.GetComponent<OrderBoard>().roseWeight = 33f;
                orderBoard.GetComponent<OrderBoard>().cactusWeight = 20f;
                orderBoard.GetComponent<OrderBoard>().lilyWeight = 15f;

                //Fertiliser
                fertiliserSpawner.SetActive(false);

                //Timer & Money 
                timeLimit = 120;
                money = 0;
                moneyGoal = 85;
                break;

            case 3:
                orderBoard.GetComponent<OrderBoard>().roseWeight = 33f;
                orderBoard.GetComponent<OrderBoard>().cactusWeight = 20f;
                orderBoard.GetComponent<OrderBoard>().lilyWeight = 15f;

                //Fertiliser
                fertiliserSpawner.SetActive(true);

                //Timer & Money 
                timeLimit = 100;
                money = 0;
                moneyGoal = 80;
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
