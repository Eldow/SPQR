using UnityEngine;

public class RobotAttack3State : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack3")) {
            return null;
        }

        if (this.IsCurrentAnimationFinished(robotStateMachine)) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Movement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("ATTACK3 ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("ATTACK3 EXIT!");
    }
}
