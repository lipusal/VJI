using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI player1Points;

    public TextMeshProUGUI player2Points;
    
    void Start()
    {
        string[] results = ScoreManager.GetInstance().ShowPartialResults();
        player1Points.text = results[0];
        player2Points.text = results[1];
    }

    void Update()
    {
        if (ScoreManager.GetInstance().GetWinnerId() == 0)
        {
            string[] results = ScoreManager.GetInstance().ShowPartialResults();
            player1Points.text = results[0];
            player2Points.text = results[1];
        }
    }
}
