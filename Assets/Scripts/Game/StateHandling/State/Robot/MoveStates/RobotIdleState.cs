using UnityEngine;

public class RobotIdleState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        if (!this.IsAnimationPlaying((RobotStateMachine) stateMachine,
            "RobotIdle")) {
            return null;
        }

        if (InputManager.attackButton()) {
            return new RobotAttack1State();
        }

        if (InputManager.blockButton()) {
            return new RobotBlockState();
        }

        if (InputManager.powerAttackButton()) {
            return new RobotPowerAttackState();
        }

        if (InputManager.dashButton()) {
            return new RobotDashState();
        }

        /*if (Mathf.Abs(InputManager.moveX()) <= 0.2f &&
            Mathf.Abs(InputManager.moveY()) <= 0.2f) {
            return null;
        }*/

        if (Mathf.Abs(InputManager.moveX()) <= 0.2f &&
            Mathf.Abs(InputManager.moveY()) <= 0.2f) {
            return null;
        }

        if ((Mathf.Abs(InputManager.moveX()) <= 0.2f &&
             Mathf.Abs(InputManager.moveY()) <= 0.2f) && InputManager.runButton()) {
            return new RobotRunState();
        }

        return new RobotWalkState();
    }

    public RobotIdleState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {}

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine) stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine) stateMachine);
    }
}
