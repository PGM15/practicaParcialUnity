using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    //PATRON SINGLETON
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private BallController ball;

    [Header("Score")]
    [SerializeField] private int scoreTeamA = 0;
    [SerializeField] private int scoreTeamB = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterGoal(Teams scoringTeam)
    {
        if (scoringTeam.Equals(Teams.TeamA))
            scoreTeamA++;
        else
            scoreTeamB++;

        Debug.Log($"Gol de {scoringTeam}. Marcador: TeamA {scoreTeamA} - TeamB {scoreTeamB}");

        ResetAfterGoal();
    }

    private void ResetAfterGoal()
    {
        if (ball != null)
        {
            ball.ResetBall(new Vector3(0f, 0.5f, 0f));
        }

        // Más adelante recolocaremos también las chapas.
    }
}