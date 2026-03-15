using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject endPanel;

 
    [SerializeField] private TMP_Text scoreTeamAText;
    [SerializeField] private TMP_Text scoreTeamBText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text timerText;

   
    [SerializeField] private TMP_Text endMessageText;

    public void ShowStartMenu()
    {
        if (startPanel != null) startPanel.SetActive(true);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (endPanel != null) endPanel.SetActive(false);
    }

    public void ShowHUD()
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);
        if (endPanel != null) endPanel.SetActive(false);
    }

    public void ShowEndMenu(string message)
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (endPanel != null) endPanel.SetActive(true);

        if (endMessageText != null)
            endMessageText.text = message;
    }

    public void UpdateScore(int scoreA, int scoreB)
    {
        if (scoreTeamAText != null)
            scoreTeamAText.text = "TeamA: " + scoreA;

        if (scoreTeamBText != null)
            scoreTeamBText.text = "TeamB: " + scoreB;
    }

    public void UpdateTurn(Teams currentTurn)
    {
        if (turnText != null)
            turnText.text = "Turno: " + currentTurn;
    }

    public void UpdateTimer(float remainingTime)
    {
        if (timerText != null)
        {
            float safeTime = Mathf.Max(0f, remainingTime);
            //Formateo
            timerText.text = "Tiempo: " + safeTime.ToString("F1") + " s";
        }
    }
}