using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public GameObject player;
    public int lossMultiplier = 2;

    public enum GameState
    {
        Starting,
        InProgress,
        FinishedWin,
        FinishedFail
    }
    public GameState gameState;
    private float timer;
    public float timeLimit = 120; //Seconds
    public int money = 0;
    public int moneyGoal = 1000;

    [Header("HUD")]
    public GameObject startingHUD;
    public GameObject inProgressHUD;
    public GameObject finishedWinHUD;
    public GameObject finishedFailHUD;

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
        //Check gamweState
        if (gameState == GameState.Starting)
        {
            if (Input.GetKey("space"))
                StartGame();
        }
        else if (gameState == GameState.FinishedWin || gameState == GameState.FinishedFail)
        {
            if (Input.GetKey("space"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else //gameState == GameState.InProgress
        {
            UpdateTimer();

            UpdateHUD();
            
            if (timer <= 0)
            {
                if (money >= moneyGoal)
                    FinishGameWin();
                else
                    FinishGameFail();
            }
        }
    }

    //Initialise the game
    private void InitialiseGame()
    {
        gameState = GameState.Starting;
        timer = timeLimit;
        money = 0;
        startingHUD.SetActive(true);
    }

    //Start the game
    private void StartGame()
    {
        gameState = GameState.InProgress;

        startingHUD.SetActive(false);
        inProgressHUD.SetActive(true);
    }

    //End game with win
    private void FinishGameWin()
    {
        gameState = GameState.FinishedWin;

        inProgressHUD.SetActive(false);
        finishedWinHUD.SetActive(true);
    }

    //End game with fail
    private void FinishGameFail()
    {
        gameState = GameState.FinishedFail;

        inProgressHUD.SetActive(false);
        finishedFailHUD.SetActive(true);
    }

    //Decreases time left
    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }

    //Add specified amount to the money counter
    public void AddMoney(int value)
    {
        money += value;
    }

    public void LoseMoney(int value)
    {
        money -= value / lossMultiplier;
    }

    //Updates HUD values
    private void UpdateHUD()
    {
        inProgressHUD.transform.Find("MoneyCounter").GameObject().GetComponent<TextMeshProUGUI>().text = "Money: $" + money + "/$" + moneyGoal;
        inProgressHUD.transform.Find("Timer").GameObject().GetComponent<TextMeshProUGUI>().text = "Time Left:\n\r" + MathF.Ceiling(timer);
    }
}
