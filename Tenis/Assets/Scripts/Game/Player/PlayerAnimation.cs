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
    private static readonly int Serve = Animator.StringToHash("serve");

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

        _animator.SetTrigger(Hit);
        _animator.SetInteger(BallSide,(int) side);
    }

    public static void StartServeAnimation()
    {
        _animator.SetTrigger(Serve);
    }

    public static bool IsPlayingHitAnimation()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        return (currentState.IsName("service") || currentState.IsName("drive") || currentState.IsName("backhand"));



    }
}
