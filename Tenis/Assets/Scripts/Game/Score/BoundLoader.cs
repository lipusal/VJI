using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundLoader : MonoBehaviour
{
    // court delimiters
    public Transform eastCourtSide;
    public Transform westCourtSide;
    public Transform southCourtSide;
    public Transform northCourtSide;
    
    // service delimiters
    public Transform southServiceWall;
    public Transform southEastServiceWall;
    public Transform southWestServiceWall;
    public Transform southMiddleServiceWall;
    public Transform northServiceWall;
    public Transform northEastServiceWall;
    public Transform northWestServiceWall;
    public Transform northMiddleServiceWall;

    // players
    public PlayerLogic player1;
    public AIPlayer player2;
    void Start()
    {
        ScoreManager.GetInstance().loadReferee(eastCourtSide.position, westCourtSide.position,
                                            southCourtSide.position, northCourtSide.position,
                                            southServiceWall.position, southEastServiceWall.position,
                                            southWestServiceWall.position, southMiddleServiceWall.position,
                                            northServiceWall.position, northEastServiceWall.position,
                                            northWestServiceWall.position, northMiddleServiceWall.position,
                                            player1, player2);
        Debug.Log("creating referee");
    }
    
}
