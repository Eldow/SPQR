using UnityEngine;

public class RobotAttack2State : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack2")) {
            return null;
        }

		if (InputManager.attackButton()) {
            return new RobotAttack3State();
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

        ((RobotStateMachine)stateMachine).PlayerController.Movement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("ATTACK2 ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("ATTACK2 EXIT!");
    }
}
