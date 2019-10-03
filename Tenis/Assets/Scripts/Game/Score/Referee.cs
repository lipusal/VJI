using System;
using System.Collections;
using System.Collections.Generic;
using Game.Score;
using UnityEngine;

public class Referee
{
    // court delimiters
    private Vector3 _eastCourtSide;
    private Vector3 _westCourtSide;
    private Vector3 _southCourtSide;
    private Vector3 _northCourtSide;
    
    // service delimiters
    private Vector3 _southServiceWall;
    private Vector3 _southEastServiceWall;
    private Vector3 _southWestServiceWall;
    private Vector3 _southMiddleServiceWall;
    private Vector3 _northServiceWall;
    private Vector3 _northEastServiceWall;
    private Vector3 _northWestServiceWall;
    private Vector3 _northMiddleServiceWall;

    

    // 0 undefined, -1 south, 1 north
    private int _lastBoucedSide;
    private int _lastHitter;
    private int _previousToLastHitter;

    private PlayerLogic _player1;
    private AIPlayer _aiPlayer;

    private bool _isServing;
    
    


    public Referee(Vector3 eastCourtSide, Vector3 westCourtSide,
                    Vector3 southCourtSide, Vector3 northCourtSide,
                    Vector3 southServiceWall, Vector3 southEastServiceWall,
                    Vector3 southWestServiceWall, Vector3 southMiddleServiceWall,
                    Vector3 northServiceWall, Vector3 northEastServiceWall,
                    Vector3 northWestServiceWall,Vector3 northMiddleServiceWall,
                    PlayerLogic player1, AIPlayer aiPlayer)
    {
        _eastCourtSide = eastCourtSide;
        _westCourtSide = westCourtSide;
        _southCourtSide = southCourtSide;
        _northCourtSide = northCourtSide;
        _lastBoucedSide = 0;
        _lastHitter = 0;
        _previousToLastHitter = 0;
        _isServing = true;

        _southServiceWall = southServiceWall;
        _southEastServiceWall = southEastServiceWall;
        _southWestServiceWall = southWestServiceWall;
        _southMiddleServiceWall = southMiddleServiceWall;
        _northServiceWall = northServiceWall;
        _northEastServiceWall = northEastServiceWall;
        _northWestServiceWall = northWestServiceWall;
        _northMiddleServiceWall = northMiddleServiceWall;

        _player1 = player1;
        _aiPlayer = aiPlayer;
    }

    // Returns -1 if is point for opponent, 1 if is point for hitting team or zero if it is not point
    public int isPoint(Vector3 bouncePosition, int hitter)
    {
        int returnValue = 0;
        if (hitter != 0)
        {
            int currentSide = GetBouncingSide(bouncePosition);
            int hittingSide = GetHittingSide(hitter);
            if (currentSide == _lastBoucedSide && _lastBoucedSide != 0 && hitter == _previousToLastHitter)
            {

                if (hittingSide == currentSide)
                {
                   // Debug.Log("bounce on same side as hitter");
                    returnValue = -1;
                }
                else
                {
                    //Debug.Log("bounced two times");
                    returnValue = 1;
                }
            }
            else if (IsOut(bouncePosition))
            {
//                Debug.Log("opponent point");
                returnValue = -1;
            }
        
            if (returnValue == 0)
            {
                _lastBoucedSide = currentSide;
            }
            else
            {
                _lastBoucedSide = 0;
            }
        }

        return returnValue;
    }

    public void UpdateLastHitter(int hitter)
    {
        Debug.Log(hitter);
        _previousToLastHitter = _lastHitter;
        _lastHitter = hitter;
    }

    private int GetBouncingSide(Vector3 bouncePosition)
    {
        return bouncePosition.x < 0 ? -1 : 1;
    }

    private int GetHittingSide(int hitter)
    {
        if (hitter == 1)
        {
            return -1;
        }
        
        if (hitter == 2)
        {
            return 1;
        }
        
        //TODO exception invalid hitter
        return 2;
    }

    private bool IsOut(Vector3 bouncePosition)
    {
        if (_isServing)
        {
            return CheckServiceLimits(bouncePosition);
        }

        if (bouncePosition.x < _southCourtSide.x || bouncePosition.x > _northCourtSide.x)
        {
//            Debug.Log("bounce long out");

            return true;
        }

        if (bouncePosition.z < _westCourtSide.z || bouncePosition.z > _eastCourtSide.z)
        {
//            Debug.Log("bounce wide out");

            return true;
        }

        return false;
    }

    private bool CheckServiceLimits(Vector3 bouncePosition)
    {
        Side servingSide = ScoreManager.GetInstance().GetServingSide();
        int servingTeam = ScoreManager.GetInstance().GetServingTeam();
        Side expectedSide = servingSide == Side.RIGHT ? Side.LEFT : Side.RIGHT;
        return IsInsideServiceBox(bouncePosition, servingTeam, expectedSide);
    }

    // returns true if ball bounce inside expected service box and false if ball bounce out
    private bool IsInsideServiceBox(Vector3 bouncePosition, int servingTeam, Side expectedSide)
    {
        bool isInside;
//        // servingTeam 1 is south, servingTeam 2 is north
//        if (servingTeam == 1 && expectedSide == Side.LEFT)
//        {
//            isInside = IsInsideNorthWestServiceBox(bouncePosition);
//        }
//
//        else if (servingTeam == 1 && expectedSide == Side.RIGHT)
//        {
//            isInside = IsInsideNorthEastServiceBox(bouncePosition);
//        }
//        
//        else if (servingTeam == 2 && expectedSide == Side.LEFT)
//        {
//            isInside = IsInsideSouthWestServiceBox(bouncePosition);
//        }
//
//        else if (servingTeam == 2 && expectedSide == Side.RIGHT)
//        {
//            isInside = IsInsideSouthEastServiceBox(bouncePosition);
//        }
        //TODO implement

        return true;
//        return isInside;
    }


    public void SetServing(bool serving)
    {
        _isServing = serving;
        if (ScoreManager.GetInstance().GetServingTeam() == 1)
        {
            _player1.SetServing(serving);
        }
        else
        {
            _aiPlayer.SetServing(serving);
        }
        //TODO set player to serving or not
    }

    public bool GetIsServing()
    {
        return _isServing;
    }
}
