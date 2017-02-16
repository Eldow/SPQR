using UnityEngine;

public class RobotAttackState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotAttack")) {
            return null;
        }

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            if (this.IsLastState(stateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.Movement();
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
