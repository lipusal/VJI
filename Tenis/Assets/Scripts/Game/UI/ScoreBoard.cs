using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI[] player1CurrentGamePoints;
    public TextMeshProUGUI[] player2CurrentGamePoints;
    public TextMeshProUGUI[] player1FullPoints;
    public TextMeshProUGUI[] player2FullPoints;

    void Update()
    {
        string[] results = ScoreManager.GetInstance().GetCurrentGameResults();
        foreach (var textField in player1CurrentGamePoints)
        {
            textField.text = results[0];
        }
        foreach (var textField in player2CurrentGamePoints)
        {
            textField.text = results[1];
        }
            
        results = ScoreManager.GetInstance().GetFullResults();
        foreach (var textField in player1FullPoints)
        {
            textField.text = results[0];
        }
        foreach (var textField in player2FullPoints)
        {
            textField.text = results[1];
        }
    }
}
