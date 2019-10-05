using System.Collections;
using System.Collections.Generic;
using FrameLord;
using UnityEngine;

public class BallLogic : MonoBehaviorSingleton<BallLogic>
{
    private Rigidbody _rigidbody;
    
    //-1 undefined, 0 lower than net, 1 greater than net
    private int side;

    private ScoreManager _scoreManager;
    // 1 for team one, 2 for team two
    private int _hittingPlayer;

    private bool _isEnabled;

    private void Start()
    {
        _isEnabled = true;
        _rigidbody = GetComponent<Rigidbody>();
        side = -1;
        _scoreManager = ScoreManager.GetInstance();
        ResetConfig();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            _scoreManager.manageBounce(transform.position, _hittingPlayer);
            AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_WALL);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            ResetConfig();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_BOUNCE);
            _scoreManager.manageBounce(transform.position, _hittingPlayer);
        }
        else if (collision.gameObject.CompareTag("Net"))
        {
           //TODO play net sound
        }
        else
        {
            Debug.Log("something else");
            //TODO add point to hitter
        }
    }

    public void SetHittingPlayer(int playerId)
    {
        _hittingPlayer = playerId;
        ScoreManager.GetInstance().UpdateLastHitter(playerId);

        if (!ScoreManager.GetInstance().CanPlayerHit(playerId))
        {
            int opponentId = (playerId % 2) + 1;
            ScoreManager.GetInstance().AddPoint(opponentId);
            //TODO check if match finished
        }
    }

    public void ResetConfig()
    {
        DesapearBall();
        _hittingPlayer = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
//        GetComponent<ParticleSystem>().Pause();
        ParticleSystem.EmissionModule emissionModule = GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = false;

    }

    public void DesapearBall()
    {
        _isEnabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void AppearBall(Vector3 position, Vector3 velocity)
    {
        _isEnabled = true;
        transform.position = position;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Rigidbody>().velocity = velocity;
//        GetComponent<ParticleSystem>().Play();
        ParticleSystem.EmissionModule emissionModule = GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = true;

    }

    public bool IsEnabled()
    {
        return _isEnabled;
    }
}
