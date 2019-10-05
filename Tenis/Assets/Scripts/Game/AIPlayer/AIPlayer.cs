using System.Collections;
using System.Collections.Generic;
using FrameLord;
using Game.Player;
using Game.Score;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public Transform ballPosition;

    private float speed = 15;

    private float hitForce = 28;

    public Transform aimTarget;

    private CharacterController _characterController;

    private PlayerAnimation _playerAnimation;

    private bool _isServing;
    
    /* player id according to court side,
    * 1 if player is on team one or
    * 2 if player is on team two
   */
    private int _id;
    
    void Start()
    {
        _isServing = false;
        _characterController = GetComponent<CharacterController>();
        _playerAnimation =  new PlayerAnimation(GetComponent<Animator>());
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
        if (BallLogic.Instance.IsEnabled())
        {
            MoveToBall();
        }
    }

    private void MoveToBall()
    {
        Vector3 movingDirection = new Vector3(transform.position.x, transform.position.y, ballPosition.position.z) - transform.position;
        _characterController.Move(movingDirection * speed * Time.deltaTime);
        _playerAnimation.StartMoveAnimation(GetMovementDirection(movingDirection));
//        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_STEPS);
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
        AudioManager.Instance.PlaySound(ball.transform.position, (int) SoundId.SOUND_HIT);
        Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
        ball.GetComponent<Rigidbody>().velocity = aimDirection * hitForce + new Vector3(0, 3.2f, 0);
        ball.SetHittingPlayer(_id);
    }
    
    private void PlayServeSound()
    {
        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_SERVE);
    }
    
    private void DeleteBallReference()
    {
        //TODO its here just to use same animation as player 1
    }
}
