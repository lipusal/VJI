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
    private float _serveForce = 40f;
    private float _currentHitForce;
    private float _playerSpeed = 50f;
    private float deltaHitForce = 1;
    public float minHitForce = 18f;

    public float _minimumOffSetToHitBall = -3.0f;

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

    private PlayerAnimation _playerAnimation;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _isCharging = false;
        _finishHitting = false;
        moveLeftRightValue = 0;
        moveForwardBackwardValue = 0;
        _ballSide = Side.RIGHT;
        _currentHitForce = minHitForce;
        _playerAnimation =  new PlayerAnimation(GetComponent<Animator>());
        _aimOffset = aimTarget.position - transform.position;
        _scoreManager = ScoreManager.GetInstance();
        SetID();
        SetIsServing();
        SetInitialPosition();
        if (_isServing)
        {
            ScoreManager.GetInstance().ActivateServingWalls(_id);
        }
        BallLogic.Instance.DesapearBall();

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
        if (!_playerAnimation.IsPlayingHitAnimation())
        {
            ReadInput();

            if (!_isCharging)
            {
                UpdatePosition();
            }
            else
            {
                UpdateAimTargetPosition();
            }
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
            if (_isServing)
            {
                _playerAnimation.StartServeAnimation();
            }
            else
            {
                //DetectBallSide();
                _playerAnimation.StartHittingAnimation(_ballSide);
            }
        }
    }

    private void UpdatePosition()
    {
        float leftRightMove = movementSpeed * moveLeftRightValue * Time.deltaTime;
        float forwardBackardMove = movementSpeed * moveForwardBackwardValue * Time.deltaTime;
        _playerAnimation.AnimateMovement(leftRightMove, forwardBackardMove);
        var vec = new Vector3(forwardBackardMove, 0, -leftRightMove);
        _characterController.SimpleMove(vec * _playerSpeed);
        //TODO steps sound as animation event
       // AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_STEPS); 
    }

   

    private void UpdateAimTargetPosition()
    {
        aimTarget.Translate(new Vector3(aimTargetSpeed * moveForwardBackwardValue * Time.deltaTime, 0,
            -aimTargetSpeed * moveLeftRightValue * Time.deltaTime));
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
        Debug.Log(deltaPosition.z <= 0 ? "RIGHT" : "LEFT");
        _ballSide = deltaPosition.z <= 0 ? Side.RIGHT : Side.LEFT;
    }
    
//    private void DetectBallSide()
//    {
//        Vector3 bsllPosition = BallLogic.Instance.transform.position;
//        Vector3 deltaPosition = bsllPosition - transform.position;
//        // Positive = left
//        // Negative = right
//        _ballSide = deltaPosition.z <= 0 ? Side.RIGHT : Side.LEFT;
//    }

    private bool IsValidBallPositionToHit(GameObject ballToHit)
    {
        return (ballToHit.transform.position - transform.position).x >= _minimumOffSetToHitBall;
    }

    private void HitBall()
    {
        if (_ball != null && IsValidBallPositionToHit(_ball))
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
        Vector3 currentPosition = transform.position;
        Vector3 aimDirection = (aimTarget.position - currentPosition).normalized;
        //float serveForce = 40f; //TODO use a private variable for serve force
        BallLogic ball = BallLogic.Instance;
        ball.AppearBall(new Vector3(currentPosition.x + 0.1f, 4.05f, currentPosition.z), Vector3.zero );
        ball.GetComponent<Rigidbody>().velocity = aimDirection * _serveForce + new Vector3(0, -1.2f, 0);
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

    public int GetId()
    {
        return _id;
    }

    private void PlayServeSound()
    {
        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_SERVE);
    }
}
