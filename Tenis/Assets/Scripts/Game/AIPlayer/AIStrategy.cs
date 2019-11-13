using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIStrategy
{
    private float _widthBase;
    private float _lengthBase;
    private float _width;
    private float _length;
    private float _height;
    private Transform _otherPlayer;

    public AIStrategy(Transform otherPlayer)
    {
        _widthBase = -10.118f;
        _lengthBase = -31.23f;
        _width = 10.5f - (-10.118f);
        _length = -3.87f - (-31.23f);
        _height = -3.032f;
        _otherPlayer = otherPlayer;
    }

    public Vector3 GenerateRandomPosition()
    {
        float width = Random.Range(0.0f, _width);
        float length = Random.Range(0.0f, _length);
        return new Vector3(_lengthBase + length, _height, _widthBase + width);
    }
    
    public Vector3 GenerateAwayFromPlayerPosition()
    {
        Vector3 playerPosition = _otherPlayer.position;
        float width1 = _widthBase;
        float width2 = _width + _widthBase;
        float width = Math.Abs(playerPosition.z - width1) >= Math.Abs(playerPosition.z - width2) ? width1 : width2;
        float length = Random.Range(0.0f, _length);
        return new Vector3(_lengthBase + length, _height, width);
    }

    public Vector3 mixedStrategy()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber > 0.6)
        {
            return GenerateAwayFromPlayerPosition();
        }
        else
        {
            return GenerateRandomPosition();
        }
    }
    
}
