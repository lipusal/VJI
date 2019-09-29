using System;
using System.Collections;
using System.Collections.Generic;
using Game.Input;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private const int IDLE = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int LEFT = 3;
    private const int DOWN = 4; //TODO all this should be an enum
    
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

    public Transform aimTarget;
    public float aimTargetSpeed = 18;

    public float movementSpeed = 14f;
    public float maxHitForce = 40f;
    private float _currentHitForce;
    private float deltaHitForce = 1;
    public float minHitForce = 18f;

    // Is true after hit button is pressed
    private bool _isHitting;

    // Is true after hit button is released;
    private bool _finishHitting;

    // Moving Left or Right (-1: left, 1: right, 0: none)
    private int moveLeftRightValue;

    // Moving Up or Down (-1: down, 1: up, 0: none)
    private int moveForwardBackwardValue;
    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _isHitting = false;
        _finishHitting = false;
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
        _currentHitForce = minHitForce;
        PlayerAnimation.InitializePlayerAnimator(GetComponent<Animator>());
    }

    void Update()
    {
        ReadInput();

        if (!_isHitting)
        {
            UpdatePosition();
        }
        else
        {
            UpdateAimTargetPosition();
        }
    }

    private void ReadInput()
    {
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
        _finishHitting = false;

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

        if (ActionMapper.GetHitPressed(hitButton))
        {
            if (!_isHitting)
            {
                _currentHitForce = minHitForce;
            }

            _isHitting = true;
            _currentHitForce += _currentHitForce + deltaHitForce;
            _currentHitForce = Math.Min(_currentHitForce, maxHitForce);
        }

        if (ActionMapper.GetHitReleased(hitButton))
        {
            _isHitting = false;
            _finishHitting = true;
        }
    }

    private void UpdatePosition()
    {
        float leftRightMove = movementSpeed * moveLeftRightValue * Time.deltaTime;
        float forwardBackardMove = movementSpeed * moveForwardBackwardValue * Time.deltaTime;
        AnimteMovement(leftRightMove, forwardBackardMove);
        _characterController.Move(new Vector3(forwardBackardMove, 0, -leftRightMove));
    }

    private void AnimteMovement(float leftRightMove, float forwardBackardMove)
    {
        if (leftRightMove > 0)
        {
            PlayerAnimation.StartMoveAnimation(RIGHT);
        }
        else if(leftRightMove < 0)
        {
            PlayerAnimation.StartMoveAnimation(LEFT);
        }
        else if(forwardBackardMove > 0)
        {
            PlayerAnimation.StartMoveAnimation(UP);
        }
        else if(forwardBackardMove < 0)
        {
            PlayerAnimation.StartMoveAnimation(DOWN);
        }
        else
        {
            PlayerAnimation.StartMoveAnimation(IDLE);
        }
        //TODO consider diagonal movement
        
    }

    private void UpdateAimTargetPosition()
    {
        aimTarget.Translate(new Vector3(aimTargetSpeed * moveForwardBackwardValue * Time.deltaTime, 0,
            -aimTargetSpeed * moveLeftRightValue * Time.deltaTime));
    }


    private void OnTriggerStay(Collider other)
    {
        if (_finishHitting && other.CompareTag("Ball"))
        {
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            Debug.Log(aimDirection.x.ToString());
            other.GetComponent<Rigidbody>().velocity = aimDirection * _currentHitForce + new Vector3(0, 6.2f, 0);
            _currentHitForce = minHitForce;
        }
    }


//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Ball"))
//        {
//            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
//
//            other.GetComponent<Rigidbody>().velocity = aimDirection * _currentHitForce + new Vector3(0, 6.2f, 0);
//            _currentHitForce = minHitForce;
//
//        }
//    }
}
