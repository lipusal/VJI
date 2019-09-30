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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        bounceQuantity = 0;
        side = -1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))// || collision.gameObject.CompareTag("Net"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPosition;
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            manageBounce();
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

//    private int getBouncingSide()
//    {
//        if()
//    }
}
