using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI player1CurrentGamePoints;
    public TextMeshProUGUI player2CurrentGamePoints;
    public TextMeshProUGUI player1FullPoints;
    public TextMeshProUGUI player2FullPoints;
    
    void Start()
    {
        Update();
    }

    void Update()
    {
        if (ScoreManager.GetInstance().GetWinnerId() == 0)
        {
            string[] results = ScoreManager.GetInstance().GetCurrentGameResults();
            player1CurrentGamePoints.text = results[0];
            player2CurrentGamePoints.text = results[1];
            
            string[] fullResults = ScoreManager.GetInstance().GetPartialResults();
            player1CurrentGamePoints.text = results[0];
            player2CurrentGamePoints.text = results[1];
        }
    }
}
