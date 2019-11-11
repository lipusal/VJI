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

    public AIStrategy()
    {
        _widthBase = -10.118f;
        _lengthBase = -31.23f;
        _width = 12.132f - (-10.118f);
        _length = -3.87f - (-31.23f);
        _height = -3.032f;
    }

    public Vector3 GenerateRandomPosition()
    {
        float width = Random.Range(0.0f, _width);
        float length = Random.Range(0.0f, _length);
        return new Vector3(_lengthBase + length, _height, _widthBase + width);
    }
    
}
