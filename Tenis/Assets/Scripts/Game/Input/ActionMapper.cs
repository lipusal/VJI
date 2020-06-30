using UnityEngine;

namespace Game.Input
{
    public class ActionMapper
    {
        public static bool GetMoveLeft(KeyCode leftButton, string horizontalAxis)
        {
            return IsButtonDown(leftButton) || UnityEngine.Input.GetAxis(horizontalAxis) < 0;
        }

        public static bool GetMoveRight(KeyCode rightButton, string horizontalAxis)
        {
            return IsButtonDown(rightButton) || UnityEngine.Input.GetAxis(horizontalAxis) > 0;
        }
        
        public static bool GetMoveForward(KeyCode forwardButton, string verticalAxis)
        {
            return IsButtonDown(forwardButton) || UnityEngine.Input.GetAxis(verticalAxis) > 0;
        }

        public static bool GetMoveBackward(KeyCode backwardButton, string verticalAxis)
        {
            return IsButtonDown(backwardButton) || UnityEngine.Input.GetAxis(verticalAxis) < 0;
        }

        public static bool GetHitPressed(KeyCode hitButton, KeyCode hitJoystickButton)
        {
            return IsButtonDown(hitButton, hitJoystickButton);
        }

        public static bool GetHitReleased(KeyCode hitButton, KeyCode hitJoystickButton)
        {
            return UnityEngine.Input.GetKeyUp(hitButton) || UnityEngine.Input.GetKeyUp(hitJoystickButton);
        }
        
        public static bool IsButtonDown(params KeyCode[] buttons)
        {
            foreach (var button in buttons)
            {
                if (UnityEngine.Input.GetKey(button))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Like IsButtonDown, but only returns true in the first frame the button is pressed. Useful when we don't want
        /// to query every single frame, eg. menu open (the menu is opened on key press, but is not toggled if the user
        /// holds the key down for more than one frame, which happens most of the time)
        /// </summary>
        /// <param name="buttons">Buttons to check.</param>
        /// <returns></returns>
        public static bool IsButtonPressed(params KeyCode[] buttons)
        {
            foreach (var button in buttons)
            {
                if (UnityEngine.Input.GetKeyDown(button))
                {
                    return true;
                }
            }

            return false;
        }
    }
}