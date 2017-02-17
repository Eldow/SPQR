using UnityEngine;

public class RobotAttack2State : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine,
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotAttack2")) {
            return null;
        }

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            if (this.IsLastState(stateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.Movement();
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK2 ENTER!");
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK2 EXIT!");
    }
}
