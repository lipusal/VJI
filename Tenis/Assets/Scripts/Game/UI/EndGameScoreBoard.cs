using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI player1Points;
    public TextMeshProUGUI player2Points;

    // Start is called before the first frame update
    void Start()
    {
        TenisSet[] sets = ScoreManager.GetInstance().GetFinalResult();
        int quantity = sets.Length;
        string player1Result = "  ";
        string player2Result = "  ";

        for (int i = 0; i < quantity; i++)
        {
            player1Result = player1Result + sets[i].GetResult()[0] + "  ";
            player2Result = player2Result + sets[i].GetResult()[1] + "  ";
        }

        player1Points.text = player1Result;
        player2Points.text = player2Result;
    }
    
}
