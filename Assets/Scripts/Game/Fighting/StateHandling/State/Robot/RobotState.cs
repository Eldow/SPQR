using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotState : State {
    public virtual RobotState HandleInput(RobotStateMachine stateMachine, XboxInput xboxInput) {
        return null;
    }

    public virtual void Update(RobotStateMachine stateMachine) {

    }

    public virtual void Enter(RobotStateMachine stateMachine) {

    }

    public virtual void Exit(RobotStateMachine stateMachine) {

    }

    public virtual bool IsAnimationPlaying(RobotStateMachine stateMachine, 
        string animationName) {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .IsName(animationName);
    }

    public virtual bool IsCurrentAnimationFinished(
        RobotStateMachine stateMachine) {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
                   .normalizedTime > 1 &&
               !stateMachine.Animator.IsInTransition(0);
    }
}
