using System.Collections;
using System.Collections.Generic;
using FrameLord;
using UnityEngine;

public class BallLogic : MonoBehaviorSingleton<BallLogic>
{
    private Vector3 initialPosition;
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
        side = -1;
        _scoreManager = ScoreManager.GetInstance();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            _scoreManager.manageBounce(transform.position, _hittingPlayer);

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPosition;
            _hittingPlayer = 0;
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_BOUNCE);
            _scoreManager.manageBounce(transform.position, _hittingPlayer);
        }
    }

    public void SetHittingPlayer(int playerId)
    {
        _hittingPlayer = playerId;
    }

    public void ResetConfig()
    {
        _hittingPlayer = 0;
        transform.position = initialPosition;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
