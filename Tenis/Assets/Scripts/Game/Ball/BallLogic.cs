using System.Collections;
using System.Collections.Generic;
using FrameLord;
using UnityEngine;

public class BallLogic : MonoBehaviorSingleton<BallLogic>
{
    private Vector3 initialPosition;
    private int bounceQuantity;
    private Rigidbody _rigidbody;
    //-1 undefined, 0 lower than net, 1 greater than net
    private int side;

    private ScoreManager _scoreManager;
    // 1 for team one, 2 for team two
    private int _hittingPlayer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        bounceQuantity = 0;
        side = -1;
        _scoreManager = ScoreManager.GetInstance();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))// || collision.gameObject.CompareTag("Net"))
        {
            _scoreManager.manageBounce(transform.position, _hittingPlayer);

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPosition;
            _hittingPlayer = 0;
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            _scoreManager.manageBounce(transform.position, _hittingPlayer);
        }
    }

    private void manageBounce()
    {
        int currentSide = 1;
//        int currentSide = getBouncingSide();
        if (currentSide == side)
        {
            bounceQuantity++;
            if (bounceQuantity == 2)
            {
                transform.position = initialPosition;
                bounceQuantity = 0;
            }
        }
        else
        {
            bounceQuantity = 0;
        }
    }

    public void SetHittingPlayer(int playerId)
    {
        _hittingPlayer = playerId;
    }

//    private int getBouncingSide()
//    {
//        if()
//    }
}
