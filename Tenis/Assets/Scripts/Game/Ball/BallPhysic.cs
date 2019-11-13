using System;
using Game.Score;
using UnityEngine;

namespace Game.Ball
{
    public class BallPhysic
    {
        private float _gravity;
        private float _floorHeight;

        public BallPhysic()
        {
            _gravity = Physics.gravity.y;
            _floorHeight = -3.022f;
        }

//    void Start()
//    {
//        _gravity = Physics.gravity.y;
//        GetComponent<Rigidbody>().velocity = GetVelocity(target.position, 1.5f);
//    }



        public Vector3 GetVelocity(Vector3 currentPosition, Vector3 targetPosition, float timeToBounce)
        {
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

        public Side GetZPositionAtFutureX(float futureX, float playerZ, Vector3 position, Vector3 ballVelocity)
        {
            if (IsBallDirecting(futureX, position.x, ballVelocity.x))
            {
                float time = (futureX - position.x) / ballVelocity.x;
                float zf = position.z + ballVelocity.z * time;
                return zf <= playerZ ? Side.RIGHT : Side.LEFT;
            }

            return Side.RIGHT; //To make drive default hit

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

        public Vector3 GetNextBouncingPosition(Vector3 ballPosition, Vector3 ballVelocity)
        {
            float timeToBounce = CalculateBouncingTime(ballPosition, ballVelocity);
            float xf = ballPosition.x + ballVelocity.x * timeToBounce;
            float zf = ballPosition.z + ballVelocity.z * timeToBounce;
            return new Vector3(xf, _floorHeight, zf);
        }

        private float CalculateBouncingTime(Vector3 ballPosition, Vector3 ballVelocity)
        {
            float a = _gravity / 2;
            float b = ballVelocity.y;
            float c = ballPosition.y - _floorHeight;
            float discriminant = (float) Math.Sqrt(b * b - 4 * a * c);
            float timeToBounce1 = (-b + discriminant) / (2 * a);
            float timeToBounce2 = (-b - discriminant) / (2 * a);

            if (timeToBounce1 < 0)
            {
                return timeToBounce2;
            }

            if (timeToBounce2 < 0)
            {
                return timeToBounce1;
            }

            return timeToBounce1 < timeToBounce2 ? timeToBounce1 : timeToBounce2;
        }
    }
}
