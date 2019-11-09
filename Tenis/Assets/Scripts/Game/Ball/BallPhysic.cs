using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysic : MonoBehaviour
{
    // Start is called before the first frame update
    private float _gravity;
    public Transform target;
    void Start()
    {
        _gravity = Physics.gravity.y;
        GetComponent<Rigidbody>().velocity = GetVelocity(target.position, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetVelocity(Vector3 targetPosition, float timeToBounce)
    {
        Vector3 currentPosition = transform.position;
        
        // current position
        float xi = currentPosition.x;
        float yi = currentPosition.y;
        float zi = currentPosition.z;

        // target position
        float xf = targetPosition.x;
        float yf = targetPosition.y;
        float zf = targetPosition.z;
        
        float vyi = (yf - yi) / timeToBounce - 0.5f * _gravity * timeToBounce;
        float vxi = (xf - xi) / timeToBounce;
        float vzi = (zf - zi) / timeToBounce;
        return new Vector3(vxi, vyi, vzi);
    }

    public float GetZPositionAtFutureX(float x)
    {
        Rigidbody ballRigidbody = BallLogic.Instance.GetComponent<Rigidbody>();
        Vector3 ballVelocity = ballRigidbody.velocity;
        Vector3 position = ballRigidbody.position;
        if (IsBallDirecting(x, position.x, ballVelocity.x))
        {
            float time = (x - position.x) / ballVelocity.x;
            float zf = position.z + ballVelocity.z * time;
            return zf;
        }
        else
        {
            return 4; //TODO check the reason is to do by default drive
        }
    }

    private bool IsBallDirecting(float targetPos, float currentPos, float velocity)
    {
        if ((currentPos <= targetPos && velocity > 0) ||
            currentPos >= targetPos && velocity < 0) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
