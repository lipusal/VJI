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
    
    // service walls
    public GameObject southServiceWall;
    public GameObject southEastServiceWall;
    public GameObject southWestServiceWall;
    public GameObject southMiddleServiceWall;
    public GameObject northServiceWall;
    public GameObject northEastServiceWall;
    public GameObject northWestServiceWall;
    public GameObject northMiddleServiceWall;

    // service box delimiters
    public Transform southServiceDelimiter;
    public Transform eastServiceDelimiter;
    public Transform westServiceDelimiter;
    public Transform northServiceDelimiter;

    // players
    public PlayerLogic player1;
    public AIPlayer player2;
    void Start()
    {
        ScoreManager.GetInstance().loadReferee(eastCourtSide.position, westCourtSide.position,
                                            southCourtSide.position, northCourtSide.position,
                                            southServiceWall, southEastServiceWall, southWestServiceWall,
                                            southMiddleServiceWall, northServiceWall, northEastServiceWall,
                                            northWestServiceWall, northMiddleServiceWall, 
                                            southServiceDelimiter.position, eastServiceDelimiter.position,
                                            westServiceDelimiter.position, northServiceDelimiter.position,
                                            player1, player2);
        Debug.Log("creating referee");
    }
    
}
