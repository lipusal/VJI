using System.Collections;
using System.Collections.Generic;
using Game.Input;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Button to move left
    public KeyCode leftButton = KeyCode.LeftArrow;
    // Button to move right
    public KeyCode rightButton = KeyCode.RightArrow;
    // Button to move forward
    public KeyCode forwardButton = KeyCode.UpArrow;
    // Button to move backward
    public KeyCode bacwardButton = KeyCode.DownArrow;
    // Button to hit
    public KeyCode hitButton = KeyCode.A;
    private bool _isHitting;
    // Moving Left or Right (-1: left, 1: right, 0: none)
    private int moveLeftRightValue;
    // Moving Up or Down (-1: down, 1: up, 0: none)
    private int moveForwardBackwardValue;
    private CharacterController _characterController;
    private float speed = 4f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _isHitting = false;
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
    }

    void Update()
    {
        ReadInput();
        UpdatePosition();
    }
    
    private void ReadInput()
    {
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
        if (ActionMapper.GetMoveLeft(leftButton))
        {
            moveLeftRightValue += -1;
        }
        
        if (ActionMapper.GetMoveRight(rightButton))
        {
            moveLeftRightValue += 1;
        }

        if (ActionMapper.GetMoveForward(forwardButton))
        {
            moveForwardBackwardValue += 1;
        }
        
        if (ActionMapper.GetMoveBackward(bacwardButton))
        {
            moveForwardBackwardValue += -1;
        }

        if (ActionMapper.GetHit(hitButton))
        {
            _isHitting = true;
        }
    }
    
    private void UpdatePosition()
    {
        float leftRightMove = speed * moveLeftRightValue * Time.deltaTime;
        float forwardBackardMove = speed * moveForwardBackwardValue * Time.deltaTime;
        _characterController.Move(new Vector3(leftRightMove, 0, forwardBackardMove));
    }

}
