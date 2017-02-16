using UnityEngine;

public class RobotRunState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (Input.GetKeyDown(xboxInput.A)) {
            Debug.Log("Can't attack while running!");
            return null;
        }

        if (Input.GetKeyDown(xboxInput.B)) {
            return new RobotBlockState();
        }

        if (Mathf.Abs(xboxInput.getLeftStickX()) <= 0.2f &&
            Mathf.Abs(xboxInput.getLeftStickY()) <= 0.2f) {
            return new RobotIdleState();
        } else {
            return !xboxInput.RT() ? new RobotWalkState() : null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.RunMovement();
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("RUN ENTER!");
        stateMachine.Animator.SetBool("IsRun", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("RUN EXIT!");
        stateMachine.Animator.SetBool("IsRun", false);
    }
}
