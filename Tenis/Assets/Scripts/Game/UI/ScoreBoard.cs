using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI player1Points;
    public TextMeshProUGUI player2Points;
    
    void Start()
    {
        Update();
    }

    void Update()
    {
        List<string>[] score = ScoreManager.GetInstance().GetScore();
        player1Points.text = $"P1 - {BuildScoreString(score[0])}";
        player2Points.text = $"P2 - {BuildScoreString(score[1])}";
    }

    private static string BuildScoreString(List<string> score)
    {
        var result = $"<b>{score[0]}</b>" + "\t"; // Current game points
        for (var i = 1; i < score.Count; i++)
        {
            result += score[i].PadLeft(2); // TODO highlight winner per previous set
            if (i < score.Count - 1)
            {
                result += " ";
            }
        }

        return result;
    }
}
