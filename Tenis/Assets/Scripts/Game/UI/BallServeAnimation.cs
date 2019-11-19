using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallServeAnimation : MonoBehaviour
{
    public AnimationCurve servingAnimationCurve;
    public float verticalAppearOffset;
    public float serveAnimationDelay;
    public float animationSpeed;

    private float _animationCurveTimeElapsed;
    private bool _animationCurveStarted;
    private Vector3 _animationStartPosition;
    private float _animationDelayTimeElapsed;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
       // GetComponent<Rigidbody>().useGravity = false;
    }

    private void Update()
    {
        if(!_animationCurveStarted && _animationDelayTimeElapsed < serveAnimationDelay)
        {
            _animationDelayTimeElapsed += Time.deltaTime;
        }

        if(_animationDelayTimeElapsed >= serveAnimationDelay)
        {
            PlayServingAnimationCurve();
        }
    }

    private void FixedUpdate()
    {
        if (_animationCurveStarted)
        {
            _animationCurveTimeElapsed += Time.deltaTime;
            transform.position += Vector3.up * servingAnimationCurve.Evaluate(_animationCurveTimeElapsed) * Time.deltaTime * animationSpeed;
            /*
            GetComponent<Rigidbody>().MovePosition(
                new Vector3(_animationStartPosition.x,
                _animationStartPosition.y + servingAnimationCurve.Evaluate(_animationCurveTimeElapsed),
                _animationStartPosition.z));*/
        }
    }

    private void PlayServingAnimationCurve()
    {
        GetComponent<MeshRenderer>().enabled = true;
        //GetComponent<Rigidbody>().useGravity = true;
        _animationCurveStarted = true;
        _animationCurveTimeElapsed = 0.0f;
        _animationStartPosition = transform.position;
        _animationDelayTimeElapsed = 0.0f;
    }
}
