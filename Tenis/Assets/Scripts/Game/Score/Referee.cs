using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee
{
    private Vector3 _eastCourtSide;
    private Vector3 _westCourtSide;
    private Vector3 _southCourtSide;
    private Vector3 _northCourtSide;

    // 0 undefined, -1 south, 1 north
    private int _lastBoucedSide;

    public Referee(Vector3 eastCourtSide, Vector3 westCourtSide,
                    Vector3 southCourtSide, Vector3 northCourtSide)
    {
        _eastCourtSide = eastCourtSide;
        _westCourtSide = westCourtSide;
        _southCourtSide = southCourtSide;
        _northCourtSide = northCourtSide;
        _lastBoucedSide = 0;
    }

    // Returns -1 if is point for oponent, 1 if is point for hiting team or zero if it is not point
    public int isPoint(Vector3 bouncePosition, int hitter)
    {
        int returnValue = 0;
        if (hitter != 0)
        {
            int currentSide = GetBouncingSide(bouncePosition);
            int hittingSide = GetHittingSide(hitter);
            if (currentSide == _lastBoucedSide && _lastBoucedSide != 0)
            {

                if (hittingSide == currentSide)
                {
                    Debug.Log("bounce on same side as hitter");

//                    Debug.Log("opponent player point");
                    returnValue = -1;
                }
                else
                {
                    Debug.Log("bounced two times");

//                    Debug.Log("hitting player point");
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

        if (bouncePosition.x < _southCourtSide.x || bouncePosition.x > _northCourtSide.x)
        {
            Debug.Log("bounce long out");

            return true;
        }

        if (bouncePosition.z < _westCourtSide.z || bouncePosition.z > _eastCourtSide.z)
        {
            Debug.Log("bounce wide out");

            return true;
        }

        return false;
    }
}
