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
    public AudioManager audioManager;

    [Header("HUD")]
    public GameObject mainMenuHUD;
    public GameObject howtoPlayHUD;
    public GameObject gameplayHUD;
    public GameObject PauseHUD;
    public GameObject[] levelMessageHUD;
    public GameObject upgradeHUD;
    public GameObject[] levelWonHUD;
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
        //Get audo manager reference
        audioManager = gameObject.GetComponent<AudioManager>();

        //Inialise Varaibles
        levelNumber = 1;

        //Initialise UpgradeHUD
        upgradeHUD.GetComponent<UpgradeUI>().Initialise();

        //Initial Game State
        gameState = GameState.MainMenu;
    }

    #region Game State Methods

    //Game State - Main Menu
    private void MainMenu()
    {
        //Show the Main Menu HUD
        if (mainMenuHUD.activeSelf == false)
            mainMenuHUD.SetActive(true);

        //Unlock & show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            //Show pause HUD
            PauseHUD.SetActive(true);

            //Change game state
            gameState = GameState.Paused;

            //Process no further
            return;
        }

        //Decrement timer
        UpdateTimer();

        //Update HUD elements
        UpdateGameplayHUD();

        //Check if time has expired
        if (timer <= 0)
        {
            //Hide gameplay HUD
            gameplayHUD.SetActive(false);

            //Check if player has won the level...
            if (money >= moneyGoal)
            {
                //TODO - levelNumber == 5: End Game

                //Add earned money to money total
                // REMOVED FOR PLAYTESTING BALANCE moneyTotal += money - moneyGoal;
                moneyTotal += money;

                //Change game state
                gameState = GameState.LevelWon;
            }
            //... or lost the level
            else
            {
                //Change game state
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
            //Hide pause HUD
            PauseHUD.SetActive(false);

            //Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Change game state
            gameState = GameState.Gameplay;
        }
    }

    //Game State - Level Message
    private void LevelMessage()
    {
        //Show the Level Message HUD
        if (levelMessageHUD[levelNumber - 1].activeSelf == false)
            levelMessageHUD[levelNumber - 1].SetActive(true);
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

    //Game State - Level Won
    private void LevelWon()
    {
        //Show the Level Won HUD
        if (levelWonHUD[levelNumber - 1].activeSelf == false)
            levelWonHUD[levelNumber - 1].SetActive(true);

        //Unlock & show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Game State - Level Lost
    private void LevelLost()
    {
        //Show the Level Lost HUD
        if (levelLostHUD.activeSelf == false)
            levelLostHUD.SetActive(true);

        //Unlock & show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    #region Button Events

    //Button Event - Start
    public void StartButton()
    {
        //Save the current upgrade state (initial state)
        upgradeHUD.GetComponent<UpgradeUI>().SaveUpgradeState();

        //Set up level
        LevelSetup(levelNumber);

        //Hide menu HUDs
        mainMenuHUD.SetActive(false);
        howtoPlayHUD.SetActive(false);

        //Change game state
        gameState = GameState.LevelMessage;
    }

    //Button Event - How To Play
    public void HowtoButton()
    {
        //If paused, hide paused HUD
        if (PauseHUD.activeSelf == true)
            PauseHUD.SetActive(false);

        //Show the How To Play HUD
        if (howtoPlayHUD.activeSelf == false)
            howtoPlayHUD.SetActive(true);
    }

    //Button Event - Back
    public void BackButton()
    {
        //Hide How To Play HUD
        if (howtoPlayHUD.activeSelf == true)
            howtoPlayHUD.SetActive(false);

        //If at the main menu, show the main menu HUD
        if (gameState == GameState.MainMenu)
            mainMenuHUD.SetActive(true);
        //Otherwise, if paused, show the paused HUD
        else if (gameState == GameState.Paused)
            PauseHUD.SetActive(true);
    }

    //Button Event - Quit
    public void QuitButton()
    {
        Application.Quit();
    }

    //Button Event - Go To Upgrade Menu
    public void GoToUpgradeMenuButton()
    {
        //Hide level message HUD
        levelMessageHUD[levelNumber - 1].SetActive(false);

        //Change game state
        gameState = GameState.UpgradeMenu;
    }

    //Button Event - Go To Gameplay
    public void GoToGameplayButton()
    {
        //Hide upgrade HUD
        upgradeHUD.SetActive(false);

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Change game state
        gameState = GameState.Gameplay;
    }

    //Button Event - Next level
    public void NextLevelButton()
    {
        //Save current upgrade state
        upgradeHUD.GetComponent<UpgradeUI>().SaveUpgradeState();

        //Set up next level
        levelNumber++;
        LevelSetup(levelNumber);

        //Hide level won HUD
        levelWonHUD[levelNumber].SetActive(false);
        levelWonHUD[levelNumber - 1].SetActive(false);

        //Change game state
        gameState = GameState.LevelMessage;
    }

    //Button Event - Quit To Menu
    public void QuitMenuButton()
    {
        //Functaionality is equivalent to simply restarting the game:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    //Restarts the current level
    public void RestartLevel()
    {
        //Revert upgrade state
        upgradeHUD.GetComponent<UpgradeUI>().RevertUpgradeState();

        //Reset level
        LevelSetup(levelNumber);

        //Hide level lost HUD
        levelLostHUD.SetActive(false);

        //Change game state
        gameState = GameState.LevelMessage;
    }

    //Decrement game timer
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
        //Show/Hide plant book during gameplay
        if (Input.GetMouseButton(1))
            PlantBook.SetActive(true);
        else 
            PlantBook.SetActive(false);

        //Update money counter
        gameplayHUD.transform.Find("MoneyCounter").GameObject().GetComponent<TextMeshProUGUI>().text = "$" + money + " / $" + moneyGoal;

        //Update game timer HUD
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
                moneyGoal = 20;
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
                money = 10;
                moneyGoal = 75;
                break;

            case 3:
                orderBoard.GetComponent<OrderBoard>().roseWeight = 33f;
                orderBoard.GetComponent<OrderBoard>().cactusWeight = 20f;
                orderBoard.GetComponent<OrderBoard>().lilyWeight = 15f;

                //Timer & Money 
                timeLimit = 120;
                money = 15;
                moneyGoal = 95;
                break;

            case 4:
                orderBoard.GetComponent<OrderBoard>().roseWeight = 33f;
                orderBoard.GetComponent<OrderBoard>().cactusWeight = 20f;
                orderBoard.GetComponent<OrderBoard>().lilyWeight = 15f;

                //Fertiliser
                fertiliserSpawner.SetActive(true);

                //Timer & Money 
                timeLimit = 90;
                money = 20;
                moneyGoal = 80;
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
