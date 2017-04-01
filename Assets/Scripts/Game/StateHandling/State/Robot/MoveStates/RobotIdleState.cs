using UnityEngine;

public class RobotIdleState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;
		
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
		
        if (!this.IsAnimationPlaying((RobotStateMachine) stateMachine,
            "RobotIdle")) {
            return null;
        }

        if (inputManager.attackButton()) {
            return new RobotAttack1State();
        }

        if (inputManager.blockButton()) {
            return new RobotBlockState();
        }

        if (inputManager.powerAttackButtonDown()) {
            return new RobotPowerAttackState();
        }

        if (inputManager.dashButton()) {
            return new RobotDashState();
        }

        /*if (Mathf.Abs(inputManager.moveX()) <= 0.2f &&
            Mathf.Abs(inputManager.moveY()) <= 0.2f) {
            return null;
        }*/

        if (Mathf.Abs(inputManager.moveX()) <= 0.2f &&
            Mathf.Abs(inputManager.moveY()) <= 0.2f) {
            return null;
        }

        if ((Mathf.Abs(inputManager.moveX()) <= 0.2f &&
             Mathf.Abs(inputManager.moveY()) <= 0.2f) && inputManager.runButton()) {
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
        this.ResumeNormalAnimation((RobotStateMachine) stateMachine);
    }
}
