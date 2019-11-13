using System;
using System.Collections;
using System.Collections.Generic;
using FrameLord;
using Game.Player;
using Game.Score;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public Transform ballPosition;

    private float speed = 10;

    private float hitForce = 28;

    public Transform aimTarget;

    public Transform otherPlayer;

    private AIStrategy _AIStrategy;
    
    private CharacterController _characterController;

    private PlayerAnimation _playerAnimation;
    private Vector3 _basePositionFromBall;
    private Vector3 _desiredPosition;
    private ScoreManager _scoreManager;
    private bool _newPosition;

    private bool _isServing;
    
    /* player id according to court side,
    * 1 if player is on team one or
    * 2 if player is on team two
   */
    private int _id;
    
    void Start()
    {
        _isServing = false;
        _AIStrategy = new AIStrategy(otherPlayer);
        _characterController = GetComponent<CharacterController>();
        _playerAnimation =  new PlayerAnimation(GetComponent<Animator>());
        _basePositionFromBall = new Vector3(7.705f,0f,0.633f);
        _newPosition = true;
        _scoreManager = ScoreManager.GetInstance();
        
        if (transform.position.x < 0)
        {
            _id = 1;
        }
        else
        {
            _id = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BallLogic ballLogic = BallLogic.Instance;
        bool hasMoved = false;
        if (ballLogic.IsEnabled() && ballLogic.GetHittingPlayer() != _id
            && ballLogic.GetHittingPlayer() != 0)
        {
            hasMoved = MoveToBall();
        }
        
        if(!hasMoved) 
        {
            _playerAnimation.StartMoveAnimation(MovementDirection.IDLE);
        }
    }

    private bool MoveToBall()
    {
        if (_newPosition)
        {
            _desiredPosition = BallLogic.Instance.GetBouncePosition();
            _desiredPosition = _desiredPosition + _basePositionFromBall;
            _newPosition = false;
        }

        if (_desiredPosition.x < 0)
        {
            return false ;
        }
        
        if (Math.Abs(transform.position.x - _desiredPosition.x) > 0.05 ||
            Math.Abs(transform.position.z - _desiredPosition.z) > 0.05)
        {
            float xDirection = _desiredPosition.x - transform.position.x;
            float zDirection = _desiredPosition.z - transform.position.z;
            Vector3 movingDirection = new Vector3(xDirection, transform.position.y, zDirection).normalized;
            _characterController.Move(Time.deltaTime * speed * movingDirection);
            _playerAnimation.StartMoveAnimation(GetMovementDirection(movingDirection));
            return true;
//        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_STEPS);     
        }

        return false;
    }

    private MovementDirection GetMovementDirection(Vector3 movingDirection)
    {
        //prioritize LEFT and RIGHT over UP and DOWN because it looks better on animation
        if (ballPosition.position.z - transform.position.z < 0)
        {
            return MovementDirection.LEFT;
        }
        else if (ballPosition.position.z - transform.position.z > 0)
        {
            return MovementDirection.RIGHT;
        }
        else if ((ballPosition.position.x - transform.position.x < 0))
        {
            return MovementDirection.UP;
        }
        else if (ballPosition.position.z - transform.position.z > 0)
        {
            return MovementDirection.DOWN;
        }
        else
        {
            return MovementDirection.IDLE;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            _playerAnimation.StartHittingAnimation(Side.RIGHT);
        }
    }

    public void SetServing(bool serving)
    {
        _isServing = serving;
    }
    
    private void HitBall()
    {
        BallLogic ball = BallLogic.Instance; 
        Vector3 aimPosition = _AIStrategy.GenerateRandomPosition();
//        Vector3 aimPosition = _AIStrategy.GenerateAwayFromPlayerPosition();
        AudioManager.Instance.PlaySound(ball.transform.position, (int) SoundId.SOUND_HIT);
        Vector3 velocity = BallLogic.Instance.GetVelocity(aimPosition, 1.8f);//change time in function of currentHitForce
        ball.GetComponent<Rigidbody>().velocity = velocity;
        ball.SetHittingPlayer(_id);
        _newPosition = true;
    }
    
    private void PlayServeSound()
    {
        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_SERVE);
    }
    
    private void DeleteBallReference()
    {
        //TODO its here just to use same animation as player 1
    }

    public void Setinitialposition()
    {
        _newPosition = true;
        Vector3 currentPosition = transform.position;

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
