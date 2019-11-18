using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Score;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimation
{
    private Animator _animator;

    private static readonly int Direction = Animator.StringToHash("direction");
    private static readonly int Hitting = Animator.StringToHash("hitting");
    private static readonly int BallSide = Animator.StringToHash("ballSide");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int Serve = Animator.StringToHash("serve");
    private static readonly int EndHit = Animator.StringToHash("endHit");
    private static readonly int Celebbrate = Animator.StringToHash("celebrate");
    private static readonly int Angry = Animator.StringToHash("angry");
    private static readonly int CelebrationNumber = Animator.StringToHash("celebrationNumber");
    private static readonly int AngryNumber = Animator.StringToHash("angryNumber");

    public PlayerAnimation(Animator animator)
    {
        //validate _animator not null?
        _animator = animator;
    }

    public void AnimateMovement(float leftRightMove, float forwardBackardMove)
    {
        if (leftRightMove > 0)
        {
            StartMoveAnimation(MovementDirection.RIGHT);
        }
        else if(leftRightMove < 0)
        {
            StartMoveAnimation(MovementDirection.LEFT);
        }
        else if(forwardBackardMove > 0)
        {
           StartMoveAnimation(MovementDirection.UP);
        }
        else if(forwardBackardMove < 0)
        {
            StartMoveAnimation(MovementDirection.DOWN);
        }
        else
        {
            StartMoveAnimation(MovementDirection.IDLE);
        }
    }
    public void StartMoveAnimation(MovementDirection direction)
    {
        _animator.SetInteger(Direction, (int) direction);
    }

    public void StartHittingAnimation(Side side)
    {
        _animator.SetBool(EndHit, false);
        _animator.SetTrigger(Hit);
        _animator.SetInteger(BallSide,(int) side);
    }

    public void EndHittingAnimation()
    {
        _animator.SetBool(EndHit, true);
    }

    public void StartServeAnimation()
    {
        _animator.SetTrigger(Serve);
    }
    
    public void StartCelebrateAnimation()
    {
        int celebrationNumber = (int) Math.Round(Random.Range(1.0f, 3.99f));
        _animator.SetTrigger(Celebbrate);
        _animator.SetInteger(CelebrationNumber, celebrationNumber);
    }

    public void StartAngryAnimation()
    {
        int angryNumber = (int) Math.Round(Random.Range(1.0f, 2.99f));
        _animator.SetTrigger(Angry);
        _animator.SetInteger(AngryNumber, angryNumber);
    }
    public bool IsPlayingHitAnimation()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName("service");
//        return (currentState.IsName("service") || currentState.IsName("drive") || currentState.IsName("backhand"));
    }
    public bool IsStuckOnHitAnimation()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        return (currentState.IsName("drive_start") || currentState.IsName("backhand_start"));
    }
}
