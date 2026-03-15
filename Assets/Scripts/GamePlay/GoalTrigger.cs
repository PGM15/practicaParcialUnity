using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private Teams scoringTeam;

    //Solo es gol si el que atraviesa la porteria de trigger es el balon
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        //Cumplimos con patron singleton y abstracción
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterGoal(scoringTeam);
    }
}