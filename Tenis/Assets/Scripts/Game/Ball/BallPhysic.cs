using Game.Score;
using UnityEngine;

namespace Game.Ball
{
    public class BallPhysic
    {
        private float _gravity;
        public Transform target;

        public BallPhysic()
        {
            _gravity = Physics.gravity.y;
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
    }
}
