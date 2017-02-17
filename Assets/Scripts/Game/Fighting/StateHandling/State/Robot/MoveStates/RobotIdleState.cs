using UnityEngine;

public class RobotIdleState : RobotState {
    public override State HandleInput(StateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!(stateMachine is RobotStateMachine)) return null;

        if (!this.IsAnimationPlaying((RobotStateMachine)stateMachine, 
            "RobotIdle")) {
            return null;
        }

        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttack1State();
        }

        if (Input.GetKeyDown(xboxInput.B)) {
            return new RobotBlockState();
        }

        if (Mathf.Abs(xboxInput.getLeftStickX()) <= 0.2f &&
            Mathf.Abs(xboxInput.getLeftStickY()) <= 0.2f) {
            return null;
        }

        if (xboxInput.RT()) {
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
