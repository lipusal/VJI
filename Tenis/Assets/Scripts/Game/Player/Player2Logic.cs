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
        ResetToIdle();
        float x, z;
        Side servingSide = _scoreManager.GetServingSide();
        if (IsTwoPlayers())
        {
            DisableTarget();
        }
        
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
            SetAimPosition(servingSide);
            ScoreManager.GetInstance().ActivateServingWalls(_id);
            x = 32f;
        }
        else
        {
            ScoreManager.GetInstance().DeactivateServingWalls(_id);
            x = 26.0f;
        }

        _characterController.enabled = false;
        Vector3 newPosition = new Vector3(x, currentPosition.y, z);
        transform.position = newPosition;
        _characterController.enabled = true;
    }

    public override void SetAimPosition(Side servingSide)
    {
        float x = -12.5f, z;
        if (servingSide == Side.RIGHT)
        {
            z = -5.6f;
        }
        else
        {
            z = 5.6f;
        }
        aimTarget.position = new Vector3(x, aimTarget.position.y, z);
    }
}