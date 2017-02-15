using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttackState : RobotState {
    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotAttack")) {
            return null;
        }

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            return new RobotWalkState();
            /* Will return a Walk State which will shift to an Idle State
            if necessary. The opposite won't work if we attack being in an
            Idle State and start moving in the same time */
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.UnlockedMovement();
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK ENTER!");
        stateMachine.Animator.SetBool("IsAttack", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK EXIT!");
        stateMachine.Animator.SetBool("IsAttack", false);
    }
}
