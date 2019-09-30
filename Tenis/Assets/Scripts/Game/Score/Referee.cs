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
        if (hitter != 0)
        {
            int currentSide = GetBouncingSide(bouncePosition);

            if (IsOut(bouncePosition, GetHittingSide(hitter), currentSide))
            {
                Debug.Log("oponent point");
                return -1;
            }

            if (currentSide == _lastBoucedSide)
            {
                Debug.Log("hitting player point");
                return 1;
            }
        }

        return 0;
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

    private bool IsOut(Vector3 bouncePosition, int hittingSide, int bouncingSide)
    {
        if (hittingSide == bouncingSide)
        {
            return true;
        }

        if (bouncePosition.x < _southCourtSide.x || bouncePosition.x > _northCourtSide.x)
        {
            return true;
        }

        if (bouncePosition.z < _westCourtSide.z || bouncePosition.z > _eastCourtSide.z)
        {
            return true;
        }

        return false;
    }
}
