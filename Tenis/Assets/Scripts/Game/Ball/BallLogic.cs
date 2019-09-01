using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic : MonoBehaviour
{
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Net"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPosition;
        }
    }
}
