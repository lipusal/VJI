using System;
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

    public Transform aimTarget;
    public float aimTargetSpeed;
    
    public float movementSpeed = 14f;
    public float hitForce = 40f;

    private bool _isHitting;
    // Moving Left or Right (-1: left, 1: right, 0: none)
    private int moveLeftRightValue;
    // Moving Up or Down (-1: down, 1: up, 0: none)
    private int moveForwardBackwardValue;
    private CharacterController _characterController;

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

        if(!_isHitting)
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
            _isHitting = true;
        }

        if (ActionMapper.GetHitReleased(hitButton))
        {
            _isHitting = false;
        }
    }
    
    private void UpdatePosition()
    {
        float leftRightMove = movementSpeed * moveLeftRightValue * Time.deltaTime;
        float forwardBackardMove = movementSpeed * moveForwardBackwardValue * Time.deltaTime;

        _characterController.Move(new Vector3(leftRightMove, 0, forwardBackardMove));
    }

    private void UpdateAimTargetPosition()
    {
        aimTarget.Translate(new Vector3(aimTargetSpeed * moveLeftRightValue * Time.deltaTime, 0, aimTargetSpeed * moveForwardBackwardValue * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("applying force to ball");
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            
            other.GetComponent<Rigidbody>().velocity = aimDirection * hitForce + new Vector3(0, 6.2f, 0);
        }
    }
}
