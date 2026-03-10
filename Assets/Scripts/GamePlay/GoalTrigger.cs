using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    
    //equipo al que se le suma el gol
    [SerializeField] private Teams scoringTeam;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        GameManager.Instance.RegisterGoal(scoringTeam);
    }
}