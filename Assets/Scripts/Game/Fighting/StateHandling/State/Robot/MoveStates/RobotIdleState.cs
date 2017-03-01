using UnityEngine;

public class RobotIdleState : RobotState {
    public override string HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        if (!this.IsAnimationPlaying((RobotStateMachine)stateMachine, 
            "RobotIdle")) {
            return null;
        }

        if (InputManager.attackButton()) {
            return typeof(RobotAttack1State).Name;
        }

        if (InputManager.blockButton()) {
            return typeof(RobotBlockState).Name;
        }

        if (Mathf.Abs(InputManager.moveX()) <= 0.2f &&
            Mathf.Abs(InputManager.moveY()) <= 0.2f) {
            return null;
        }

        if (InputManager.runButton()) {
            return typeof(RobotRunState).Name;
        }

        return typeof(RobotWalkState).Name;
    }

    public RobotIdleState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine)stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {

        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine)stateMachine);
    }
}
