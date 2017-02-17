using UnityEngine;

public class RobotAttack1State : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotAttack1")) {
            return null;
        }

        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttack2State();
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
        Debug.Log("ATTACK1 ENTER!");
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK1 EXIT!");
    }
}
