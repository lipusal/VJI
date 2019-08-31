using UnityEngine;

namespace Game.Input
{
    public class ActionMapper
    {
        public static bool GetMoveLeft(KeyCode leftButton)
        {
            return UnityEngine.Input.GetKey(leftButton);
        }

        public static bool GetMoveRight(KeyCode rightButton)
        {
            return UnityEngine.Input.GetKey(rightButton);
        }
        
        public static bool GetMoveForward(KeyCode forwardButton)
        {
            return UnityEngine.Input.GetKey(forwardButton);
        }

        public static bool GetMoveBackward(KeyCode backwardButton)
        {
            return UnityEngine.Input.GetKey(backwardButton);
        }

        public static bool GetHit(KeyCode hitButton)
        {
            return UnityEngine.Input.GetKeyDown(hitButton);
        }
    }
}
