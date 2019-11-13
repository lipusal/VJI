using FrameLord;
using Game.Events;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI player1CurrentGamePoints;
    public TextMeshProUGUI player2CurrentGamePoints;

    void Start()
    {
        GameEventDispatcher.Instance.AddListener(PointEvent.NAME, Callback);
    }

    private void Callback(object sender, GameEvent e)
    {
        PointEvent pe = (PointEvent) e;
        player1CurrentGamePoints.text = pe.NewCurrentGamePoints[0];
        player2CurrentGamePoints.text = pe.NewCurrentGamePoints[1];
    }
}
