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
}
