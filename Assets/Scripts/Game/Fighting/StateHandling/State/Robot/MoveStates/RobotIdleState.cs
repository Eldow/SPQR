using UnityEngine;

public class RobotIdleState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        if (!this.IsAnimationPlaying((RobotStateMachine)stateMachine, 
            "RobotIdle")) {
            return null;
        }

		if (InputManager.attackButton()) {
            return new RobotAttack1State();
        }

		if (InputManager.blockButton()) {
            return new RobotBlockState();
        }

		if (Mathf.Abs(InputManager.moveX()) <= 0.2f &&
			Mathf.Abs(InputManager.moveY()) <= 0.2f) {
            return null;
        }

		if (InputManager.runButton()) {
            return new RobotRunState();
        }

        return new RobotWalkState();
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("IDLE ENTER!");
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine)stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("IDLE EXIT!");

        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine)stateMachine);
    }
}
