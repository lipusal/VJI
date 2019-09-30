using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation
{
    private static Animator _animator;

    private static readonly int Direction = Animator.StringToHash("direction");
    private static readonly int Hitting = Animator.StringToHash("hitting");

    // Start is called before the first frame update
    

    public static void InitializePlayerAnimator(Animator animator)
    {
        _animator = animator;
    }
    
    public static void StartMoveAnimation(int direction)//TODO should receive enum of direction
    {
        //validate _animator not null?
        _animator.SetInteger(Direction, direction);
    }

    public static void StartHittingAnimation()
    {
        //validate _animator not null?
        _animator.SetBool(Hitting, true);
    }
    
    public static void StopHittingAnimation()
    {
        //validate _animator not null?
        _animator.SetBool(Hitting, false);
    }
}
