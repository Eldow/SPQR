using System;
using UnityEngine;

public class RobotBlockState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotBlock")) {
            return null;
        }

        if (this.CheckIfBlockHolding(xboxInput)) {
            if (this.IsCurrentAnimationPlayedPast(stateMachine, .5f) && 
                Math.Abs(stateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(stateMachine);
            }

            return null;
        }

        this.ResumeAnimation(stateMachine);

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            return new RobotIdleState();
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {

    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK ENTER!");
        stateMachine.Animator.SetBool("IsBlock", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK EXIT!");
        stateMachine.Animator.SetBool("IsBlock", false);
    }

    protected virtual bool CheckIfBlockHolding(XboxInput xboxInput) {
        return Input.GetKey(xboxInput.B);
    }
}
