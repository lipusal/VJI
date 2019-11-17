using System;
using System.Collections;
using System.Collections.Generic;
using FrameLord;
using Game.Input;
using Game.Score;
using TMPro;
using UnityEngine;

public class Player2Logic : PlayerLogic
{
    public override void SetInitialPosition()
    {
        Vector3 currentPosition = transform.position;
        _isCharging = false;
        float x, z; 
        Side servingSide = _scoreManager.GetServingSide();
        if (servingSide == Side.RIGHT)
        {
            z = 6.57f;
        }
        else
        {
            z = -6.24f;
        }

        if (_isServing)
        {
            ScoreManager.GetInstance().ActivateServingWalls(_id);
            x = 32f;
        }
        else
        {
            x = 26.0f;
        }

        _characterController.enabled = false;
        Vector3 newPosition = new Vector3(x, currentPosition.y, z);
        transform.position = newPosition;
        _characterController.enabled = true;
    }
}
