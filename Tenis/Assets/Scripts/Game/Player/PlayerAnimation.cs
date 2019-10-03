using System.Collections;
using System.Collections.Generic;
using Game.Score;
using UnityEngine;

public class PlayerAnimation
{
    private static Animator _animator;

    private static readonly int Direction = Animator.StringToHash("direction");
    private static readonly int Hitting = Animator.StringToHash("hitting");
    private static readonly int BallSide = Animator.StringToHash("ballSide");
    private static readonly int Hit = Animator.StringToHash("hit");

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

    public static void StartHittingAnimation(Side side)
    {
        //validate _animator not null?
//        _animator.SetInteger(BallSide, (int) side);
Debug.Log("hitting");
        _animator.SetTrigger(Hit);
    }
    
    public static void StopHittingAnimation()
    {
        //validate _animator not null?
        _animator.SetTrigger(Hit);
    }
}
