using System;
using System.Collections;
using System.Collections.Generic;
using Game.Score;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIStrategy
{
    private float _widthBase;
    private float _lengthBase;
    private float _width;
    private float _length;
    private float _height;
    private float _widthMiddle;
    private float _baseServeLength;
    private float _maxServeLength;
    private float _rightWidth;
    private float _leftWidth;
    private Transform _otherPlayer;

    public AIStrategy(Transform otherPlayer)
    {
        _widthBase = -10.118f;
        _lengthBase = -31.23f;
        _width = 10.5f - (-10.118f);
        _length = -3.87f - (-31.23f);
        _height = -3.032f;
        _otherPlayer = otherPlayer;
        _widthMiddle = 0;
        _rightWidth = -11.0f;
        _leftWidth = 10.8f;
        _baseServeLength = -5.0f;
        _maxServeLength = -16.8f;
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

    public Vector3 MixedStrategy(float randomPercentage)
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber > randomPercentage)
        {
            return GenerateAwayFromPlayerPosition();
        }
        else
        {
            return GenerateRandomPosition();
        }
    }

    public Vector3 GetServeTarget(Side servingSide)
    {
        Vector3 target = Vector3.zero;
        float x, z;
        x = Random.Range(_baseServeLength, _maxServeLength);
        if (servingSide == Side.RIGHT)
        {
            z = Random.Range(_rightWidth, _widthMiddle);
        }
        else
        {
            z =Random.Range(_widthMiddle, _leftWidth);
        }
        target = new Vector3(x, _height, z);
        return target;
    }

    public Vector3 GeneratePositionBasedOnDifficulty(int difficulty)
    {
        Vector3 resultPosition;
        if (difficulty == 4)
        {
            resultPosition = MixedStrategy(0.4f);
        }
        else if (difficulty == 3)
        {
            resultPosition = MixedStrategy(0.6f);
        }
        else
        {
            resultPosition = GenerateRandomPosition();
        }

        return resultPosition;
    }
}
