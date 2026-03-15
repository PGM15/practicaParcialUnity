using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Patron singleton

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private BallController ball;
    [SerializeField] private List<DiscController> discs = new List<DiscController>();
    [SerializeField] private UIManager uiManager;
    
    [SerializeField] private Teams currentTurn = Teams.TeamA;
    [SerializeField] private GameState gameState = GameState.StartMenu;
    [SerializeField] private float matchDuration = 30f;
    
    [SerializeField] private int scoreTeamA = 0;
    [SerializeField] private int scoreTeamB = 0;
    
    [SerializeField] private float minResolutionTime = 1f;
    [SerializeField] private float maxResolutionTime = 5f;
    [SerializeField] private float linearStopThreshold = 0.08f;
    [SerializeField] private float angularStopThreshold = 0.08f;

    [SerializeField] private Vector3 ballStartPosition;

    //Quiero que la bola empiece donde la pongo en el editor de unity
    private bool ballStartPositionCaptured = false;

    private float shotResolutionTimer = 0f;
    private float remainingTime;
    private bool matchStarted = false;

    public Teams CurrentTurn => currentTurn;
    public GameState CurrentGameState => gameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (discs.Count == 0)
        {
            DiscController[] foundDiscs = FindObjectsByType<DiscController>(FindObjectsSortMode.None);
            discs.AddRange(foundDiscs);
        }

        if (ball != null && !ballStartPositionCaptured)
        {
            ballStartPosition = ball.transform.position;
            ballStartPositionCaptured = true;
        }

        remainingTime = matchDuration;
        matchStarted = false;
        gameState = GameState.StartMenu;

        ForceStopAllMovingObjects();
        UpdateUI();

        if (uiManager != null)
            uiManager.ShowStartMenu();
    }

    private void Update()
    {
        if (matchStarted && gameState != GameState.GameOver)
            UpdateMatchTimer();

        if (gameState == GameState.ResolvingShot)
            UpdateShotResolution();
    }

    public void StartMatch()
    {
        scoreTeamA = 0;
        scoreTeamB = 0;
        currentTurn = Teams.TeamA;
        remainingTime = matchDuration;
        shotResolutionTimer = 0f;

        matchStarted = true;
        gameState = GameState.WaitingForShot;

        ResetBallToStart();
        ForceStopAllMovingObjects();
        UpdateUI();

        if (uiManager != null)
            uiManager.ShowHUD();

        Debug.Log("Partida iniciada.");
    }

    private void UpdateMatchTimer()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime < 0f)
            remainingTime = 0f;

        if (uiManager != null)
            uiManager.UpdateTimer(remainingTime);

        if (remainingTime <= 0f)
            EndMatchByTime();
    }

    //Hay que cumplir varias condiciones para poder disparar la chapa
    public bool CanShootDisc(DiscController disc)
    {
        if (!matchStarted)
            return false;

        if (gameState != GameState.WaitingForShot)
            return false;

        if (disc == null)
            return false;

        return disc.Team == currentTurn;
    }

    public void NotifyShotStarted()
    {
        if (gameState != GameState.WaitingForShot)
            return;

        gameState = GameState.ResolvingShot;
        shotResolutionTimer = 0f;
    }

    private void UpdateShotResolution()
    {
        shotResolutionTimer += Time.deltaTime;

        bool everythingStopped = AreAllObjectsStopped();

        if (shotResolutionTimer >= minResolutionTime && everythingStopped)
        {
            EndTurn();
            return;
        }

        if (shotResolutionTimer >= maxResolutionTime)
        {
            ForceStopAllMovingObjects();
            EndTurn();
        }
    }

    private bool AreAllObjectsStopped()
    {
        if (ball != null && !ball.IsReallyStopped(0.08f, 0.08f, 0.03f))
            return false;

        foreach (DiscController disc in discs)
        {
            if (disc != null && disc.IsMoving(linearStopThreshold, angularStopThreshold))
                return false;
        }

        return true;
    }

    private void ForceStopAllMovingObjects()
    {
        if (ball != null)
            ball.ForceStop();

        foreach (DiscController disc in discs)
        {
            if (disc != null)
                disc.ForceStop();
        }
    }

    private void EndTurn()
    {
        ChangeTurn();
        gameState = GameState.WaitingForShot;
        shotResolutionTimer = 0f;
        UpdateUI();
    }

    private void ChangeTurn()
    {
        currentTurn = currentTurn == Teams.TeamA ? Teams.TeamB : Teams.TeamA;
    }

    public void RegisterGoal(Teams scoringTeam)
    {
        if (gameState == GameState.GameOver)
            return;

        if (!matchStarted)
            return;

        if (scoringTeam == Teams.TeamA)
            scoreTeamA++;
        else
            scoreTeamB++;

        ForceStopAllMovingObjects();
        UpdateUI();
        EndMatchByGoal(scoringTeam);
    }

    private void EndMatchByGoal(Teams winner)
    {
        matchStarted = false;
        gameState = GameState.GameOver;

        ForceStopAllMovingObjects();

        if (uiManager != null)
            uiManager.ShowEndMenu("GOOOOOOOOL ha ganado " + winner);
    }

    private void EndMatchByTime()
    {
        if (gameState == GameState.GameOver)
            return;

        matchStarted = false;
        gameState = GameState.GameOver;

        ForceStopAllMovingObjects();

        if (uiManager != null)
        {
            uiManager.UpdateTimer(0f);
            uiManager.ShowEndMenu("Se acabó el tiempo. No ha ganado ningún equipo.");
        }
    }

    private void ResetBallToStart()
    {
        if (ball != null)
            ball.ResetBall(ballStartPosition);
    }

    private void UpdateUI()
    {
        if (uiManager == null)
            return;

        uiManager.UpdateScore(scoreTeamA, scoreTeamB);
        uiManager.UpdateTurn(currentTurn);
        uiManager.UpdateTimer(remainingTime);
    }

    public void RestartMatch()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}