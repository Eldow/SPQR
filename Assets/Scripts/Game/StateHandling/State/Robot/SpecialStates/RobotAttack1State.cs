﻿public class RobotAttack1State : RobotAttackState {
    protected override void Initialize() {
        this.AlreadyHitByAttack = false;
        this.MaxFrame = 30;
        this.IASA = 21;
        this.MinActiveState = 7;
        this.MaxActiveState = 13;
        this.Damage = 5;
        this.Hitstun = 10;
        this.HeatCost = 3;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;
		
        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
		
        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack1")) {
            return null;
        }

        if (inputManager.attackButton()) {
                return new RobotAttack2State();
        }

        if (this.IsDischarge(robotStateMachine)) {
            return new RobotDischargeState();
        }

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions(stateMachine);

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public RobotAttack1State() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Exit(StateMachine stateMachine) {
    }

    public override RobotState CheckInterruptibleActions(StateMachine stateMachine) {
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
        if (inputManager.moveX() > .02f || inputManager.moveY() > .02f) {
            if (inputManager.runButton()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        return null;
    }
}
