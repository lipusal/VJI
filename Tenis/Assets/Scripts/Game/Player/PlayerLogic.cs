using System;
using System.Collections;
using System.Collections.Generic;
using FrameLord;
using Game.Input;
using Game.Score;
using TMPro;
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
    private float _playerSpeed = 50f;
    private float deltaHitForce = 1;
    public float minHitForce = 18f;

    private ScoreManager _scoreManager;
    private Side _ballSide;
    private bool _isServing;
    
    // Is true after hit button is pressed
    private bool _isCharging;

    // Is true after hit button is released;
    private bool _finishHitting;

    // Moving Left or Right (-1: left, 1: right, 0: none)
    private int moveLeftRightValue;

    // Moving Up or Down (-1: down, 1: up, 0: none)
    private int moveForwardBackwardValue;
    
    private CharacterController _characterController;

    // Position of aim target
    private Vector3 _aimOffset;

    /* player id according to court side,
     * 1 if player is on team one or
     * 2 if player is on team two
    */
    private int _id;

    private GameObject _ball;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _isCharging = false;
        _finishHitting = false;
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
        _ballSide = Side.RIGHT;
        _currentHitForce = minHitForce;
        PlayerAnimation.InitializePlayerAnimator(GetComponent<Animator>());
        _aimOffset = aimTarget.position - transform.position;
        _scoreManager = ScoreManager.GetInstance();
        SetID();
        SetIsServing();
        SetInitialPosition();

    }

    public void SetInitialPosition()
    {
        Vector3 currentPosition = transform.position;
        float x, z; 
        Side servingSide = _scoreManager.GetServingSide();
        if (servingSide == Side.RIGHT)
        {
            z = -5f;
        }
        else
        {
            z = 5f;
        }

        if (_isServing)
        {
            //TODO activate mesh of serving walls
            ScoreManager.GetInstance().ActivateServingWalls(_id);
            x = -32f;
        }
        else
        {
            x = -27f;
        }

        _characterController.enabled = false;
        Vector3 newPosition = new Vector3(x, currentPosition.y, z);
        transform.position = newPosition;
        _characterController.enabled = true;
    }

    private void SetIsServing()
    {
        _isServing = _id == _scoreManager.GetServingTeam();
    }

    private void SetID()
    {
        if (transform.position.x < 0)
        {
            _id = 1;
        }
        else
        {
            _id = 2;
        }

    }

    void Update()
    {
        ReadInput();

        if (!_isCharging)
        {
//            PlayerAnimation.StopHittingAnimation();
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
            if (!_isCharging)
            {
                _currentHitForce = minHitForce;
                aimTarget.position = transform.position + _aimOffset;
            }

            _isCharging = true;
            _currentHitForce += _currentHitForce + deltaHitForce;
            _currentHitForce = Math.Min(_currentHitForce, maxHitForce);

        }

        if (ActionMapper.GetHitReleased(hitButton))
        {
            _isCharging = false;
            _finishHitting = true;
            PlayerAnimation.StartHittingAnimation(_ballSide);
        }
    }

    private void UpdatePosition()
    {
        float leftRightMove = movementSpeed * moveLeftRightValue * Time.deltaTime;
        float forwardBackardMove = movementSpeed * moveForwardBackwardValue * Time.deltaTime;
        AnimateMovement(leftRightMove, forwardBackardMove);
        //    _characterController.Move(new Vector3(forwardBackardMove, 0, -leftRightMove));
        var vec = new Vector3(forwardBackardMove, 0, -leftRightMove);
        _characterController.SimpleMove(vec * _playerSpeed);
       // AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_STEPS);
    }

    private void AnimateMovement(float leftRightMove, float forwardBackardMove)
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

    private void OnTriggerEnter(Collider other)
    {
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            DetectBallSide(other);
            
            if (_finishHitting)
            {
                _ball = other.gameObject;
            }
        }
    }

    private void DetectBallSide(Collider other)
    {
        Vector3 deltaPosition = other.gameObject.transform.position - transform.position;
        // Positive = left
        // Negative = right
        _ballSide = deltaPosition.z <= 0 ? Side.RIGHT : Side.LEFT;
       
    }

    private void HitBall()
    {
        if (_ball != null)
        {
            AudioManager.Instance.PlaySound(_ball.transform.position, (int) SoundId.SOUND_HIT);
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            _ball.GetComponent<Rigidbody>().velocity = aimDirection * _currentHitForce + new Vector3(0, 6.2f, 0);
            _currentHitForce = minHitForce;
            BallLogic.Instance.SetHittingPlayer(_id);
        }
    }

    private void Serve()
    {
        AudioManager.Instance.PlaySound(_ball.transform.position, (int) SoundId.SOUND_HIT); //TODO this when button to serve is pressed?
        Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
        float serveForce = 40f; //TODO use a private variable for serve force
        BallLogic ball = BallLogic.Instance;
        //set ball to height of service
        // activate Ball renderer as it will be turned off when state is serving
        _ball.GetComponent<Rigidbody>().velocity = aimDirection * serveForce + new Vector3(0, -3.2f, 0);
        BallLogic.Instance.SetHittingPlayer(_id);
    }
    private void DeleteBallReference()
    {
        _ball = null;
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
    public void SetServing(bool serving)
    {
        _isServing = serving;
    }
}
